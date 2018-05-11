using System.Collections.Generic;

namespace Hyl.Core.Domain.Survey
{
    public class SurveyDesignResult
    {
        public long id { get; set; }

        public int orderById { get; set; }

        public List<QuItem> quItems { get; set; }= new List<QuItem>();

        public List<QuItem> quLogics { get; set; } =new List<QuItem>();

        public List<QuItem> quColumnItems { get; set; } =new List<QuItem>();
        public List<QuItem> quRowItems { get; set; } =new List<QuItem>();
    }

    public class QuItem
    {
        public long id { get; set; }
        public string title { get; set; }
    }
}
