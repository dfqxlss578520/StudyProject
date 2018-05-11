using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;
using Dapper.Contrib.Extensions;
using Dapper;
using System.Threading.Tasks;

namespace Hyl.Repository.Dapper
{
    /// <summary>
    /// The Dapper.Contrib extensions for Dapper
    /// </summary>
    public static partial class SqlMapperExtensions
    {
        /// <summary>
        /// The function to get a database type from the given <see cref="IDbConnection"/>.
        /// </summary>
        /// <param name="connection">The connection to get a database type name from.</param>
        public delegate string GetDatabaseTypeDelegate(IDbConnection connection);
        /// <summary>
        /// The function to get a a table name from a given <see cref="Type"/>
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get a table name for.</param>
        public delegate string TableNameMapperDelegate(Type type);

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> KeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> ExplicitKeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();

        private static readonly ISqlAdapter DefaultAdapter = new SqlServerAdapter();
        private static readonly Dictionary<string, ISqlAdapter> AdapterDictionary
            = new Dictionary<string, ISqlAdapter>
            {
                ["sqlconnection"] = new SqlServerAdapter(),
                ["sqlceconnection"] = new SqlCeServerAdapter(),
                ["npgsqlconnection"] = new PostgresAdapter(),
                ["sqliteconnection"] = new SQLiteAdapter(),
                ["mysqlconnection"] = new MySqlAdapter(),
                ["fbconnection"] = new FbAdapter()
            };

        private static List<PropertyInfo> ExplicitKeyPropertiesCache(Type type)
        {
            if (ExplicitKeyProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pi))
            {
                return pi.ToList();
            }

            var explicitKeyProperties = TypePropertiesCache(type).Where(p => p.GetCustomAttributes(true).Any(a => a is ExplicitKeyAttribute)).ToList();

            ExplicitKeyProperties[type.TypeHandle] = explicitKeyProperties;
            return explicitKeyProperties;
        }

        private static List<PropertyInfo> KeyPropertiesCache(Type type)
        {
            if (KeyProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pi))
            {
                return pi.ToList();
            }

