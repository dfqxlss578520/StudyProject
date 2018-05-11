using System.Linq;
using Hyl.Core.Domain.Survey;
using Hyl.Core.Domain.Survey.ViewModel;
using Hyl.Repository;
using Hyl.Core.Caching;
using Hyl.Core.Configuration.Survey;
using Hyl.Core.Helpers.Utility;
using System;

namespace Hyl.Service.Survey
{
    public class SurveyDirectoryServices : ISurveyDirectoryServices
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<SurveyDirectory> _repository;
        private readonly IDbQuery _dbQuery;
        private readonly ISurveyDetailServices _surveyDetailServices;
        private readonly IQuestionServices _questionservices;
        private readonly ISurveyStyleServices _surveyStyleServices;

        public SurveyDirectoryServices(IRepository<SurveyDirectory> repository,
            IDbQuery dbQuery, ISurveyDetailServices surveyDetailServices,
            ISurveyStyleServices surveyStyleServices,
            ICacheManager cacheManager,
            IQuestionServices questionservices)
        {
            _repository = repository;
            _dbQuery = dbQuery;
            _surveyDetailServices = surveyDetailServices;
            _questionservices = questionservices;
            _surveyStyleServices = surveyStyleServices;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 问卷列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SurveyDirectoryViewModel GetPaged(SurveyDirectoryViewModel model)
        {
            model.Conditions = " where (@surveystate ='-1' OR SurveyState=@surveystate) AND (@surveyname='' OR SurveyName =@surveyname) and Uid=@uid and IsValid =1";
            model.Parameters = new { surveystate = model.QuerySurveyState, surveyname = Utils.HtmlEncode(Utils.UrlDecode(model.QuerySurveyName)), uid = model.Uid };
            model.Items = _repository.GetPaged(model.Current).Items;
            return model;
        }


        #region 基础方法
        /// <summary>
        ///  新建问卷
        /// </summary>
        /// <param name="surveyDirectory"></param>
        /// <returns></returns>
        public long Insert(SurveyDirectory surveyDirectory)
        {
            var dirid = _repository.Add(surveyDirectory);
            if (dirid > 0)
            {
                var detailId = _surveyDetailServices.Add(new SurveyDetail()
                {
                    DirId = (int)dirid,
                    SurveyNote = surveyDirectory.DirType == 2 ? "非常感谢您的参与！如有涉及个人信息，我们将严格保密。" : string.Empty,
                    EndTime = DateTime.Now.AddMonths(1)
                });
                _dbQuery.ExecuteSql("UPDATE dbo.SurveyDirectory SET SurveyDetailId = @surveydetailid WHERE id=@id", new { surveydetailid = detailId, id = dirid });
            }
            return dirid;
        }

        /// <summary>
        /// 删除问卷
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var rst = _repository.Delete(id);
            if (rst)
            {
                if (_cacheManager.IsSet(string.Format(SurveyConfig.DIR, id)))
                {
                    _cacheManager.Remove(string.Format(SurveyConfig.DIR, id));
                }
                if (_cacheManager.IsSet(string.Format(SurveyConfig.DIRPOLICY, id)))
                {
                    _cacheManager.Remove(string.Format(SurveyConfig.DIRPOLICY, id));
                }
                if (_cacheManager.IsSet(string.Format(SurveyConfig.DIRDETAILSTYLE, id)))
                {
                    _cacheManager.Remove(string.Format(SurveyConfig.DIRDETAILSTYLE, id));
                }
            }
            return rst;
        }

        /// <summary>
        /// 复制问卷
        /// </summary>
        /// <param name="surveyid"></param>
        /// <returns></returns>
        public int Copy(int surveyid)
        {
            return _dbQuery.ExecuteProcSql("CopySurvey", new { surveyid = surveyid });
        }
        #endregion


