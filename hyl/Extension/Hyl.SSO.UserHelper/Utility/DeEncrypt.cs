using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class DeEncrypt
    {
        static string CONST_ENCRYKEY = "WangyqLijSuks_GainALotOfMoney_AndBuyCarAndBuildHouse";

        public DataTable Init()
        {
            var Is64Machine = ConfigurationManager.AppSettings["Is64Machine"].ToLower() == "true";
            DataTable dt = SqlHelper.GetTable("SELECT * FROM dbo.Ts_User", CommandType.Text, null);
            DataTable newTable = new DataTable();
            newTable.Columns.AddRange(new[]
            {
                new DataColumn("username"),
                new DataColumn("password"),
            });
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    DataRow newRow = newTable.NewRow();
                    newRow["username"] = dtRow["LoginName"];
                    if (Is64Machine)
                    {
                        if (dtRow["password"].ToString() == "sa")
                        {
                            newRow["password"] = EncryTool.DecryptDES(dtRow["password"].ToString(), CONST_ENCRYKEY).Trim();
                        }
                        else
                        {
                            newRow["password"] = EncryTool.DeCryptData64(dtRow["password"].ToString(), CONST_ENCRYKEY).Trim();
                        }
                    }
                    else
                    {
                        newRow["password"] = EncryTool.DecryptDES(dtRow["password"].ToString(), CONST_ENCRYKEY).Trim();                        
                    }
                    newTable.Rows.Add(newRow);
                }
            }
            return newTable;

        }
    }
}
