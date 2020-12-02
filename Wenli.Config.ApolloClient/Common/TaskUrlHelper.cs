using System;
using System.Web;

namespace Wenli.Config.ApolloClient.Common
{
    /// <summary>
    /// 任务url工具类
    /// </summary>
    public static class TaskUrlHelper
    {
        static readonly string localIP = NetHelper.GetLocalIP();


        /// <summary>
        /// 获取服务url
        /// </summary>
        /// <param name="serverUrl"></param>
        /// <returns></returns>
        public static string GetServiceUrl(string serverUrl)
        {
            UriBuilder uriBuilder = new UriBuilder(serverUrl + "services/config");

            var query = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(localIP))
            {
                query["ip"] = localIP;
            }

            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }

        /// <summary>
        /// 获取配置url
        /// </summary>
        /// <param name="serverUrl"></param>
        /// <param name="appID"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static string GetConfigUrl(string serverUrl, string appID, string env)
        {
            UriBuilder uriBuilder = new UriBuilder($"{serverUrl}configs/{appID}/{env}/application");

            var query = HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(localIP))
            {
                query["ip"] = localIP;
            }

            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }

        /// <summary>
        /// 获取长轮询url
        /// </summary>
        /// <param name="serverUrl"></param>
        /// <param name="appID"></param>
        /// <param name="cluster"></param>
        /// <returns></returns>
        public static string GetLongPollingUrl(string serverUrl, string appID, string cluster, long notificationId = -1)
        {
            UriBuilder uriBuilder = new UriBuilder($"{serverUrl}notifications/v2");

            var query = HttpUtility.ParseQueryString(string.Empty);

            query["appId"] = appID;

            query["cluster"] = cluster;

            query["notifications"] = "[{\"namespaceName\":\"application\",\"notificationId\":"+ notificationId + "}]";

            if (!string.IsNullOrEmpty(localIP))
            {
                query["ip"] = localIP;
            }

            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }

    }
}
