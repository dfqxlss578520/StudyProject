using System.Collections.Generic;
using System.Linq;
using Hyl.Core.Domain.Survey;
using Hyl.Repository;
using Hyl.Core.Domain.PageDomain;
using Hyl.Core.Helpers.Utility;
using Hyl.Core.Domain.Survey.ViewModel;
using Newtonsoft.Json;
using System.Text;

namespace Hyl.Service.Survey
{
    public class SurveyAnswerServices : ISurveyAnswerServices
    {
        private readonly IRepository<SurveyAnswer> _repository;
        private readonly IRepository<AnswerRadio> _repositoryAnswerRadio;
        private readonly IRepository<AnswerCheckbox> _repositoryAnswerCheckbox;
        private readonly IRepository<AnswerFillblank> _repositoryAnswerFillblank;
        private readonly IRepository<AnswerScore> _repositoryAnswerScore;
        private readonly IRepository<AnswerOrder> _repositoryAnswerOrder;
        private readonly IRepository<AnswerMultiFillblank> _repositoryAnswerMultiFillblank;
        private readonly IRepository<AnswerChenRadio> _repositoryAnswerChenRadio;
        private readonly IRepository<AnswerChenCheckbox> _repositoryAnswerChenCheckbox;
        private readonly IRepository<AnswerChenScore> _repositoryAnswerChenScore;
        private readonly IRepository<AnswerChenFillblank> _repositoryAnswerChenFillblank;
        private readonly IQuestionServices _questionServices;
        private readonly ISurveyDirectoryServices _repositorySurveyDirectory;
        private readonly IDbQuery _dbQuery;
        public SurveyAnswerServices(IRepository<SurveyAnswer> repository,
                    IRepository<AnswerRadio> repositoryAnswerRadio,
                    IRepository<AnswerCheckbox> repositoryAnswerCheckbox,
                    IRepository<AnswerFillblank> repositoryAnswerFillblank,
                    IRepository<AnswerScore> repositoryAnswerScore,
                    IRepository<AnswerOrder> repositoryAnswerOrder,
                    IRepository<AnswerMultiFillblank> repositoryAnswerMultiFillblank,
                    IRepository<AnswerChenRadio> repositoryAnswerChenRadio,
                    IRepository<AnswerChenCheckbox> repositoryAnswerChenCheckbox,
                    IRepository<AnswerChenScore> repositoryAnswerChenScore,
                    ISurveyDirectoryServices repositorySurveyDirectory,
                    IRepository<AnswerChenFillblank> repositoryAnswerChenFillblank,
                    IQuestionServices questionServices,
                    IDbQuery dbQuery)
        {
            _repository = repository;
            _dbQuery = dbQuery;
            _repositoryAnswerRadio = repositoryAnswerRadio;
            _repositoryAnswerCheckbox = repositoryAnswerCheckbox;
            _repositoryAnswerFillblank = repositoryAnswerFillblank;
            _repositoryAnswerScore = repositoryAnswerScore;
            _repositoryAnswerOrder = repositoryAnswerOrder;
            _repositoryAnswerMultiFillblank = repositoryAnswerMultiFillblank;
            _repositoryAnswerChenRadio = repositoryAnswerChenRadio;
            _repositoryAnswerChenCheckbox = repositoryAnswerChenCheckbox;
            _repositoryAnswerChenScore = repositoryAnswerChenScore;
            _repositoryAnswerChenFillblank = repositoryAnswerChenFillblank;
            _questionServices = questionServices;
            _repositorySurveyDirectory = repositorySurveyDirectory;
        }
        

