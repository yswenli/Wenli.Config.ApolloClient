using Wenli.Config.ApolloClient.Newtonsoft.Json;
using System.Collections.Generic;

namespace Wenli.Config.ApolloClient.Model
{
    /// <summary>
    /// 服务配置项实体
    /// </summary>
    public class ServiceConfig
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        [JsonProperty(PropertyName = "appName")]
        public string AppName
        {
            get; set;
        }
        /// <summary>
        /// 实例id
        /// </summary>
        [JsonProperty(PropertyName = "instanceId")]
        public string InstanceID
        {
            get; set;
        }
        /// <summary>
        /// 配置项地扯
        /// </summary>
        [JsonProperty(PropertyName = "homepageUrl")]
        public string HomePageUrl
        {
            get; set;
        }
    }
    /// <summary>
    /// 服务配置项集合实体
    /// </summary>
    public class ServiceConfigs : List<ServiceConfig>
    {

    }
}
