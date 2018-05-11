using System;
using System.Collections.Generic;
using System.Linq;
using Hyl.Core.Domain.Survey;
using Hyl.Core.Helpers.Utility;
using Hyl.Repository;

namespace Hyl.Service.Survey
{
    public class QuestionServices : IQuestionServices
    {
        private IDbQuery _dbQuery;
        private readonly IRepository<Question> _repository;
        private readonly IRepository<QuestionRadio> _quradioRepository;
        private readonly IRepository<QuestionCheckbox> _quCheckboxRepository;
        private readonly IRepository<QuestionMultiFillblank> _quMultiFillblankRepository;

        private readonly IRepository<QuestionScore> _quScoreRepository;
        private readonly IRepository<QuestionOrderby> _quOrderbyRepository;
        private readonly IRepository<QuestionChenRow> _quChenRowRepository;
        private readonly IRepository<QuestionChenColumn> _quChenColumnRepository;
        private readonly IRepository<QuestionLogic> _questionLogicRepository;

        public QuestionServices(IDbQuery dbQuery,
            IRepository<Question> repository,
            IRepository<QuestionRadio> quradioRepository,
            IRepository<QuestionCheckbox> quCheckboxRepository,
            IRepository<QuestionScore> quScoreRepository,
            IRepository<QuestionOrderby> quOrderbyRepository,
            IRepository<QuestionChenRow> quChenRowRepository,
            IRepository<QuestionChenColumn> quChenColumnRepository,
            IRepository<QuestionLogic> questionLogicRepository,
            IRepository<QuestionMultiFillblank> quMultiFillblankRepository)
        {
            _dbQuery = dbQuery;
            _repository = repository;
            _quradioRepository = quradioRepository;
            _quCheckboxRepository = quCheckboxRepository;
            _quMultiFillblankRepository = quMultiFillblankRepository;
            _quScoreRepository = quScoreRepository;
            _quOrderbyRepository = quOrderbyRepository;
            _quChenRowRepository = quChenRowRepository;
            _quChenColumnRepository = quChenColumnRepository;
            _questionLogicRepository = questionLogicRepository;
        }

