using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{


    [Table("t_surveyReqUrl")]
    public class SurveyReqUrl : BaseEntity
    {
        //短ID
        public string sid;
        public string surveyId;
        public int state = 0;//0 默认主链接,1 特定用户的链接

    }

}
