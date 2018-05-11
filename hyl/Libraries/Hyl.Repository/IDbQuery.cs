using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Hyl.Repository
{
    /// <summary>
    /// 多表联合查询使用
    /// </summary>
    public interface IDbQuery
    {
        IDbConnection GetDbConnection();

        T QuerySingle<T>(string sql, object paramPairs) where T : class ;
        IEnumerable<T> QueryList<T>(string sql, object paramPairs) where T : class;
        IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> fun, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null);
        IEnumerable<TReturn> Query<TFirst, TSecond, TThired, TReturn>(string sql, Func<TFirst, TSecond, TThired, TReturn> fun, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null);
        int ExecuteSql(string sql, dynamic paramPairs = null);
        int ExecuteProcSql(string sql, object paramPairs = null);
        T QuerySingleProc<T>(string procName, object paramPairs) where T : class;
        long Count(string sql, dynamic paramPairs = null);
        IEnumerable<T> QueryListProc<T>(string sql, object paramPairs) where T : class;



        Task<T> QuerySingleAsync<T>(string sql, object paramPairs) where T : class;
        Task<IEnumerable<T>> QueryListAsync<T>(string sql, object paramPairs) where T : class;
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> fun, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null);
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThired, TReturn>(string sql, Func<TFirst, TSecond, TThired, TReturn> fun, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null);
        Task<int> ExecuteSqlAsync(string sql, dynamic paramPairs = null);
        Task<T> QuerySingleProcAsync<T>(string procName, object paramPairs) where T : class;
        Task<long> CountAsync(string sql, dynamic paramPairs = null);
        Task<IEnumerable<T>> QueryListProcAsync<T>(string sql, object paramPairs) where T : class;
    }
}