            var allProperties = TypePropertiesCache(type);
            var keyProperties = allProperties.Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).ToList();

            if (keyProperties.Count == 0)
            {
                var idProp = allProperties.FirstOrDefault(p => p.Name.ToLower() == "id");
                if (idProp != null && !idProp.GetCustomAttributes(true).Any(a => a is ExplicitKeyAttribute))
                {
                    keyProperties.Add(idProp);
                }
            }

            KeyProperties[type.TypeHandle] = keyProperties;
            return keyProperties;
        }

        private static List<PropertyInfo> TypePropertiesCache(Type type)
        {
            if (TypeProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pis))
            {
                return pis.ToList();
            }

            var properties = type.GetProperties().Where(IsWriteable).ToArray();
            TypeProperties[type.TypeHandle] = properties;
            return properties.ToList();
        }

        private static bool IsWriteable(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(typeof(WriteAttribute), false).AsList();
            if (attributes.Count != 1) return true;

            var writeAttribute = (WriteAttribute)attributes[0];
            return writeAttribute.Write;
        }

        /// <summary>
        /// Specify a custom table name mapper based on the POCO type name
        /// </summary>
        public static TableNameMapperDelegate TableNameMapper;

        private static string GetTableName(Type type)
        {
            if (TypeTableName.TryGetValue(type.TypeHandle, out string name)) return name;

            if (TableNameMapper != null)
            {
                name = TableNameMapper(type);
            }
            else
            {
                //NOTE: This as dynamic trick should be able to handle both our own Table-attribute as well as the one in EntityFramework 
                var tableAttr = type
#if COREFX
                    .GetTypeInfo()
#endif
                    .GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic;
                if (tableAttr != null)
                {
                    name = tableAttr.Name;
                }
                else
                {
                    name = type.Name + "s";
                    if (type.IsInterface && name.StartsWith("I"))
                        name = name.Substring(1);
                }
            }

            TypeTableName[type.TypeHandle] = name;
            return name;
        }

        /// <summary>
        /// Specifies a custom callback that detects the database type instead of relying on the default strategy (the name of the connection type object).
        /// Please note that this callback is global and will be used by all the calls that require a database specific adapter.
        /// </summary>
        public static GetDatabaseTypeDelegate GetDatabaseType;

        private static ISqlAdapter GetFormatter(IDbConnection connection)
        {
            var name = GetDatabaseType?.Invoke(connection).ToLower()
                       ?? connection.GetType().Name.ToLower();

            return !AdapterDictionary.ContainsKey(name)
                ? DefaultAdapter
                : AdapterDictionary[name];
        }

        
        
        /// <summary>
        /// <para>By default queries the table matching the class name</para>
        /// <para>-Table name can be overridden by adding an attribute on your class [Table("YourTableName")]</para>
        /// <para>conditions is an SQL where clause ex: "where name='bob'" or "where age>=@Age" - not required </para>
        /// <para>orderby is a column or list of columns to order by ex: "lastname, age desc" - not required - default is by primary key</para>
        /// <para>parameters is an anonymous type to pass in named parameter values: new { Age = 15 }</para>
        /// <para>Supports transaction and command timeout</para>
        /// <para>Returns a list of entities that match where conditions</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fieleds"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>Gets a paged list of entities with optional exact match where conditions</returns>
        public static IEnumerable<T> GetPagedList<T>(this IDbConnection connection, int pageIndex, int pageSize, string fieleds, string conditions, string orderby, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var query = GetFormatter(connection).GetPaginationSql();

            if (string.IsNullOrEmpty(query))
                throw new Exception("GetListPage is not supported with the current SQL Dialect");

            var currenttype = typeof(T);
            var keyProperties = KeyPropertiesCache(currenttype).ToList();  //added ToList() due to issue #418, must work on a list copy
            var explicitKeyProperties = ExplicitKeyPropertiesCache(currenttype);
            if (keyProperties.Count == 0 && explicitKeyProperties.Count == 0)
                throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");

            if (string.IsNullOrEmpty(orderby))
            {
                orderby = keyProperties.FirstOrDefault() != null ? keyProperties.FirstOrDefault().Name : explicitKeyProperties.FirstOrDefault().Name;
            }

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 20;
            if (string.IsNullOrEmpty(fieleds)) fieleds = "*";

            //create a new empty instance of the type to get the base properties
            query = query.Replace("{SelectColumns}", fieleds);
            query = query.Replace("{TableName}", GetTableName(currenttype));
            query = query.Replace("{PageNumber}", pageIndex.ToString());
            query = query.Replace("{RowsPerPage}", pageSize.ToString());
            query = query.Replace("{OrderBy}", orderby);
            query = query.Replace("{WhereClause}", conditions);
            query = query.Replace("{Offset}", ((pageIndex - 1) * pageSize).ToString());

            return connection.Query<T>(query, parameters, transaction, true, commandTimeout);
        }
        /// <summary>
        /// <para>By default queries the table matching the class name</para>
        /// <para>-Table name can be overridden by adding an attribute on your class [Table("YourTableName")]</para>
        /// <para>conditions is an SQL where clause ex: "where name='bob'" or "where age>=@Age" - not required </para>
        /// <para>orderby is a column or list of columns to order by ex: "lastname, age desc" - not required - default is by primary key</para>
        /// <para>parameters is an anonymous type to pass in named parameter values: new { Age = 15 }</para>
        /// <para>Supports transaction and command timeout</para>
        /// <para>Returns a list of entities that match where conditions</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fieleds"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>Gets a paged list of entities with optional exact match where conditions</returns>
        public static Tuple<int, IEnumerable<T>> GetPaged<T>(this IDbConnection connection, int pageIndex, int pageSize, string fieleds, string conditions, string orderby, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var query = GetFormatter(connection).GetPaginationSql();

            if (string.IsNullOrEmpty(query))
                throw new Exception("GetListPage is not supported with the current SQL Dialect");

            var currenttype = typeof(T);
            var keyProperties = KeyPropertiesCache(currenttype).ToList();  //added ToList() due to issue #418, must work on a list copy
            var explicitKeyProperties = ExplicitKeyPropertiesCache(currenttype);
            if (keyProperties.Count == 0 && explicitKeyProperties.Count == 0)
                throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");

            if (string.IsNullOrEmpty(orderby))
            {
                orderby = keyProperties.FirstOrDefault() != null ? keyProperties.FirstOrDefault().Name : explicitKeyProperties.FirstOrDefault().Name;
            }

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 20;
            if (string.IsNullOrEmpty(fieleds)) fieleds = "*";

            //create a new empty instance of the type to get the base properties
            query = query.Replace("{SelectColumns}", fieleds);
            query = query.Replace("{TableName}", GetTableName(currenttype));
            query = query.Replace("{PageNumber}", pageIndex.ToString());
            query = query.Replace("{RowsPerPage}", pageSize.ToString());
            query = query.Replace("{OrderBy}", orderby);
            query = query.Replace("{WhereClause}", conditions);
            query = query.Replace("{Offset}", ((pageIndex - 1) * pageSize).ToString());

            using (var multiRst = connection.QueryMultiple(query, parameters, transaction, commandTimeout))
            {
                var data = multiRst.Read<T>();
                var totalNums = multiRst.Read<int>().FirstOrDefault();
                return Tuple.Create(totalNums, data);
            }
        }

        /// <summary>
        /// <para>By default queries the table matching the class name</para>
        /// <para>-Table name can be overridden by adding an attribute on your class [Table("YourTableName")]</para>
        /// <para>conditions is an SQL where clause ex: "where name='bob'" or "where age>=@Age" - not required </para>
        /// <para>orderby is a column or list of columns to order by ex: "lastname, age desc" - not required - default is by primary key</para>
        /// <para>parameters is an anonymous type to pass in named parameter values: new { Age = 15 }</para>
        /// <para>Supports transaction and command timeout</para>
        /// <para>Returns a list of entities that match where conditions</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fieleds"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>Gets a paged list of entities with optional exact match where conditions</returns>
        public static async Task<IEnumerable<T>> GetPagedListAsync<T>(this IDbConnection connection, int pageIndex, int pageSize, string fieleds, string conditions, string orderby, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var query = GetFormatter(connection).GetPaginationSql();

            if (string.IsNullOrEmpty(query))
                throw new Exception("GetListPage is not supported with the current SQL Dialect");

            var currenttype = typeof(T);
            var keyProperties = KeyPropertiesCache(currenttype).ToList();  //added ToList() due to issue #418, must work on a list copy
            var explicitKeyProperties = ExplicitKeyPropertiesCache(currenttype);
            if (keyProperties.Count == 0 && explicitKeyProperties.Count == 0)
                throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");

            if (string.IsNullOrEmpty(orderby))
            {
                orderby = keyProperties.FirstOrDefault() != null ? keyProperties.FirstOrDefault().Name : explicitKeyProperties.FirstOrDefault().Name;
            }

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 20;
            if (string.IsNullOrEmpty(fieleds)) fieleds = "*";

            //create a new empty instance of the type to get the base properties
            query = query.Replace("{SelectColumns}", fieleds);
            query = query.Replace("{TableName}", GetTableName(currenttype));
            query = query.Replace("{PageNumber}", pageIndex.ToString());
            query = query.Replace("{RowsPerPage}", pageSize.ToString());
            query = query.Replace("{OrderBy}", orderby);
            query = query.Replace("{WhereClause}", conditions);
            query = query.Replace("{Offset}", ((pageIndex - 1) * pageSize).ToString());
            
            return await connection.QueryAsync<T>(query, parameters, transaction, commandTimeout);
        }
        /// <summary>
        /// <para>By default queries the table matching the class name</para>
        /// <para>-Table name can be overridden by adding an attribute on your class [Table("YourTableName")]</para>
        /// <para>conditions is an SQL where clause ex: "where name='bob'" or "where age>=@Age" - not required </para>
        /// <para>orderby is a column or list of columns to order by ex: "lastname, age desc" - not required - default is by primary key</para>
        /// <para>parameters is an anonymous type to pass in named parameter values: new { Age = 15 }</para>
        /// <para>Supports transaction and command timeout</para>
        /// <para>Returns a list of entities that match where conditions</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fieleds"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>Gets a paged list of entities with optional exact match where conditions</returns>
        public static async Task<Tuple<int, IEnumerable<T>>> GetPagedAsync<T>(this IDbConnection connection, int pageIndex, int pageSize, string fieleds, string conditions, string orderby, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var query = GetFormatter(connection).GetPaginationSql();

            if (string.IsNullOrEmpty(query))
                throw new Exception("GetListPage is not supported with the current SQL Dialect");

            var currenttype = typeof(T);
            var keyProperties = KeyPropertiesCache(currenttype).ToList();  //added ToList() due to issue #418, must work on a list copy
            var explicitKeyProperties = ExplicitKeyPropertiesCache(currenttype);
            if (keyProperties.Count == 0 && explicitKeyProperties.Count == 0)
                throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");

            if (string.IsNullOrEmpty(orderby))
            {
                orderby = keyProperties.FirstOrDefault() != null ? keyProperties.FirstOrDefault().Name : explicitKeyProperties.FirstOrDefault().Name;
            }

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 20;
            if (string.IsNullOrEmpty(fieleds)) fieleds = "*";

            //create a new empty instance of the type to get the base properties
            query = query.Replace("{SelectColumns}", fieleds);
            query = query.Replace("{TableName}", GetTableName(currenttype));
            query = query.Replace("{PageNumber}", pageIndex.ToString());
            query = query.Replace("{RowsPerPage}", pageSize.ToString());
            query = query.Replace("{OrderBy}", orderby);
            query = query.Replace("{WhereClause}", conditions);
            query = query.Replace("{Offset}", ((pageIndex - 1) * pageSize).ToString());

            using (var multiRst = await connection.QueryMultipleAsync(query, parameters, transaction, commandTimeout))
            {
                var data = multiRst.Read<T>();
                var totalNums = multiRst.Read<int>().FirstOrDefault();
                return Tuple.Create(totalNums, data);
            }
        }

    }

}

