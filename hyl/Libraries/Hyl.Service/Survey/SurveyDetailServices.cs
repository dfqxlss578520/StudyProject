using Hyl.Core.Domain.Survey;
using Hyl.Repository;

namespace Hyl.Service.Survey
{
    public class SurveyDetailServices : ISurveyDetailServices
    {
        private readonly IRepository<SurveyDetail> _repository;
        private readonly IDbQuery _dbQuery;
        public SurveyDetailServices(IRepository<SurveyDetail> repository, IDbQuery dbQuery)
        {
            _repository = repository;
            _dbQuery = dbQuery;
        }

        public long Add(SurveyDetail model)
        {
            return _repository.Add(model);
        }

        public bool Update(SurveyDetail model)
        {
            return _repository.Update(model);
        }

        public SurveyDetail Get(int id)
        {
            return _repository.Get(id);
        }

        public SurveyDetail GetBySurveyId(int surveyid)
        {
            return _dbQuery.QuerySingle<SurveyDetail>("SELECT * FROM dbo.SurveyDetail WHERE DirId =@dirid", new { dirid = surveyid });
        }

    }
}
