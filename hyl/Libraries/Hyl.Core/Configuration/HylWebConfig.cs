using System;
using System.Configuration;
using System.Xml;

namespace Hyl.Core.Configuration
{
    public partial class HylWebConfig : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="section">Section XML node.</param>
        /// <returns>The created section handler object.</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new HylWebConfig();

            var startupNode = section.SelectSingleNode("Startup");
            config.IgnoreStartupTasks = GetBool(startupNode, "IgnoreStartupTasks");

            var dbfactory = section.SelectSingleNode("DbFactory");
            config.ConnectionString = GetString(dbfactory, "ConnectionString");
            config.DbType = GetString(dbfactory, "DbType");
            config.ForceToLog = GetBool(dbfactory, "ForceToLog");

            var redisCachingNode = section.SelectSingleNode("RedisCaching");
            config.RedisCachingEnabled = GetBool(redisCachingNode, "Enabled");
            config.RedisCachingConnectionString = GetString(redisCachingNode, "ConnectionString");


            var webFarmsNode = section.SelectSingleNode("WebFarms");
            config.MultipleInstancesEnabled = GetBool(webFarmsNode, "MultipleInstancesEnabled");
            config.RunOnAzureWebApps = GetBool(webFarmsNode, "RunOnAzureWebApps");

            var azureBlobStorageNode = section.SelectSingleNode("AzureBlobStorage");
            config.AzureBlobStorageConnectionString = GetString(azureBlobStorageNode, "ConnectionString");
            config.AzureBlobStorageContainerName = GetString(azureBlobStorageNode, "ContainerName");
            config.AzureBlobStorageEndPoint = GetString(azureBlobStorageNode, "EndPoint");

            var gk = section.SelectSingleNode("Gk");
            config.IntegrateGuanKong = GetBool(gk, "IntegrateGuanKong");
            config.GkConnectionString = GetString(gk, "GkConnectionString");
            config.GkUrl = GetString(gk, "GkUrl");

            return config;
        }

        public string GetString(XmlNode node, string attrName)
        {
            return SetByXElement<string>(node, attrName, Convert.ToString);
        }

        private bool GetBool(XmlNode node, string attrName)
        {
            return SetByXElement<bool>(node, attrName, Convert.ToBoolean);
        }

        private T SetByXElement<T>(XmlNode node, string attrName, Func<string, T> converter)
        {
            if (node == null || node.Attributes == null) return default(T);
            var attr = node.Attributes[attrName];
            if (attr == null) return default(T);
            var attrVal = attr.Value;
            return converter(attrVal);
        }

        /// <summary>
        /// 启动时是否启动任务
        /// </summary>
        public bool IgnoreStartupTasks { get; private set; }


        public string ConnectionString { get; set; }
        /// <summary>
        /// 数据库类型 Hyl.Repository.DbConnectionFactory.CreateDbConnection()
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// 强制日志组件使用此链接字符串
        /// </summary>
        public bool ForceToLog { get; set; }


        /// <summary>
        /// Indicates whether we should use Redis server for caching (instead of default in-memory caching)
        /// </summary>
        public bool RedisCachingEnabled { get; private set; }
        /// <summary>
        /// Redis connection string. Used when Redis caching is enabled
        /// </summary>
        public string RedisCachingConnectionString { get; private set; }


        /// <summary>
        /// A value indicating whether the site is run on multiple instances (e.g. web farm, Windows Azure with multiple instances, etc).
        /// Do not enable it if you run on Azure but use one instance only
        /// </summary>
        public bool MultipleInstancesEnabled { get; private set; }

        /// <summary>
        /// A value indicating whether the site is run on Windows Azure Web Apps
        /// </summary>
        public bool RunOnAzureWebApps { get; private set; }

        /// <summary>
        /// Connection string for Azure BLOB storage
        /// </summary>
        public string AzureBlobStorageConnectionString { get; private set; }
        /// <summary>
        /// Container name for Azure BLOB storage
        /// </summary>
        public string AzureBlobStorageContainerName { get; private set; }
        /// <summary>
        /// End point for Azure BLOB storage
        /// </summary>
        public string AzureBlobStorageEndPoint { get; private set; }

        /// <summary>
        /// Integrate with Gk_Dvp project
        /// </summary>
        public bool IntegrateGuanKong { get; set; }
        public string GkConnectionString { get; set; }
        public string GkUrl { get; set; }


    }
}
