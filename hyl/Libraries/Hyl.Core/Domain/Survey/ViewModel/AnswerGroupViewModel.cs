using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey.ViewModel
{
    public class AnswerGroupViewModel
    {
        /// <summary>
        /// 默认为选项RowId或Itemid
        /// </summary>
        public int GroupKey { get; set; }
        /// <summary>
        /// 列ID
        /// </summary>
        public int GroupColKey { get; set; }
        /// <summary>
        /// count计算结果
        /// </summary>
        public int GroupCount { get; set; }
        /// <summary>
        /// Avg计算结果
        /// </summary>
        public float AvgNum { get; set; }
        /// <summary>
        /// Sum计算结果
        /// </summary>
        public int SumNum { get; set; }

        /// <summary>
        /// 统计列表拓展字段
        /// </summary>
        [Write(false)]
        public int EmptyCount { get; set; }
        [Write(false)]
        public int NoEmptyCount { get; set; }
    }
}
