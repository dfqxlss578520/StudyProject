using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Collections;


namespace Utility
{
    public static class SqlHelper
    {

        /// <summary>
        /// �����ַ���
        /// </summary>
        private static readonly string conStr = ConfigurationManager.AppSettings["SQLConnString"];

        /// <summary>
        /// ��ѯ,���� SqlDataReader
        /// </summary>
        /// <param name="cmdText">SQL���</param>
        /// <param name="parms">��������</param>
        /// <returns></returns>
        /// 
        public static SqlDataReader GetDataReader(string cmdText, CommandType type, params SqlParameter[] parms)
        {
            SqlDataReader reader;
            SqlConnection cn = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand(cmdText, cn);
            cmd.CommandType = type;
            if (parms != null)
            {
                foreach (SqlParameter p in parms)
                {
                    cmd.Parameters.Add(p);
                }
            }
            if (cn.State == ConnectionState.Broken)
            {
                cn.Close();
            }
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
            reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cn.Close();
            return reader;
        }




        /// <summary>
        ///  ��ӡ�ɾ�����޸�,������Ӱ�������
        /// </summary>
        /// <param name="cmdText">SQL���</param>
        /// <param name="parms">��������</param>
        /// <returns>��Ӱ�������</returns>
        public static int ExecuteCommand(string cmdText, CommandType type, params SqlParameter[] parms)
        {
            int count = 0;
            using (SqlConnection cn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(cmdText, cn);
                cmd.CommandType = type;
                if (parms != null)
                {
                    foreach (SqlParameter p in parms)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                if (cn.State == ConnectionState.Broken)
                {
                    cn.Close();
                }
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                count = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
            }
            return count;
        }




        /// <summary>
        /// ���ص�һ�е�һ��
        /// </summary>
        /// <param name="cmdText">SQL���</param>
        /// <param name="parms">��������</param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdText, CommandType type, params SqlParameter[] parms)
        {
            object obj;
            using (SqlConnection cn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(cmdText, cn);
                cmd.CommandType = type;
                if (parms != null)
                {
                    foreach (SqlParameter p in parms)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                if (cn.State == ConnectionState.Broken)
                {
                    cn.Close();
                }
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }
            return obj;
        }

        /// <summary>
        /// ��ѯ,���� DataTable
        /// </summary>
        /// <param name="cmdText">SQL���</param>
        /// <param name="type"></param>
        /// <param name="parms">��������</param>
        /// <returns></returns>
        public static DataTable GetTable(string cmdText, CommandType type, params SqlParameter[] parms)
        {
            SqlConnection sqlCon = new SqlConnection(conStr);
            SqlCommand sqlCmd = sqlCon.CreateCommand();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

            sqlCmd.CommandText = cmdText;
            sqlCmd.CommandType = type;

            if (parms != null)
            {
                foreach (SqlParameter p in parms)
                {
                    sqlCmd.Parameters.Add(p);
                }
            }

            adapter.SelectCommand = sqlCmd;
            DataTable dt = new DataTable();
            try
            {
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        
    }
}