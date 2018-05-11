using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Hyl.Repository
{
    /// <summary>
    /// 多表联合查询使用
    /// </summary>
    public class DbQuery : IDbQuery
    {
        private IDbConnection Conn { get; set; }
        public DbQuery(IDbConnection connection)
        {
            Conn = connection;
        }

        public IDbConnection GetDbConnection()
        {
            return Conn;
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> fun, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return Conn.Query(sql, fun, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        public IEnumerable<TReturn> Query<TFirst, TSecond, TThired, TReturn>(string sql, Func<TFirst, TSecond, TThired, TReturn> fun, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return Conn.Query(sql, fun, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        /// <summary>
        /// 执行一段sql返回int值
        /// usage: ExecuteSql(@"insert User(colA, colB) values (@a, @b)",new {a = 1,b = 2})
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql, object paramPairs = null)
        {
            return Conn.Execute(sql, paramPairs);
        }

        /// <summary>
        /// 执行一段sql返回int值
        /// usage: ExecuteSql(@"insert User(colA, colB) values (@a, @b)",new {a = 1,b = 2})
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public int ExecuteProcSql(string sql, object paramPairs = null)
        {
            return Conn.Execute(sql, paramPairs,commandType:CommandType.StoredProcedure);
        }

        /// <summary>
        /// 执行sql返回统计值
        /// usage: Count(@"select count(1) from user where id>@a",new {a = 5})
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public long Count(string sql, object paramPairs = null)
        {
            return Conn.Query<long>(sql, paramPairs).SingleOrDefault();
        }


        /// <summary>
        /// 执行sql返回单个数据模型
        /// usage: QuerySingle<User>(@"insert User(colA, colB) values (@a, @b)",new {a = 1,b = 2})
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public T QuerySingle<T>(string sql, object paramPairs) where T : class
        {
            return Conn.Query<T>(sql, paramPairs).SingleOrDefault();
        }


        /// <summary>
        /// 执行存储过程返回模型
        /// usage: QuerySingleProc<User>("spGetUser", new {Id = 1}）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public T QuerySingleProc<T>(string procName, object paramPairs) where T : class
        {
            return Conn.Query<T>(procName, paramPairs, commandType: CommandType.StoredProcedure).SingleOrDefault();
        }


        /// <summary>
        /// 执行sql返回列表
        /// usage: QueryList<User>(@"select * from users where uid> @a",new {a = 1})
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryList<T>(string sql, object paramPairs) where T : class
        {
            return Conn.Query<T>(sql, paramPairs);
        }

        /// <summary>
        /// 执行sql返回列表
        /// usage: QueryListProc<User>(@"spGetUserList",new {a = 1})
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryListProc<T>(string sql, object paramPairs) where T : class
        {
            return Conn.Query<T>(sql, paramPairs, commandType: CommandType.StoredProcedure);
        }



        #region Async
        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> fun, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return await Conn.QueryAsync(sql, fun, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThired, TReturn>(string sql, Func<TFirst, TSecond, TThired, TReturn> fun, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return await Conn.QueryAsync(sql, fun, param, transaction, buffered, splitOn, commandTimeout, commandType);
        }

        /// <summary>
        /// 执行一段sql返回int值
        /// usage: ExecuteSql(@"insert User(colA, colB) values (@a, @b)",new {a = 1,b = 2})
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlAsync(string sql, object paramPairs = null)
        {
            return await Conn.ExecuteAsync(sql, paramPairs);
        }

        /// <summary>
        /// 执行sql返回统计值
        /// usage: Count(@"select count(1) from user where id>@a",new {a = 5})
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public async Task<long> CountAsync(string sql, object paramPairs = null)
        {
            return (await Conn.QueryAsync<long>(sql, paramPairs)).SingleOrDefault();
        }


        /// <summary>
        /// 执行sql返回单个数据模型
        /// usage: QuerySingle<User>(@"insert User(colA, colB) values (@a, @b)",new {a = 1,b = 2})
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public async Task<T> QuerySingleAsync<T>(string sql, object paramPairs) where T : class
        {
            return (await Conn.QueryAsync<T>(sql, paramPairs)).SingleOrDefault();
        }


        /// <summary>
        /// 执行存储过程返回模型
        /// usage: QuerySingleProc<User>("spGetUser", new {Id = 1}）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public async Task<T> QuerySingleProcAsync<T>(string procName, object paramPairs) where T : class
        {
            return (await Conn.QueryAsync<T>(procName, paramPairs, commandType: CommandType.StoredProcedure)).SingleOrDefault();
        }


        /// <summary>
        /// 执行sql返回列表
        /// usage: QueryList<User>(@"select * from users where uid> @a",new {a = 1})
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryListAsync<T>(string sql, object paramPairs) where T : class
        {
            return await Conn.QueryAsync<T>(sql, paramPairs);
        }

        /// <summary>
        /// 执行sql返回列表
        /// usage: QueryListProc<User>(@"spGetUserList",new {a = 1})
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryListProcAsync<T>(string sql, object paramPairs) where T : class
        {
            return await Conn.QueryAsync<T>(sql, paramPairs, commandType: CommandType.StoredProcedure);
        } 
        #endregion

    }
}
