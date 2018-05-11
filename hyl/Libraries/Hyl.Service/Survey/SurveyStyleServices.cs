using System.Collections.Generic;
using Hyl.Core.Domain.Survey;
using Hyl.Repository;

namespace Hyl.Service.Survey
{
    public class SurveyStyleServices : ISurveyStyleServices
    {
        private readonly IRepository<SurveyStyle> _repository;
        public SurveyStyleServices(IRepository<SurveyStyle> repository)
        {
            _repository = repository;
        }


        public long Insert(SurveyStyle sitemanager)
        {
            return _repository.Add(sitemanager);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public SurveyStyle Get(int id)
        {
            return _repository.Get(id);
        }

        public List<SurveyStyle> GetList()
        {
            return _repository.GetAllList();
        }

        public bool Update(SurveyStyle sitemanager)
        {
            return _repository.Update(sitemanager);
        }
    }
}