/// <summary>
/// The interface for all Dapper.Contrib database operations
/// Implementing this is each provider's model.
/// </summary>
public partial interface ISqlAdapter
{
    string GetListPaginationSql();
    string GetPaginationSql();
}

/// <summary>
/// The SQL Server database adapter.
/// </summary>
public partial class SqlServerAdapter : ISqlAdapter
{
    public string GetListPaginationSql()
    {
        return "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {OrderBy}) AS PagedNumber, {SelectColumns} FROM {TableName} {WhereClause}) AS u WHERE PagedNUMBER BETWEEN (({PageNumber}-1) * {RowsPerPage} + 1) AND ({PageNumber} * {RowsPerPage});";
    }
    public string GetPaginationSql()
    {
        return "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {OrderBy}) AS PagedNumber, {SelectColumns} FROM {TableName} {WhereClause}) AS u WHERE PagedNUMBER BETWEEN (({PageNumber}-1) * {RowsPerPage} + 1) AND ({PageNumber} * {RowsPerPage});SELECT COUNT(1) FROM {TableName} {WhereClause};";
    }
}

/// <summary>
/// The SQL Server Compact Edition database adapter.
/// </summary>
public partial class SqlCeServerAdapter : ISqlAdapter
{
    public string GetListPaginationSql()
    {
        return string.Empty;
    }

