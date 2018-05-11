using Hyl.Core.Domain.PageDomain;

namespace Hyl.Core.Domain.Survey.ViewModel
{
    public class SurveyDirectoryViewModel : Page<SurveyDirectory>
    {
        public string QuerySurveyState { get; set; } = "-1";

        public string QuerySurveyName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public long Uid { get; set; }
    }
}