        public SurveyDirectory GetDirectoryWithQuestion(int id)
        {
            var cacheModel = _cacheManager.Get<SurveyDirectory>(string.Format(SurveyConfig.DIR, id));
            if (cacheModel == null)
            {
                var surveyDirectory = _dbQuery.QueryList<SurveyDirectory>("SELECT * FROM SurveyDirectory WHERE id=@id and IsValid =1;", new { Id = id }).FirstOrDefault();
                if (surveyDirectory != null)
                {
                    surveyDirectory.Questions = _questionservices.Get(surveyDirectory.Id);
                }
                if (surveyDirectory.SurveyState > 0)
                {
                    _cacheManager.Set(string.Format(SurveyConfig.DIR, id), surveyDirectory, 60 * 24);
                }
                return surveyDirectory;
            }
            return cacheModel;
        }
        public SurveyDirectory GetDirectoryWithQuestion(int id, long userid)
        {
            var cacheModel = _cacheManager.Get<SurveyDirectory>(string.Format(SurveyConfig.DIRPOLICY, id));
            if (cacheModel == null)
            {
                var surveyDirectory = _dbQuery.QueryList<SurveyDirectory>("SELECT * FROM SurveyDirectory WHERE Uid = @uid AND id=@id and IsValid =1", new { Id = id, uid = userid }).FirstOrDefault();
                if (surveyDirectory != null)
                {
                    surveyDirectory.Questions = _questionservices.Get(surveyDirectory.Id);
                }
                if (surveyDirectory.SurveyState > 0)
                {
                    _cacheManager.Set(string.Format(SurveyConfig.DIRPOLICY, id), surveyDirectory, 60 * 24);
                }
                return surveyDirectory;
            }
            return cacheModel;
        }


        public SurveyDirectory GetDirectoryDetailStyle(int id)
        {
            var cacheModel = _cacheManager.Get<SurveyDirectory>(string.Format(SurveyConfig.DIRDETAILSTYLE, id));
            if (cacheModel == null)
            {
                var surveyDirectory = _dbQuery.Query<SurveyDirectory, SurveyDetail, SurveyStyle, SurveyDirectory>("SELECT a.*,b.*,c.* FROM dbo.SurveyDirectory a INNER JOIN dbo.SurveyDetail b ON a.id = b.DirId LEFT JOIN dbo.SurveyStyle c ON a.id =c.DirId AND c.IsValid=1 WHERE a.id = @Id AND a.IsValid = 1",
                    (dir, detail, style) =>
                    {
                        dir.SurveyDetail = detail;
                        dir.SurveyStyle = style ?? new SurveyStyle();
                        return dir;
                    }, new { Id = id }, splitOn: "id").FirstOrDefault();
                if (surveyDirectory != null)
                {
                    surveyDirectory.Questions = _questionservices.Get(surveyDirectory.Id);
                }
                if (surveyDirectory.SurveyState > 0)
                {
                    _cacheManager.Set(string.Format(SurveyConfig.DIRDETAILSTYLE, id), surveyDirectory, 60 * 24);
                }
                return surveyDirectory;
            }
            return cacheModel;
        }
        public SurveyDirectory GetDirectoryDetailStyle(int id, long userid)
        {
            var surveyDirectory = _dbQuery.Query<SurveyDirectory, SurveyDetail, SurveyStyle, SurveyDirectory>("SELECT a.*,b.*,c.* FROM dbo.SurveyDirectory a INNER JOIN dbo.SurveyDetail b ON a.id = b.DirId LEFT JOIN dbo.SurveyStyle c ON a.id =c.DirId AND c.IsValid=1 WHERE a.id = @Id AND a.Uid = @UserId and a.IsValid = 1",
                (dir, detail, style) =>
                {
                    dir.SurveyDetail = detail;
                    dir.SurveyStyle = style ?? new SurveyStyle();
                    return dir;
                }, new { Id = id, UserId = userid }, splitOn: "id").FirstOrDefault();
            if (surveyDirectory != null)
            {
                surveyDirectory.Questions = _questionservices.Get(surveyDirectory.Id);
            }
            return surveyDirectory;
        }