    public string GetPaginationSql()
    {
        return string.Empty;
    }
}

/// <summary>
/// The MySQL database adapter.
/// </summary>
public partial class MySqlAdapter : ISqlAdapter
{
    public string GetListPaginationSql()
    {
        return string.Empty;
    }

    public string GetPaginationSql()
    {
        return "Select {SelectColumns} from {TableName} {WhereClause} Order By {OrderBy} LIMIT {Offset},{RowsPerPage}";
    }
}

/// <summary>
/// The Postgres database adapter.
/// </summary>
public partial class PostgresAdapter : ISqlAdapter
{
    public string GetListPaginationSql()
    {
        return string.Empty;
    }

    public string GetPaginationSql()
    {
        return "Select {SelectColumns} from {TableName} {WhereClause} Order By {OrderBy} LIMIT {RowsPerPage} OFFSET (({PageNumber}-1) * {RowsPerPage})";
    }
}

/// <summary>
/// The SQLite database adapter.
/// </summary>
public partial class SQLiteAdapter : ISqlAdapter
{
    public string GetListPaginationSql()
    {
        return string.Empty;
    }

    public string GetPaginationSql()
    {
        return "Select {SelectColumns} from {TableName} {WhereClause} Order By {OrderBy} LIMIT {RowsPerPage} OFFSET (({PageNumber}-1) * {RowsPerPage})";
    }
}

/// <summary>
/// The Firebase SQL adapeter.
/// </summary>
public partial class FbAdapter : ISqlAdapter
{
    public string GetListPaginationSql()
    {
        return string.Empty;
    }

    public string GetPaginationSql()
    {
        return string.Empty;
    }
}