        public Question Get(long questionid)
        {
            var questions = _dbQuery.Query<Question, QuestionLogic, Question>("SELECT q.id , q.Uid , q.DirId , q.QuName , q.QuTitle , q.QuNote , q.Keywords , q.QuType , q.Tag , q.OrderById , q.QuTag , q.ParentQuId , q.YesnoOption , q.IsRequired , q.CheckType , q.ParamInt01 , q.ParamInt02 , q.CopyFromId , q.Hv , q.RandOrder , q.CellCount , q.ContactsAttr , q.ContactsField , q.AnswerInputWidth , q.AnswerInputRow , q.CreateDate , q.IsValid ,l.id, l.QuId , l.Uid , l.QuItemId , l.SkipToQuId , l.LogicType , l.GeLe , l.ScoreNum , l.CreateDate , l.IsValid FROM dbo.Question q LEFT JOIN dbo.QuestionLogic l ON q.id = l.QuId WHERE q.Id = @quid AND q.IsValid = 1 ORDER BY q.OrderById ASC;",
                (question, logic) =>
                {
                    if (logic != null)
                    {
                        question.QuestionLogics.Add(logic);
                    }
                    return question;
                }, new { quid = questionid }, splitOn: "id").ToList();

            if (questions.Count > 0)
            {
                bool hasQueryChenGroup = false;
                var group = from p in questions group p by p.QuType into g select g;
                foreach (IGrouping<QuType, Question> groupItem in group)
                {
                    switch (groupItem.Key)
                    {
                        case QuType.RADIO:
                            var radiolist = _dbQuery.QueryList<QuestionRadio>("SELECT * FROM dbo.QuestionRadio WHERE QuId in @quid AND IsValid=1", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            radiolist.ForEach(p => p.OptionName = Utils.HtmlDecode(p.OptionName));
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.HtmlDecode(question.QuName);
                                question.QuRadios.AddRange(radiolist.Where(p => p.QuId == question.Id));
                            }
                            break;
                        case QuType.CHECKBOX:
                            var checklist = _dbQuery.QueryList<QuestionCheckbox>("SELECT * FROM dbo.QuestionCheckbox WHERE QuId in @quid AND IsValid=1", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            checklist.ForEach(p => p.OptionName = Utils.HtmlDecode(p.OptionName));
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.HtmlDecode(question.QuName);
                                question.QuCheckboxes.AddRange(checklist.Where(p => p.QuId == question.Id));
                            }
                            break;
                        case QuType.SCORE:
                            var scorelist = _dbQuery.QueryList<QuestionScore>("SELECT * FROM dbo.QuestionScore WHERE QuId in @quid AND IsValid=1", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            scorelist.ForEach(p => p.OptionName = Utils.HtmlDecode(p.OptionName));
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.HtmlDecode(question.QuName);
                                question.QuScores.AddRange(scorelist.Where(p => p.QuId == question.Id));
                            }
                            break;
                        case QuType.ORDERQU:
                            var orderbylist = _dbQuery.QueryList<QuestionOrderby>("SELECT * FROM dbo.QuestionOrderby WHERE QuId in @quid AND IsValid=1", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            orderbylist.ForEach(p => p.OptionName = Utils.HtmlDecode(p.OptionName));
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.HtmlDecode(question.QuName);
                                question.QuOrderbies.AddRange(orderbylist.Where(p => p.QuId == question.Id));
                            }
                            break;
                        case QuType.MULTIFILLBLANK:
                            var mulitFillBlanklist = _dbQuery.QueryList<QuestionMultiFillblank>("SELECT * FROM dbo.QuestionMultiFillblank WHERE QuId in @quid AND IsValid=1", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            mulitFillBlanklist.ForEach(p => p.OptionName = Utils.HtmlDecode(p.OptionName));
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.HtmlDecode(question.QuName);
                                question.QuMultiFillblanks.AddRange(mulitFillBlanklist.Where(p => p.QuId == question.Id));
                            }
                            break;
                        case QuType.CHENRADIO:
                        case QuType.CHENCHECKBOX:
                        case QuType.CHENSCORE:
                        case QuType.CHENFBK:
                            if (hasQueryChenGroup)
                            {
                                continue;
                            }
                            var multiItems = questions.Where(q => q.QuType == QuType.CHENRADIO || q.QuType == QuType.CHENCHECKBOX || q.QuType == QuType.CHENSCORE || q.QuType == QuType.CHENFBK).ToList();
                            var rowlist = _dbQuery.QueryList<QuestionChenRow>("SELECT * FROM dbo.QuestionChenRow WHERE QuId in @quid AND IsValid=1", new
                            {
                                quid = multiItems.Select(p => p.Id)
                            }).ToList();
                            rowlist.ForEach(p => p.OptionName = Utils.HtmlDecode(p.OptionName));
                            var columnlist = _dbQuery.QueryList<QuestionChenColumn>("SELECT * FROM dbo.QuestionChenColumn WHERE QuId in @quid AND IsValid=1", new
                            {
                                quid = multiItems.Select(p => p.Id)
                            }).ToList();
                            columnlist.ForEach(p => p.OptionName = Utils.HtmlDecode(p.OptionName));

                            foreach (Question question in multiItems)
                            {
                                question.QuName = Utils.HtmlDecode(question.QuName);
                                question.QuChenRows.AddRange(rowlist.Where(p => p.QuId == question.Id));
                                question.QuChenColumns.AddRange(columnlist.Where(p => p.QuId == question.Id));
                            }
                            hasQueryChenGroup = true;
                            break;
                        case QuType.PAGETAG:
                        case QuType.PARAGRAPH:
                        case QuType.FILLBLANK:
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.HtmlDecode(question.QuName);
                            }
                            break;
                    }
                }
            }
            return questions.FirstOrDefault();
        }

