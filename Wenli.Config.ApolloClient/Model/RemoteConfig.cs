using System.Collections.Generic;

namespace Wenli.Config.ApolloClient.Model
{
    /// <summary>
    /// apollo配置实体类
    /// </summary>
    public class RemoteConfig
    {
        private string appId;

        private string cluster;

        private string namespaceName;

        private Dictionary<string, string> configurations;

        private string releaseKey;

        /// <summary>
        /// apollo配置实体类
        /// </summary>
        public RemoteConfig()
        {
        }
        /// <summary>
        /// apollo配置实体类
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="cluster"></param>
        /// <param name="namespaceName"></param>
        /// <param name="releaseKey"></param>
        public RemoteConfig(string appId, string cluster, string namespaceName, string releaseKey)
        {
            this.appId = appId;
            this.cluster = cluster;
            this.namespaceName = namespaceName;
            this.releaseKey = releaseKey;
        }

        /// <summary>
        /// 应用id
        /// </summary>
        public string AppId
        {
            get
            {
                return appId;
            }
            set
            {
                this.appId = value;
            }
        }
        /// <summary>
        /// 集群
        /// </summary>
        public string Cluster
        {
            get
            {
                return cluster;
            }
            set
            {
                this.cluster = value;
            }
        }
        /// <summary>
        /// 命名空间
        /// </summary>
        public string NamespaceName
        {
            get
            {
                return namespaceName;
            }
            set
            {
                this.namespaceName = value;
            }
        }
        /// <summary>
        /// 发布的key
        /// </summary>
        public string ReleaseKey
        {
            get
            {
                return releaseKey;
            }
            set
            {
                this.releaseKey = value;
            }
        }
        /// <summary>
        /// 配置的集合
        /// </summary>
        public Dictionary<string, string> Configurations
        {
            get
            {
                return configurations;
            }
            set
            {
                this.configurations = value;
            }
        }
        /// <summary>
        /// 格式化的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ApolloConfig{" + "appId='" + appId + '\'' + ", cluster='" + cluster + '\'' +
                ", namespaceName='" + namespaceName + '\'' + ", configurations=" + configurations +
                ", releaseKey='" + releaseKey + '\'' + '}';
        }
    }
    /// <summary>
    /// apollo配置集合实体类
    /// </summary>
    public class RemoteConfigs : List<RemoteConfig>
    {
    }
}