        public Question GetChart(long questionid)
        {
            var questions = new List<Question>() {
                _questionServices.Get(questionid)
            };
            List<AnswerDataCross> dataCrosses = new List<AnswerDataCross>();
            if (questions.Count > 0)
            {
                //分问题类型查询
                var groups = from p in questions group p by p.QuType into g select g;
                foreach (IGrouping<QuType, Question> groupItem in groups)
                {
                    switch (groupItem.Key)
                    {
                        //所有类型处理逻辑类似，仅第一项做注释解析
                        case QuType.RADIO:
                            #region 查出radio类型的回答问题统计数，遍历每个单选项，然后分别赋值                            
                            var radioItemAnswerlist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuItemId GroupKey,COUNT(QuItemId) GroupCount FROM dbo.AnswerRadio WHERE QuId IN @quid AND IsValid =1 GROUP BY QuItemId;", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)//从类型组里取出单个问题
                            {
                                question.QuName = Utils.UrlDecode(question.QuName);
                                List<AnswerDataCross> crosses = new List<AnswerDataCross>();
                                foreach (QuestionRadio item in question.QuRadios)//从问题中取出单个单选按钮选项
                                {
                                    crosses.Add(new AnswerDataCross()
                                    {
                                        OptionName = Utils.UrlDecode(question.QuName),
                                        Count = (radioItemAnswerlist.FirstOrDefault(p => p.GroupKey == item.Id) ?? new AnswerGroupViewModel()).GroupCount
                                    });
                                }
                                question.StatJson = JsonConvert.SerializeObject(crosses);
                            }
                            #endregion
                            break;
                        case QuType.CHECKBOX:
                            #region 逻辑同上
                            var checklist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuItemId GroupKey,COUNT(QuItemId) GroupCount FROM dbo.AnswerCheckbox WHERE QuId IN @quid AND IsValid =1 GROUP BY QuItemId;", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.UrlDecode(question.QuName);
                                List<AnswerDataCross> crosses = new List<AnswerDataCross>();
                                foreach (QuestionCheckbox item in question.QuCheckboxes)
                                {
                                    crosses.Add(new AnswerDataCross()
                                    {
                                        OptionName = Utils.UrlDecode(question.QuName),
                                        Count = (checklist.FirstOrDefault(p => p.GroupKey == item.Id) ?? new AnswerGroupViewModel()).GroupCount
                                    });
                                }
                                question.StatJson = JsonConvert.SerializeObject(crosses);
                            }
                            #endregion
                            break;
                        case QuType.FILLBLANK:
                            #region 逻辑同上
                            var fbklist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuId, COUNT(CASE WHEN ISNULL(Answers,'') ='' THEN 1 END) EmptyCount,COUNT(CASE WHEN ISNULL(Answers,'') !='' THEN 1 END) NoEmptyCount FROM dbo.AnswerFillblank WHERE QuId IN @quid AND IsValid =1 GROUP BY QuId", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.UrlDecode(question.QuName);
                                question.EmptyCount = (fbklist.FirstOrDefault(p => p.GroupKey == question.Id) ?? new AnswerGroupViewModel()).EmptyCount;
                                question.NoEmptyCount = (fbklist.FirstOrDefault(p => p.GroupKey == question.Id) ?? new AnswerGroupViewModel()).NoEmptyCount;
                            }
                            #endregion
                            break;
                        case QuType.MULTIFILLBLANK:
                            #region 逻辑同上
                            var mulitFillBlanklist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuItemId GroupKey,COUNT(Id) GroupCount FROM dbo.AnswerMultiFillblank WHERE QuId IN @quid AND IsValid =1 GROUP BY QuItemId", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.UrlDecode(question.QuName);
                                List<AnswerDataCross> crosses = new List<AnswerDataCross>();
                                foreach (QuestionMultiFillblank item in question.QuMultiFillblanks)
                                {
                                    crosses.Add(new AnswerDataCross()
                                    {
                                        OptionName = Utils.UrlDecode(question.QuName),
                                        Count = (mulitFillBlanklist.FirstOrDefault(p => p.GroupKey == item.Id) ?? new AnswerGroupViewModel()).GroupCount
                                    });
                                }
                                question.StatJson = JsonConvert.SerializeObject(crosses);
                            }
                            #endregion
                            break;
                        case QuType.CHENRADIO:
                            #region 逻辑同上
                            var chenradiolist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuRowId GroupKey, QuColId GroupColKey,COUNT(Id) GroupCount FROM dbo.AnswerChenRadio WHERE QuId IN @quid AND IsValid =1 GROUP BY QuRowId, QuColId", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.UrlDecode(question.QuName);
                                //List<AnswerDataCross> crosses = new List<AnswerDataCross>();
                                //foreach (QuestionChenRow item in question.QuChenRows)
                                //{
                                //    crosses.Add(new AnswerDataCross()
                                //    {
                                //        OptionName = Utils.UrlDecode(question.QuName),
                                //        Count = (chenradiolist.FirstOrDefault(p => p.GroupKey == item.Id) ?? new AnswerGroupViewModel()).GroupCount
                                //    });

                                //    item.AnswerCount = (chenradiolist.FirstOrDefault(p => p.GroupKey == item.Id) ?? new AnswerGroupViewModel()).GroupCount;
                                //}
                                //question.StatJson = JsonConvert.SerializeObject(crosses);
                            }
                            #endregion
                            break;
                    }
                }
            }

            var rst = questions.FirstOrDefault();
            rst.StatJson = JsonConvert.SerializeObject(dataCrosses);
            return rst;
        }

        public long SaveAnswer(SurveyAnswer model)
        {
            var answerId = _repository.Add(model);
            if (answerId > 0)
            {
                model.AnswerRadio.ForEach(p => p.BelongAnswerId = answerId);
                _repositoryAnswerRadio.AddBatch(model.AnswerRadio);

                model.AnswerCheckbox.ForEach(p => p.BelongAnswerId = answerId);
                _repositoryAnswerCheckbox.AddBatch(model.AnswerCheckbox);

                model.AnswerFillblank.ForEach(p => p.BelongAnswerId = answerId);
                _repositoryAnswerFillblank.AddBatch(model.AnswerFillblank);

                model.AnswerScore.ForEach(p => p.BelongAnswerId = answerId);
                _repositoryAnswerScore.AddBatch(model.AnswerScore);

                model.AnswerOrder.ForEach(p => p.BelongAnswerId = answerId);
                _repositoryAnswerOrder.AddBatch(model.AnswerOrder);


                model.AnswerMultiFillblank.ForEach(p => p.BelongAnswerId = answerId);
                _repositoryAnswerMultiFillblank.AddBatch(model.AnswerMultiFillblank);

                model.AnswerChenRadio.ForEach(p => p.BelongAnswerId = answerId);
                _repositoryAnswerChenRadio.AddBatch(model.AnswerChenRadio);

                model.AnswerChenCheckbox.ForEach(p => p.BelongAnswerId = answerId);
                _repositoryAnswerChenCheckbox.AddBatch(model.AnswerChenCheckbox);

                model.AnswerChenScore.ForEach(p => p.BelongAnswerId = answerId);
                _repositoryAnswerChenScore.AddBatch(model.AnswerChenScore);

                model.AnswerChenFillblank.ForEach(p => p.BelongAnswerId = answerId);
                _repositoryAnswerChenFillblank.AddBatch(model.AnswerChenFillblank);

                _dbQuery.ExecuteSql("UPDATE dbo.SurveyDirectory SET AnswerNum =AnswerNum + 1 WHERE IsValid=1 AND id=@dirid;", new { dirid = model.DirId });

                return answerId;
            }
            return 0;
        }

        public Page<SurveyAnswer> GetPageSurveyAnswer(Page<SurveyAnswer> page)
        {
            return _repository.GetPaged(page);
        }

        public List<Question> GetStatistics(SurveyDirectory survey)
        {
            var questions = _questionServices.Get(survey.Id);
            if (questions.Count > 0)
            {
                //分问题类型查询
                var groups = from p in questions group p by p.QuType into g select g;
                foreach (IGrouping<QuType, Question> groupItem in groups)
                {
                    switch (groupItem.Key)
                    {
                        //所有类型处理逻辑类似，仅第一项做注释解析
                        case QuType.RADIO:
                            #region 一次性查出所有radio类型的回答问题统计数，遍历每个单选项，然后分别赋值                            
                            var radioItemAnswerlist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuItemId GroupKey,COUNT(QuItemId) GroupCount FROM dbo.AnswerRadio WHERE QuId IN @quid AND IsValid =1 GROUP BY QuItemId;", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)//从类型组里取出单个问题
                            {
                                question.QuName = Utils.UrlDecode(question.QuName);
                                foreach (QuestionRadio item in question.QuRadios)//从问题中取出单个单选按钮选项
                                {
                                    item.AnswerCount = (radioItemAnswerlist.FirstOrDefault(p => p.GroupKey == item.Id) ?? new AnswerGroupViewModel()).GroupCount;
                                }
                            }
                            #endregion
                            break;
                        case QuType.CHECKBOX:
                            #region 逻辑同上
                            var checklist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuItemId GroupKey,COUNT(QuItemId) GroupCount FROM dbo.AnswerCheckbox WHERE QuId IN @quid AND IsValid =1 GROUP BY QuItemId;", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.UrlDecode(question.QuName);
                                foreach (QuestionCheckbox item in question.QuCheckboxes)
                                {
                                    item.AnswerCount = (checklist.FirstOrDefault(p => p.GroupKey == item.Id) ?? new AnswerGroupViewModel()).GroupCount;
                                    question.AnswerCount += item.AnswerCount;
                                }
                            }
                            #endregion
                            break;
                        case QuType.SCORE:
                            #region 逻辑同上
                            var scorelist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuRowId GroupKey,COUNT(QuRowId) GroupCount,AVG(AnswserScore) AvgNum FROM dbo.AnswerScore WHERE QuId IN @quid AND IsValid =1 GROUP BY QuRowId;", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.UrlDecode(question.QuName);
                                foreach (QuestionScore item in question.QuScores)
                                {
                                    var model = scorelist.FirstOrDefault(p => p.GroupKey == item.Id) ?? new AnswerGroupViewModel();
                                    item.AnswerCount = model.GroupCount;
                                    item.AvgNum = model.AvgNum;
                                    question.AnswerCount += item.AnswerCount;
                                }
                            }
                            #endregion
                            break;
                        case QuType.ORDERQU:
                            #region 逻辑同上
                            var orderbylist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuRowId GroupKey,SUM(OrderyNum) SumNum FROM dbo.AnswerOrder WHERE QuId IN @quid AND IsValid =1 GROUP BY QuRowId", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)
                            {
                                question.QuOrderbies = question.QuOrderbies.OrderByDescending(p => p.SumNum).ToList();
                                question.QuName = Utils.UrlDecode(question.QuName);
                                foreach (QuestionOrderby item in question.QuOrderbies)
                                {
                                    item.SumNum = (orderbylist.FirstOrDefault(p => p.GroupKey == item.Id) ?? new AnswerGroupViewModel()).SumNum;
                                }
                            }
                            #endregion
                            break;
                        case QuType.FILLBLANK:
                            #region 逻辑同上
                            var fbklist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuId GroupKey, COUNT(CASE WHEN ISNULL(Answers,'') ='' THEN 1 END) EmptyCount,COUNT(CASE WHEN ISNULL(Answers,'') !='' THEN 1 END) NoEmptyCount FROM dbo.AnswerFillblank WHERE QuId IN @quid AND IsValid =1 GROUP BY QuId", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.UrlDecode(question.QuName);
                                question.EmptyCount = (fbklist.FirstOrDefault(p => p.GroupKey == question.Id) ?? new AnswerGroupViewModel()).EmptyCount;
                                question.NoEmptyCount = (fbklist.FirstOrDefault(p => p.GroupKey == question.Id) ?? new AnswerGroupViewModel()).NoEmptyCount;
                                question.AnswerCount = question.NoEmptyCount;
                            }
                            #endregion
                            break;
                        case QuType.MULTIFILLBLANK:
                            #region 逻辑同上
                            var mulitFillBlanklist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuItemId GroupKey,COUNT(Id) GroupCount FROM dbo.AnswerMultiFillblank WHERE QuId IN @quid AND IsValid =1 GROUP BY QuItemId", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.UrlDecode(question.QuName);
                                foreach (QuestionMultiFillblank item in question.QuMultiFillblanks)
                                {
                                    item.AnswerCount = (mulitFillBlanklist.FirstOrDefault(p => p.GroupKey == item.Id) ?? new AnswerGroupViewModel()).GroupCount;
                                }
                            }
                            #endregion
                            break;
                        case QuType.CHENRADIO:
                            #region 逻辑同上
                            var chenradiolist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuRowId GroupKey, QuColId GroupColKey,COUNT(Id) GroupCount FROM dbo.AnswerChenRadio WHERE QuId IN @quid AND IsValid =1 GROUP BY QuRowId, QuColId", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.UrlDecode(question.QuName);
                                foreach (QuestionChenRow item in question.QuChenRows)
                                {
                                    item.AnswerCount = chenradiolist.Where(p => p.GroupKey == item.Id).Sum(p => p.GroupCount);
                                    question.AnswerCount += item.AnswerCount;
                                }
                                foreach (var answerItem in chenradiolist)
                                {
                                    question.AnChenRadio.Add(new AnswerChenRadio()
                                    {
                                        QuRowId = answerItem.GroupKey,
                                        QuColId = answerItem.GroupColKey,
                                        Count = answerItem.GroupCount
                                    });
                                }
                            }
                            #endregion
                            break;
                        case QuType.CHENCHECKBOX:
                            #region 逻辑同上
                            var chencheckboxlist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuRowId GroupKey, QuColId GroupColKey,COUNT(Id) GroupCount FROM dbo.AnswerChenCheckbox WHERE QuId IN @quid AND IsValid =1 GROUP BY QuRowId, QuColId", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.UrlDecode(question.QuName);
                                foreach (QuestionChenRow item in question.QuChenRows)
                                {
                                    item.AnswerCount = chencheckboxlist.Where(p => p.GroupKey == item.Id).Sum(p => p.GroupCount);
                                    question.AnswerCount += item.AnswerCount;
                                }
                                foreach (var answerItem in chencheckboxlist)
                                {
                                    question.AnChenCheckbox.Add(new AnswerChenCheckbox()
                                    {
                                        QuRowId = answerItem.GroupKey,
                                        QuColId = answerItem.GroupColKey,
                                        Count = answerItem.GroupCount
                                    });
                                }
                            }
                            #endregion
                            break;
                        case QuType.CHENSCORE:
                            #region 逻辑同上
                            //SELECT GroupKey, GroupColKey, AVG(totalNum) AvgNum FROM(SELECT QuRowId GroupKey , QuColId GroupColKey, SUM(AnswserScore) totalNum FROM dbo.AnswerChenScore WHERE QuId IN @quid AND IsValid = 1 GROUP BY QuRowId , QuColId)AS t GROUP BY GroupKey , GroupColKey
                            var chenscorelist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuRowId GroupKey, QuColId GroupColKey,ROUND(AVG(CONVERT(DECIMAL(18,2),AnswserScore)),2) AvgNum FROM dbo.AnswerChenScore WHERE QuId IN @quid AND IsValid =1 GROUP BY QuRowId, QuColId", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.UrlDecode(question.QuName);
                                foreach (var answerItem in chenscorelist)
                                {
                                    question.AnChenScore.Add(new AnswerChenScore()
                                    {
                                        QuRowId = answerItem.GroupKey,
                                        QuColId = answerItem.GroupColKey,
                                        AvgScore = answerItem.AvgNum
                                    });
                                }
                            }
                            #endregion
                            break;
                        case QuType.CHENFBK:
                            #region 逻辑同上
                            var chenFBKlist = _dbQuery.QueryList<AnswerGroupViewModel>("SELECT QuRowId GroupKey, QuColId GroupColKey,COUNT(Id) GroupCount FROM dbo.AnswerChenFillblank WHERE QuId IN @quid AND IsValid =1 GROUP BY QuRowId, QuColId", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.UrlDecode(question.QuName);
                                foreach (QuestionChenRow item in question.QuChenRows)
                                {
                                    item.AnswerCount = chenFBKlist.Where(p => p.GroupKey == item.Id).Sum(p => p.GroupCount);
                                    question.AnswerCount += item.AnswerCount;
                                }
                                foreach (var answerItem in chenFBKlist)
                                {
                                    question.AnChenFillblank.Add(new AnswerChenFillblank()
                                    {
                                        QuRowId = answerItem.GroupKey,
                                        QuColId = answerItem.GroupColKey,
                                        Count = answerItem.GroupCount
                                    });
                                }
                            }
                            #endregion
                            break;
                    }
                }
            }
            return questions;
        }

        public SurveyDirectory GetAnswers(int answerId)
        {
            SurveyAnswer answer = _repository.Get(answerId);
            if (answer != null)
            {
                var directory = _repositorySurveyDirectory.GetDirectoryDetailStyle(answer.DirId);
                var questions = directory.Questions;
                if (questions.Count > 0)
                {
                    //分问题类型查询
                    var groups = from p in questions group p by p.QuType into g select g;
                    foreach (IGrouping<QuType, Question> groupItem in groups)
                    {
                        switch (groupItem.Key)
                        {
                            //所有类型处理逻辑类似，仅第一项做注释解析
                            case QuType.RADIO:
                                #region 一次性查出所有radio类型的回答问题统计数，遍历每个单选项，然后分别赋值                            
                                var radioItemAnswerlist = _dbQuery.QueryList<AnswerRadio>("SELECT * FROM dbo.AnswerRadio WHERE QuId in @quid AND BelongAnswerId = @answerid AND IsValid=1;", new
                                {
                                    quid = groupItem.Select(p => p.Id),
                                    answerid = answer.Id
                                }).ToList();
                                foreach (Question question in groupItem)//从类型组里取出单个问题
                                {
                                    question.QuName = Utils.UrlDecode(question.QuName);
                                    foreach (var radioAnswerItem in radioItemAnswerlist.Where(p => p.QuId == question.Id))
                                    {
                                        question.AnRadios.Add(radioAnswerItem);
                                    }
                                }
                                #endregion
                                break;
                            case QuType.CHECKBOX:
                                #region 逻辑同上
                                var checklist = _dbQuery.QueryList<AnswerCheckbox>("SELECT * FROM dbo.AnswerCheckbox WHERE QuId in @quid AND BelongAnswerId = @answerid AND IsValid=1;", new
                                {
                                    quid = groupItem.Select(p => p.Id),
                                    answerid = answer.Id
                                }).ToList();
                                foreach (Question question in groupItem)//从类型组里取出单个问题
                                {
                                    question.QuName = Utils.UrlDecode(question.QuName);
                                    foreach (var checkAnswerItem in checklist.Where(p => p.QuId == question.Id))
                                    {
                                        question.AnCheckboxs.Add(checkAnswerItem);
                                    }
                                }
                                #endregion
                                break;
                            case QuType.SCORE:
                                #region 逻辑同上
                                var scorelist = _dbQuery.QueryList<AnswerScore>("SELECT * FROM dbo.AnswerScore WHERE QuId in @quid AND BelongAnswerId = @answerid AND IsValid=1;", new
                                {
                                    quid = groupItem.Select(p => p.Id),
                                    answerid = answer.Id
                                }).ToList();
                                foreach (Question question in groupItem)//从类型组里取出单个问题
                                {
                                    question.QuName = Utils.UrlDecode(question.QuName);
                                    foreach (var scoreAnswerItem in scorelist.Where(p => p.QuId == question.Id))
                                    {
                                        question.AnScores.Add(scoreAnswerItem);
                                    }
                                }
                                #endregion
                                break;
                            case QuType.ORDERQU:
                                #region 逻辑同上
                                var orderbylist = _dbQuery.QueryList<AnswerOrder>("SELECT * FROM dbo.AnswerOrder WHERE QuId in @quid AND BelongAnswerId = @answerid AND IsValid=1;", new
                                {
                                    quid = groupItem.Select(p => p.Id),
                                    answerid = answer.Id
                                }).ToList();
                                foreach (Question question in groupItem)//从类型组里取出单个问题
                                {
                                    question.QuName = Utils.UrlDecode(question.QuName);
                                    foreach (var orderAnswerItem in orderbylist.Where(p => p.QuId == question.Id))
                                    {
                                        question.AnOrders.Add(orderAnswerItem);
                                    }
                                }
                                #endregion
                                break;
                            case QuType.FILLBLANK:
                                #region 逻辑同上
                                var fbklist = _dbQuery.QueryList<AnswerFillblank>("SELECT * FROM dbo.AnswerFillblank WHERE QuId in @quid AND BelongAnswerId = @answerid AND IsValid=1;", new
                                {
                                    quid = groupItem.Select(p => p.Id),
                                    answerid = answer.Id
                                }).ToList();
                                foreach (Question question in groupItem)//从类型组里取出单个问题
                                {
                                    question.QuName = Utils.UrlDecode(question.QuName);
                                    foreach (var fbAnswerItem in fbklist.Where(p => p.QuId == question.Id))
                                    {
                                        question.AnFillblanks.Add(fbAnswerItem);
                                    }
                                }
                                #endregion
                                break;
                            case QuType.MULTIFILLBLANK:
                                #region 逻辑同上
                                var mulitFillBlanklist = _dbQuery.QueryList<AnswerMultiFillblank>("SELECT * FROM dbo.AnswerMultiFillblank WHERE QuId in @quid AND BelongAnswerId = @answerid AND IsValid=1;", new
                                {
                                    quid = groupItem.Select(p => p.Id),
                                    answerid = answer.Id
                                }).ToList();
                                foreach (Question question in groupItem)//从类型组里取出单个问题
                                {
                                    question.QuName = Utils.UrlDecode(question.QuName);
                                    foreach (var multifbAnswerItem in mulitFillBlanklist.Where(p => p.QuId == question.Id))
                                    {
                                        question.AnMultiFillblanks.Add(multifbAnswerItem);
                                    }
                                }
                                #endregion
                                break;
                            case QuType.CHENRADIO:
                                #region 逻辑同上
                                var chenradiolist = _dbQuery.QueryList<AnswerChenRadio>("SELECT * FROM dbo.AnswerChenRadio WHERE QuId in @quid AND BelongAnswerId = @answerid AND IsValid=1;", new
                                {
                                    quid = groupItem.Select(p => p.Id),
                                    answerid = answer.Id
                                }).ToList();
                                foreach (Question question in groupItem)//从类型组里取出单个问题
                                {
                                    question.QuName = Utils.UrlDecode(question.QuName);
                                    foreach (var chenRadioAnswerItem in chenradiolist.Where(p => p.QuId == question.Id))
                                    {
                                        question.AnChenRadio.Add(chenRadioAnswerItem);
                                    }
                                }
                                #endregion
                                break;
                            case QuType.CHENCHECKBOX:
                                #region 逻辑同上
                                var chencheckboxlist = _dbQuery.QueryList<AnswerChenCheckbox>("SELECT * FROM dbo.AnswerChenCheckbox WHERE QuId in @quid AND BelongAnswerId = @answerid AND IsValid=1;", new
                                {
                                    quid = groupItem.Select(p => p.Id),
                                    answerid = answer.Id
                                }).ToList();
                                foreach (Question question in groupItem)//从类型组里取出单个问题
                                {
                                    question.QuName = Utils.UrlDecode(question.QuName);
                                    foreach (var chencheckAnswerItem in chencheckboxlist.Where(p => p.QuId == question.Id))
                                    {
                                        question.AnChenCheckbox.Add(chencheckAnswerItem);
                                    }
                                }
                                #endregion
                                break;
                            case QuType.CHENSCORE:
                                #region 逻辑同上
                                var chenscorelist = _dbQuery.QueryList<AnswerChenScore>("SELECT * FROM dbo.AnswerChenScore WHERE QuId in @quid AND BelongAnswerId = @answerid AND IsValid=1;", new
                                {
                                    quid = groupItem.Select(p => p.Id),
                                    answerid = answer.Id
                                }).ToList();
                                foreach (Question question in groupItem)//从类型组里取出单个问题
                                {
                                    question.QuName = Utils.UrlDecode(question.QuName);
                                    foreach (var chenscoreAnswerItem in chenscorelist.Where(p => p.QuId == question.Id))
                                    {
                                        question.AnChenScore.Add(chenscoreAnswerItem);
                                    }
                                }
                                #endregion
                                break;
                            case QuType.CHENFBK:
                                #region 逻辑同上
                                var chenFBKlist = _dbQuery.QueryList<AnswerChenFillblank>("SELECT * FROM dbo.AnswerChenFillblank WHERE QuId in @quid AND BelongAnswerId = @answerid AND IsValid=1;", new
                                {
                                    quid = groupItem.Select(p => p.Id),
                                    answerid = answer.Id
                                }).ToList();
                                foreach (Question question in groupItem)//从类型组里取出单个问题
                                {
                                    question.QuName = Utils.UrlDecode(question.QuName);
                                    foreach (var chenFBKAnswerItem in chenFBKlist.Where(p => p.QuId == question.Id))
                                    {
                                        question.AnChenFillblank.Add(chenFBKAnswerItem);
                                    }
                                }
                                #endregion
                                break;
                        }
                    }
                }
                return directory;
            }
            return null;
        }

        public bool Delete(int answerId)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE dbo.SurveyDirectory SET SurveyQuNum = SurveyQuNum-1 WHERE IsValid =1 AND id IN (SELECT TOP 1 DirId FROM dbo.SurveyAnswer WHERE Id=@answerid)");
                sql.Append("UPDATE dbo.SurveyAnswer SET IsValid = 0 WHERE Id = @answerid AND IsValid =1;");

                sql.Append("UPDATE dbo.AnswerRadio SET IsValid =0 WHERE BelongAnswerId =@answerid AND IsValid=1;");
                sql.Append("UPDATE dbo.AnswerCheckbox SET IsValid =0 WHERE BelongAnswerId =@answerid AND IsValid=1;");
                sql.Append("UPDATE dbo.AnswerFillblank SET IsValid = 0 WHERE BelongAnswerId = @answerid AND IsValid = 1;");
                sql.Append("UPDATE dbo.AnswerScore SET IsValid =0 WHERE BelongAnswerId =@answerid AND IsValid=1;");
                sql.Append("UPDATE dbo.AnswerOrder SET IsValid =0 WHERE BelongAnswerId =@answerid AND IsValid=1;");

                sql.Append("UPDATE dbo.AnswerMultiFillblank SET IsValid =0 WHERE BelongAnswerId =@answerid AND IsValid=1;");
                sql.Append("UPDATE dbo.AnswerChenRadio SET IsValid =0 WHERE BelongAnswerId =@answerid AND IsValid=1;");
                sql.Append("UPDATE dbo.AnswerChenCheckbox SET IsValid =0 WHERE BelongAnswerId =@answerid AND IsValid=1;");
                sql.Append("UPDATE dbo.AnswerChenScore SET IsValid =0 WHERE BelongAnswerId =@answerid AND IsValid=1;");
                sql.Append("UPDATE dbo.AnswerChenFillblank SET IsValid =0 WHERE BelongAnswerId =@answerid AND IsValid=1;");

                _dbQuery.ExecuteSql(sql.ToString(), new { @answerid = answerId });
                return true;
            }
            catch
            {
                return false;

            }
        }
    }
}
