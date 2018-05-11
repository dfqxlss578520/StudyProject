using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace Hyl.SSO.UserHelper
{
    public partial class Form1 : Form
    {
        private DataTable newTable = new DataTable();
        private int companyId = Convert.ToInt32(ConfigurationManager.AppSettings["CompanyId"]);
        private string FromDb = ConfigurationManager.AppSettings["SQLConnString"];//源数据
        private string ToDb = ConfigurationManager.AppSettings["ssoConn"]; //目标数据库
        public Form1()
        {
            InitializeComponent();
            if (companyId == 0 || string.IsNullOrEmpty(FromDb) || string.IsNullOrEmpty(ToDb))
            {
                btn_run.Visible = false;
            }
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            btn_run.Enabled = false;
            DeEncrypt deEncrypt = new DeEncrypt();
            newTable = deEncrypt.Init();
            dgv.DataSource = newTable;
            btn_save.Visible = true;
            btn_run.Enabled = true;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            btn_run.Enabled = false;
            btn_save.Enabled = false;
            if (newTable != null && newTable.Rows.Count > 0)
            {
                StringBuilder sql = new StringBuilder();
                foreach (DataRow row in newTable.Rows)
                {
                    Users user = new Users();
                    user.UserName = row["Username"].ToString();
                    user.Salt = Guid.NewGuid().ToString().Split('-')[0];
                    user.Password = EncryTool.StrongEncrypt(user.Salt, row["password"].ToString());
                    user.IsValid = true;
                    sql.AppendFormat("INSERT INTO dbo.Users (UserName, Password, Salt, IsValid,Company) VALUES ('{0}', '{1}', '{2}', 1, {3});", user.UserName, user.Password, user.Salt, companyId);
                    if (sql.Length > 5000)
                    {
                        SsoHelper.ExecuteCommand(sql.ToString(), CommandType.Text, null);
                        sql = new StringBuilder();
                    }
                }
                SsoHelper.ExecuteCommand(sql.ToString(), CommandType.Text, null);
                MessageBox.Show("保存完成");
            }
            else
            {
                MessageBox.Show("没有数据");
            }
            btn_run.Enabled = true;
            btn_save.Enabled = true;
        }

    }
}