        public bool UpdateSurveyNameAndDetail(SurveyDirectory model)
        {
            if (model != null)
            {
                _dbQuery.ExecuteSql("UPDATE dbo.SurveyDirectory SET SurveyName=@surveyname WHERE id =@id and IsValid =1", new { id = model.Id, surveyname = Utils.HtmlEncode(Utils.UrlDecode(model.SurveyName)) });
                model.SurveyDetail.DirId = model.Id;
                model.SurveyDetail.Id = model.SurveyDetailId;
                model.SurveyDetail.SurveyNote = Utils.HtmlEncode(Utils.UrlDecode(model.SurveyDetail.SurveyNote));
                var rst = _surveyDetailServices.Update(model.SurveyDetail);
                if (rst)
                {
                    if (_cacheManager.IsSet(string.Format(SurveyConfig.DIR, model.Id)))
                    {
                        _cacheManager.Remove(string.Format(SurveyConfig.DIR, model.Id));
                    }
                    if (_cacheManager.IsSet(string.Format(SurveyConfig.DIRPOLICY, model.Id)))
                    {
                        _cacheManager.Remove(string.Format(SurveyConfig.DIRPOLICY, model.Id));
                    }
                    if (_cacheManager.IsSet(string.Format(SurveyConfig.DIRDETAILSTYLE, model.Id)))
                    {
                        _cacheManager.Remove(string.Format(SurveyConfig.DIRDETAILSTYLE, model.Id));
                    }
                }
                return rst;
            }
            return false;

        }

        public bool Publish(int id, long userid)
        {
            return SaveSurveyState(id, userid, 1);
        }
        public bool SaveSurveyState(int id, long userid, int surveystate)
        {
            var surveyDirectory = _dbQuery.QueryList<SurveyDirectory>("SELECT * FROM SurveyDirectory WHERE Uid = @uid AND id=@id and IsValid =1", new { Id = id, uid = userid }).FirstOrDefault();
            if (surveyDirectory != null)
            {

                var rst = _dbQuery.ExecuteSql("UPDATE dbo.SurveyDirectory SET SurveyState = @surveystate WHERE id = @id;", new { Id = id, surveystate = surveystate }) > 0;
                if (rst)
                {
                    if (_cacheManager.IsSet(string.Format(SurveyConfig.DIR, id)))
                    {
                        _cacheManager.Remove(string.Format(SurveyConfig.DIR, id));
                    }
                    if (_cacheManager.IsSet(string.Format(SurveyConfig.DIRPOLICY, id)))
                    {
                        _cacheManager.Remove(string.Format(SurveyConfig.DIRPOLICY, id));
                    }
                    if (_cacheManager.IsSet(string.Format(SurveyConfig.DIRDETAILSTYLE, id)))
                    {
                        _cacheManager.Remove(string.Format(SurveyConfig.DIRDETAILSTYLE, id));
                    }
                }
                return rst;
            }
            return false;
        }


        public bool UpdateSurveyStyle(SurveyDirectory model)
        {
            if (model != null)
            {
                bool rst = true;
                if (model.SurveyDetail != null)
                {
                    model.SurveyDetail.SurveyNote = Utils.HtmlEncode(Utils.UrlDecode(model.SurveyDetail.SurveyNote));
                    rst = _surveyDetailServices.Update(model.SurveyDetail);
                }
                if (model.SurveyStyle != null)
                {
                    var existStyle = _dbQuery.QuerySingle<SurveyStyle>("SELECT * FROM dbo.SurveyStyle WHERE DirId = @dirid and IsValid =1", new { dirid = model.Id });
                    var sourceStyle = _dbQuery.QuerySingle<SurveyStyle>("SELECT * FROM dbo.SurveyStyle WHERE id = @id and IsValid =1", new { id = model.SurveyStyle.Id });
                    if (existStyle != null)
                    {
                        sourceStyle.Id = existStyle.Id;
                        sourceStyle.DirId = model.Id;
                        var updateRst = _surveyStyleServices.Update(sourceStyle);
                        rst = rst || updateRst;
                    }
                    else
                    {
                        sourceStyle.DirId = model.Id;
                        var istrst = _surveyStyleServices.Insert(sourceStyle);
                        rst = rst || istrst > 0;
                    }
                }
                if (rst)
                {
                    if (_cacheManager.IsSet(string.Format(SurveyConfig.DIRDETAILSTYLE, model.Id)))
                    {
                        _cacheManager.Remove(string.Format(SurveyConfig.DIRDETAILSTYLE, model.Id));
                    }
                }
                return rst;
            }
            return false;
        }

    }
}