        public List<Question> Get(int surveyId)
        {
            var questions = _dbQuery.Query<Question, QuestionLogic, Question>("SELECT q.id , q.Uid , q.DirId , q.QuName , q.QuTitle , q.QuNote , q.Keywords , q.QuType , q.Tag , q.OrderById , q.QuTag , q.ParentQuId , q.YesnoOption , q.IsRequired , q.CheckType , q.ParamInt01 , q.ParamInt02 , q.CopyFromId , q.Hv , q.RandOrder , q.CellCount , q.ContactsAttr , q.ContactsField , q.AnswerInputWidth , q.AnswerInputRow , q.CreateDate , q.IsValid ,l.id, l.QuId , l.Uid , l.QuItemId , l.SkipToQuId , l.LogicType , l.GeLe , l.ScoreNum , l.CreateDate , l.IsValid FROM dbo.Question q LEFT JOIN dbo.QuestionLogic l ON q.id = l.QuId WHERE q.DirId = @dirid AND q.IsValid = 1 ORDER BY q.OrderById ASC;",
                (question, logic) =>
                {
                    if (logic != null)
                    {
                        question.QuestionLogics.Add(logic);
                    }
                    return question;
                }, new { dirid = surveyId }, splitOn: "id").ToList();

            if (questions.Count > 0)
            {
                bool hasQueryChenGroup = false;
                var group = from p in questions group p by p.QuType into g select g;
                foreach (IGrouping<QuType, Question> groupItem in group)
                {
                    switch (groupItem.Key)
                    {
                        case QuType.RADIO:
                            var radiolist = _dbQuery.QueryList<QuestionRadio>("SELECT * FROM dbo.QuestionRadio WHERE QuId in @quid AND IsValid=1 ORDER BY OrderById", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            radiolist.ForEach(p => p.OptionName = Utils.HtmlDecode(p.OptionName));
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.HtmlDecode(question.QuName);
                                question.QuRadios.AddRange(radiolist.Where(p => p.QuId == question.Id));
                            }
                            break;
                        case QuType.CHECKBOX:
                            var checklist = _dbQuery.QueryList<QuestionCheckbox>("SELECT * FROM dbo.QuestionCheckbox WHERE QuId in @quid AND IsValid=1 ORDER BY OrderById", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            checklist.ForEach(p => p.OptionName = Utils.HtmlDecode(p.OptionName));
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.HtmlDecode(question.QuName);
                                question.QuCheckboxes.AddRange(checklist.Where(p => p.QuId == question.Id));
                            }
                            break;
                        case QuType.SCORE:
                            var scorelist = _dbQuery.QueryList<QuestionScore>("SELECT * FROM dbo.QuestionScore WHERE QuId in @quid AND IsValid=1 ORDER BY OrderById", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            scorelist.ForEach(p => p.OptionName = Utils.HtmlDecode(p.OptionName));
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.HtmlDecode(question.QuName);
                                question.QuScores.AddRange(scorelist.Where(p => p.QuId == question.Id));
                            }
                            break;
                        case QuType.ORDERQU:
                            var orderbylist = _dbQuery.QueryList<QuestionOrderby>("SELECT * FROM dbo.QuestionOrderby WHERE QuId in @quid AND IsValid=1 ORDER BY OrderById", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            orderbylist.ForEach(p => p.OptionName = Utils.HtmlDecode(p.OptionName));
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.HtmlDecode(question.QuName);
                                question.QuOrderbies.AddRange(orderbylist.Where(p => p.QuId == question.Id));
                            }
                            break;
                        case QuType.MULTIFILLBLANK:
                            var mulitFillBlanklist = _dbQuery.QueryList<QuestionMultiFillblank>("SELECT * FROM dbo.QuestionMultiFillblank WHERE QuId in @quid AND IsValid=1 ORDER BY OrderById", new
                            {
                                quid = groupItem.Select(p => p.Id)
                            }).ToList();
                            mulitFillBlanklist.ForEach(p => p.OptionName = Utils.HtmlDecode(p.OptionName));
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.HtmlDecode(question.QuName);
                                question.QuMultiFillblanks.AddRange(mulitFillBlanklist.Where(p => p.QuId == question.Id));
                            }
                            break;
                        case QuType.CHENRADIO:
                        case QuType.CHENCHECKBOX:
                        case QuType.CHENSCORE:
                        case QuType.CHENFBK:
                            if (hasQueryChenGroup)
                            {
                                continue;
                            }
                            var multiItems = questions.Where(q => q.QuType == QuType.CHENRADIO || q.QuType == QuType.CHENCHECKBOX || q.QuType == QuType.CHENSCORE || q.QuType == QuType.CHENFBK).ToList();
                            var rowlist = _dbQuery.QueryList<QuestionChenRow>("SELECT * FROM dbo.QuestionChenRow WHERE QuId in @quid AND IsValid=1 ORDER BY OrderById", new
                            {
                                quid = multiItems.Select(p => p.Id)
                            }).ToList();
                            rowlist.ForEach(p => p.OptionName = Utils.HtmlDecode(p.OptionName));
                            var columnlist = _dbQuery.QueryList<QuestionChenColumn>("SELECT * FROM dbo.QuestionChenColumn WHERE QuId in @quid AND IsValid=1 ORDER BY OrderById", new
                            {
                                quid = multiItems.Select(p => p.Id)
                            }).ToList();
                            columnlist.ForEach(p => p.OptionName = Utils.HtmlDecode(p.OptionName));

                            foreach (Question question in multiItems)
                            {
                                question.QuName = Utils.HtmlDecode(question.QuName);
                                question.QuChenRows.AddRange(rowlist.Where(p => p.QuId == question.Id));
                                question.QuChenColumns.AddRange(columnlist.Where(p => p.QuId == question.Id));
                            }
                            hasQueryChenGroup = true;
                            break;
                        case QuType.PAGETAG:
                        case QuType.PARAGRAPH:
                        case QuType.FILLBLANK:
                            foreach (Question question in groupItem)
                            {
                                question.QuName = Utils.HtmlDecode(question.QuName);
                            }
                            break;
                    }
                }
            }
            return questions;
        }

        #region Save
        public SurveyDesignResult Save(Question question)
        {
            var rst = new SurveyDesignResult
            {
                orderById = question.OrderById
            };
            if (question.QuType == QuType.PAGETAG)
            {
                question.QuName = "分页";
            }
            else if (question.QuType == QuType.PARAGRAPH)
            {
                question.QuName = "分段";
            }
            question.QuName = Utils.HtmlEncode(Utils.UrlDecode(question.QuName));
            if (question.Id > 0)
            {
                rst.id = question.Id;
                var existModel = _repository.Get(question.Id);
                if (existModel != null && (existModel.Uid != question.Uid || !existModel.IsValid))
                {
                    return null;
                }
                _repository.Update(question);
            }
            else
            {
                rst.id = _repository.Add(question);
                UpdateQuestionNum(question.DirId);
            }

            if ((question.QuType == QuType.RADIO ) && question.QuRadios != null && question.QuRadios.Count > 0)
            {
                rst.quItems = SaveRadio(question.QuRadios, (int)rst.id, question.Uid);
            }
            else if ((question.QuType == QuType.CHECKBOX) && question.QuCheckboxes != null && question.QuCheckboxes.Count > 0)
            {
                rst.quItems = SaveCheckbox(question.QuCheckboxes, (int)rst.id, question.Uid);
            }
            else if (question.QuType == QuType.SCORE && question.QuScores != null && question.QuScores.Count > 0)
            {
                rst.quItems = SaveQuScore(question.QuScores, (int)rst.id, question.Uid);
            }
            else if (question.QuType == QuType.ORDERQU && question.QuOrderbies != null && question.QuOrderbies.Count > 0)
            {
                rst.quItems = SaveQuOrderby(question.QuOrderbies, (int)rst.id, question.Uid);
            }
            else if (question.QuType == QuType.MULTIFILLBLANK && question.QuMultiFillblanks != null && question.QuMultiFillblanks.Count > 0)
            {
                rst.quItems = SaveMultiFillblank(question.QuMultiFillblanks, (int)rst.id, question.Uid);
            }
            else if (question.QuType == QuType.CHENRADIO || question.QuType == QuType.CHENCHECKBOX || question.QuType == QuType.CHENSCORE || question.QuType == QuType.CHENFBK)
            {
                //矩阵题型
                var chenRst = SaveChens(question.QuChenColumns, question.QuChenRows, (int)rst.id, question.Uid);
                rst.quColumnItems = chenRst.Item1;
                rst.quRowItems = chenRst.Item2;
            }

            //更新排序号--如果是新增
            if (question.QuestionLogics != null)
            {
                rst.quLogics = SaveLogic(question.QuestionLogics, (int)rst.id, question.Uid);
            }

            return rst;
        }

        /// <summary>
        /// 单选、复合单选
        /// </summary>
        /// <param name="list"></param>
        /// <param name="surveyid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<QuItem> SaveRadio(List<QuestionRadio> list, int surveyid, long uid)
        {
            List<QuItem> rst = new List<QuItem>();
            if (list != null)
            {
                foreach (QuestionRadio quRadio in list)
                {
                    if (quRadio != null)
                    {
                        QuItem item = new QuItem()
                        {
                            title = quRadio.OrderById.ToString(),
                            id = quRadio.Id
                        };
                        quRadio.QuId = surveyid;
                        quRadio.Uid = uid;
                        quRadio.OptionName = Utils.HtmlEncode(Utils.UrlDecode(quRadio.OptionName));
                        if (quRadio.Id > 0)
                        {
                            var model = _quradioRepository.Get(quRadio.Id);
                            if (model != null && model.Id > 0)
                            {
                                _quradioRepository.Update(quRadio);
                            }
                            else
                            {
                                item.id = _quradioRepository.Add(quRadio);
                            }
                        }
                        else
                        {
                            item.id = _quradioRepository.Add(quRadio);
                        }
                        rst.Add(item);
                    }
                }
            }
            return rst;
        }

        public List<QuItem> SaveCheckbox(List<QuestionCheckbox> list, int surveyid, long uid)
        {
            List<QuItem> rst = new List<QuItem>();
            if (list != null)
            {
                foreach (QuestionCheckbox checkbox in list)
                {
                    if (checkbox != null)
                    {
                        QuItem item = new QuItem()
                        {
                            title = checkbox.OrderById.ToString(),
                            id = checkbox.Id
                        };
                        checkbox.QuId = surveyid;
                        checkbox.Uid = uid;
                        checkbox.OptionName = Utils.HtmlEncode(Utils.UrlDecode(checkbox.OptionName));
                        if (checkbox.Id > 0)
                        {
                            var model = _quCheckboxRepository.Get(checkbox.Id);
                            if (model != null && model.Id > 0)
                            {
                                _quCheckboxRepository.Update(checkbox);
                            }
                            else
                            {
                                item.id = _quCheckboxRepository.Add(checkbox);
                            }
                        }
                        else
                        {
                            item.id = _quCheckboxRepository.Add(checkbox);
                        }
                        rst.Add(item);
                    }
                }
            }
            return rst;
        }

        public List<QuItem> SaveQuScore(List<QuestionScore> list, int surveyid, long uid)
        {
            List<QuItem> rst = new List<QuItem>();
            if (list != null)
            {
                foreach (QuestionScore score in list)
                {
                    if (score != null)
                    {
                        QuItem item = new QuItem()
                        {
                            title = score.OrderById.ToString(),
                            id = score.Id
                        };
                        score.QuId = surveyid;
                        score.Uid = uid;
                        score.OptionName = Utils.HtmlEncode(Utils.UrlDecode(score.OptionName));
                        if (score.Id > 0)
                        {
                            var model = _quScoreRepository.Get(score.Id);
                            if (model != null && model.Id > 0)
                            {
                                _quScoreRepository.Update(score);
                            }
                            else
                            {
                                item.id = _quScoreRepository.Add(score);
                            }
                        }
                        else
                        {
                            item.id = _quScoreRepository.Add(score);
                        }
                        rst.Add(item);
                    }
                }
            }
            return rst;
        }

        public List<QuItem> SaveQuOrderby(List<QuestionOrderby> list, int surveyid, long uid)
        {
            List<QuItem> rst = new List<QuItem>();
            if (list != null)
            {
                foreach (QuestionOrderby orderby in list)
                {
                    if (orderby != null)
                    {
                        QuItem item = new QuItem()
                        {
                            title = orderby.OrderById.ToString(),
                            id = orderby.Id
                        };
                        orderby.QuId = surveyid;
                        orderby.Uid = uid;
                        orderby.OptionName = Utils.HtmlEncode(Utils.UrlDecode(orderby.OptionName));
                        if (orderby.Id > 0)
                        {
                            var model = _quOrderbyRepository.Get(orderby.Id);
                            if (model != null && model.Id > 0)
                            {
                                _quOrderbyRepository.Update(orderby);
                            }
                            else
                            {
                                item.id = _quOrderbyRepository.Add(orderby);
                            }
                        }
                        else
                        {
                            item.id = _quOrderbyRepository.Add(orderby);
                        }
                        rst.Add(item);
                    }
                }
            }
            return rst;
        }

        public List<QuItem> SaveMultiFillblank(List<QuestionMultiFillblank> list, int surveyid, long uid)
        {
            List<QuItem> rst = new List<QuItem>();
            if (list != null)
            {
                foreach (QuestionMultiFillblank muliFillblank in list)
                {
                    if (muliFillblank != null)
                    {
                        QuItem item = new QuItem()
                        {
                            title = muliFillblank.OrderById.ToString(),
                            id = muliFillblank.Id
                        };
                        muliFillblank.QuId = surveyid;
                        muliFillblank.Uid = uid;
                        muliFillblank.OptionName = Utils.HtmlEncode(Utils.UrlDecode(muliFillblank.OptionName));
                        if (muliFillblank.Id > 0)
                        {
                            var model = _quMultiFillblankRepository.Get(muliFillblank.Id);
                            if (model != null && model.Id > 0)
                            {
                                _quMultiFillblankRepository.Update(muliFillblank);
                            }
                            else
                            {
                                item.id = _quMultiFillblankRepository.Add(muliFillblank);
                            }
                        }
                        else
                        {
                            item.id = _quMultiFillblankRepository.Add(muliFillblank);
                        }
                        rst.Add(item);
                    }
                }
            }
            return rst;
        }

        public Tuple<List<QuItem>, List<QuItem>> SaveChens(List<QuestionChenColumn> columns, List<QuestionChenRow> rows, int surveyid, long uid)
        {
            List<QuItem> columnrst = new List<QuItem>();
            if (columns != null)
            {
                foreach (QuestionChenColumn column in columns)
                {
                    if (column != null)
                    {
                        QuItem item = new QuItem()
                        {
                            title = column.OrderById.ToString(),
                            id = column.Id
                        };
                        column.QuId = surveyid;
                        column.Uid = uid;
                        column.OptionName = Utils.HtmlEncode(Utils.UrlDecode(column.OptionName));
                        if (column.Id > 0)
                        {
                            var model = _quChenColumnRepository.Get(column.Id);
                            if (model != null && model.Id > 0)
                            {
                                _quChenColumnRepository.Update(column);
                            }
                            else
                            {
                                item.id = _quChenColumnRepository.Add(column);
                            }
                        }
                        else
                        {
                            item.id = _quChenColumnRepository.Add(column);
                        }
                        columnrst.Add(item);
                    }
                }
            }
            List<QuItem> rowrst = new List<QuItem>();
            if (rows != null)
            {
                foreach (QuestionChenRow chenRow in rows)
                {
                    if (chenRow != null)
                    {
                        QuItem item = new QuItem()
                        {
                            title = chenRow.OrderById.ToString(),
                            id = chenRow.Id
                        };
                        chenRow.QuId = surveyid;
                        chenRow.Uid = uid;
                        chenRow.OptionName = Utils.HtmlEncode(Utils.UrlDecode(chenRow.OptionName));
                        if (chenRow.Id > 0)
                        {
                            var model = _quChenRowRepository.Get(chenRow.Id);
                            if (model != null && model.Id > 0)
                            {
                                _quChenRowRepository.Update(chenRow);
                            }
                            else
                            {
                                item.id = _quChenRowRepository.Add(chenRow);
                            }
                        }
                        else
                        {
                            item.id = _quChenRowRepository.Add(chenRow);
                        }
                        rowrst.Add(item);
                    }
                }
            }
            return Tuple.Create(columnrst, rowrst);
        }

        public int UpdateQuestionNum(long dirid)
        {
            return _dbQuery.ExecuteSql("UPDATE dbo.SurveyDirectory SET SurveyQuNum =SurveyQuNum + 1 WHERE IsValid=1 AND id=@dirid;",
                new { dirid = dirid});
        }

        public List<QuItem> SaveLogic(List<QuestionLogic> questionLogics, int surveyid, long uid)
        {
            List<QuItem> logicItems = new List<QuItem>();
            foreach (QuestionLogic logic in questionLogics)
            {
                if (logic != null)
                {
                    QuItem item = new QuItem()
                    {
                        title = logic.Title
                    };
                    logic.QuId = surveyid;
                    logic.Uid = uid;
                    if (logic.Id > 0)
                    {
                        var existModel = _questionLogicRepository.Get(logic.Id);
                        if (existModel != null && existModel.Uid == uid)
                        {
                            item.id = logic.Id;
                            _questionLogicRepository.Update(logic);
                        }
                        else
                        {
                            item.id = _questionLogicRepository.Add(logic);
                        }
                    }
                    else
                    {
                        item.id = _questionLogicRepository.Add(logic);
                    }

                    logicItems.Add(item);
                }
            }
            return logicItems;
        }
        #endregion


        #region Delete
        public int DeleteQuestion(long uid, int id)
        {
            return _dbQuery.ExecuteSql("UPDATE dbo.Question SET IsValid = 0 WHERE id = @id AND Uid = @uid and IsValid =1;UPDATE dbo.SurveyDirectory SET SurveyQuNum =SurveyQuNum -1 WHERE IsValid=1 AND id IN (SELECT TOP 1 DirId FROM dbo.Question WHERE id=@id)", new { id = id, uid = uid });
        }
        public int DeleteQuestionRadio(long uid, int id)
        {
            return _dbQuery.ExecuteSql("UPDATE dbo.QuestionRadio SET IsValid = 0 WHERE id = @id AND Uid = @uid and IsValid =1;UPDATE dbo.SurveyDirectory SET SurveyQuNum =SurveyQuNum -1 WHERE IsValid=1 AND id IN (SELECT TOP 1 DirId FROM dbo.Question WHERE id=@id)", new { id = id, uid = uid });
        }
        public int DeleteQuestionCheckbox(long uid, int id)
        {
            return _dbQuery.ExecuteSql("UPDATE dbo.QuestionCheckbox SET IsValid = 0 WHERE id = @id AND Uid = @uid and IsValid =1;UPDATE dbo.SurveyDirectory SET SurveyQuNum =SurveyQuNum -1 WHERE IsValid=1 AND id IN (SELECT TOP 1 DirId FROM dbo.Question WHERE id=@id)", new { id = id, uid = uid });
        }
        public int DeleteQuestionScore(long uid, int id)
        {
            return _dbQuery.ExecuteSql("UPDATE dbo.QuestionScore SET IsValid = 0 WHERE id = @id AND Uid = @uid and IsValid =1;UPDATE dbo.SurveyDirectory SET SurveyQuNum =SurveyQuNum -1 WHERE IsValid=1 AND id IN (SELECT TOP 1 DirId FROM dbo.Question WHERE id=@id)", new { id = id, uid = uid });
        }
        public int DeleteQuestionOrderby(long uid, int id)
        {
            return _dbQuery.ExecuteSql("UPDATE dbo.QuestionOrderby SET IsValid = 0 WHERE id = @id AND Uid = @uid and IsValid =1;UPDATE dbo.SurveyDirectory SET SurveyQuNum =SurveyQuNum -1 WHERE IsValid=1 AND id IN (SELECT TOP 1 DirId FROM dbo.Question WHERE id=@id)", new { id = id, uid = uid });
        }
        public int DeleteQuestionMultiFillblank(long uid, int id)
        {
            return _dbQuery.ExecuteSql("UPDATE dbo.QuestionMultiFillblank SET IsValid = 0 WHERE id = @id AND Uid = @uid and IsValid =1;UPDATE dbo.SurveyDirectory SET SurveyQuNum =SurveyQuNum -1 WHERE IsValid=1 AND id IN (SELECT TOP 1 DirId FROM dbo.Question WHERE id=@id)", new { id = id, uid = uid });
        }
        public int DeleteQuestionChenColumn(long uid, int id)
        {
            return _dbQuery.ExecuteSql("UPDATE dbo.QuestionChenColumn SET IsValid = 0 WHERE id = @id AND Uid = @uid and IsValid =1;UPDATE dbo.SurveyDirectory SET SurveyQuNum =SurveyQuNum -1 WHERE IsValid=1 AND id IN (SELECT TOP 1 DirId FROM dbo.Question WHERE id=@id)", new { id = id, uid = uid });
        }
        public int DeleteQuestionChenRow(long uid, int id)
        {
            return _dbQuery.ExecuteSql("UPDATE dbo.QuestionChenRow SET IsValid = 0 WHERE id = @id AND Uid = @uid and IsValid =1;UPDATE dbo.SurveyDirectory SET SurveyQuNum =SurveyQuNum -1 WHERE IsValid=1 AND id IN (SELECT TOP 1 DirId FROM dbo.Question WHERE id=@id)", new { id = id, uid = uid });
        }

        #endregion


    }
}
