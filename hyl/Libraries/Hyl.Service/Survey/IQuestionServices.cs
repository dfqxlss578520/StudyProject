using System.Collections.Generic;
using Hyl.Core;
using Hyl.Core.Domain.Survey;

namespace Hyl.Service.Survey
{
    public interface IQuestionServices:IServicesDependency
    {
        SurveyDesignResult Save(Question question);
        int DeleteQuestionRadio(long uid, int id);
        int DeleteQuestion(long uid, int id);
        int DeleteQuestionCheckbox(long uid, int id);
        int DeleteQuestionScore(long uid, int id);
        int DeleteQuestionOrderby(long uid, int id);
        int DeleteQuestionMultiFillblank(long uid, int id);
        int DeleteQuestionChenColumn(long uid, int id);
        int DeleteQuestionChenRow(long uid, int id);

        Question Get(long questionid);
        List<Question> Get(int surveyId);
    }
}
