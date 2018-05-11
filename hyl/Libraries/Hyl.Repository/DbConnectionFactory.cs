using StackExchange.Profiling;
using System.Data;
using Hyl.Core.Configuration;

namespace Hyl.Repository
{
    public class DbConnectionFactory
    {
        private static string connectionString;
        private static string databaseType;
        

        public DbConnectionFactory(HylWebConfig config)
        {            
            connectionString = config.ConnectionString;
            databaseType = config.DbType;
        }

        public IDbConnection CreateDbConnection()
        {
            IDbConnection connection = null;
            switch (databaseType)
            {
                case "sqlserver":
                    var cnn = new System.Data.SqlClient.SqlConnection(connectionString);
                    connection = new StackExchange.Profiling.Data.ProfiledDbConnection(cnn, MiniProfiler.Current);
                    //connection = new System.Data.SqlClient.SqlConnection(connectionString);
                    break;
                case "mysql":
                    //connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                    break;
                case "oracle":
                    //connection = new Oracle.DataAccess.Client.OracleConnection(connectionString);
                    //connection = new System.Data.OracleClient.OracleConnection(connectionString);
                    break;
                case "db2":
                    //connection = new System.Data.OleDb.OleDbConnection(connectionString);
                    break;
                default:
                    connection = new System.Data.SqlClient.SqlConnection(connectionString);
                    break;
            }
            return connection;
        }
    }
}
