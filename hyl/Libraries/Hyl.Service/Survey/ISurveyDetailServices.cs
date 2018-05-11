using Hyl.Core;
using Hyl.Core.Domain.Survey;

namespace Hyl.Service.Survey
{
    public interface ISurveyDetailServices : IServicesDependency
    {
        long Add(SurveyDetail model);
        bool Update(SurveyDetail model);
        SurveyDetail Get(int id);
        SurveyDetail GetBySurveyId(int surveyid);
    }
}
