using System.Collections.Generic;
using Hyl.Core;
using Hyl.Core.Domain.Survey;
using Hyl.Core.Domain.PageDomain;

namespace Hyl.Service.Survey
{
    public interface ISurveyAnswerServices: IServicesDependency
    {

        Question GetChart(long questionid);

        long SaveAnswer(SurveyAnswer model);

        Page<SurveyAnswer> GetPageSurveyAnswer(Page<SurveyAnswer> page);

        List<Question> GetStatistics(SurveyDirectory survey);

        SurveyDirectory GetAnswers(int answerId);

        bool Delete(int answerId);
    }
}
