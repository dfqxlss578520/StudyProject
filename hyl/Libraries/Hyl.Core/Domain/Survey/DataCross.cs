using System;
using System.Collections.Generic;

namespace Hyl.Core.Domain.Survey
{

    /// <summary>
    /// 答案 是非题结果保存表
    /// </summary>
    public class DataCross : BaseEntity
    {
		
        public string optionName;

        private int count;

        //	private int colCount;

        //	private int rowCount;

        private List<DataCross> colDataCrosss = new List<DataCross>();

        public int getCount()
        {
            return count;
        }

        public void setCount(int count)
        {
            this.count = count;
        }

        public String getOptionName()
        {
            return optionName;
        }

        public void setOptionName(String optionName)
        {
            this.optionName = optionName;
        }

        public List<DataCross> getColDataCrosss()
        {
            return colDataCrosss;
        }

        public void setColDataCrosss(List<DataCross> colDataCrosss)
        {
            this.colDataCrosss = colDataCrosss;
        }


    }
}
