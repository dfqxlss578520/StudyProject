using Hyl.Core;
using Hyl.Core.Domain.Survey;
using Hyl.Core.Domain.Survey.ViewModel;

namespace Hyl.Service.Survey
{
    public interface ISurveyDirectoryServices : IServicesDependency
    {
        long Insert(SurveyDirectory sitemanager);
        bool Delete(int id);
        SurveyDirectory GetDirectoryWithQuestion(int id);
        SurveyDirectory GetDirectoryWithQuestion(int id, long userid);
        SurveyDirectory GetDirectoryDetailStyle(int id);
        SurveyDirectory GetDirectoryDetailStyle(int id, long userid);

        SurveyDirectoryViewModel GetPaged(SurveyDirectoryViewModel model);

        bool UpdateSurveyNameAndDetail(SurveyDirectory model);
        bool UpdateSurveyStyle(SurveyDirectory model);

        bool Publish(int id, long userid);

        bool SaveSurveyState(int id, long userid, int surveystate);

        int Copy(int surveyid);

    }
}
