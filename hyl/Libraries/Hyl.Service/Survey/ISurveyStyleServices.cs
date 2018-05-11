using Hyl.Core;
using Hyl.Core.Domain.Survey;
using System.Collections.Generic;

namespace Hyl.Service.Survey
{
    public interface ISurveyStyleServices: IServicesDependency
    {
        long Insert(SurveyStyle sitemanager);
        bool Update(SurveyStyle sitemanager);
        bool Delete(int id);
        SurveyStyle Get(int id);
        List<SurveyStyle> GetList();
    }
}
