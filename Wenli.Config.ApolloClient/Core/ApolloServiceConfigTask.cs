/****************************************************************************
*项目名称：Wenli.Config.ApolloClient.Base
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：Wenli.Config.ApolloClient.Base
*类 名 称：ApolloServiceConfigTask
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/1/8 15:15:55
*描述：
*=====================================================================
*修改时间：2019/1/8 15:15:55
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using Wenli.Config.ApolloClient.Common;
using Wenli.Config.ApolloClient.Model;
using System;
using System.Threading;
using System.Linq;

namespace Wenli.Config.ApolloClient.Core
{
    /// <summary>
    /// 获取配置任务
    /// </summary>
    class ApolloServiceConfigTask
    {
        HttpHelper _httpHelper;

        ApolloConfig _apolloConfig;

        string _cachePath = string.Empty;

        /// <summary>
        /// 获取配置任务
        /// </summary>
        /// <param name="apolloConfig"></param>
        public ApolloServiceConfigTask(ApolloConfig apolloConfig)
        {
            _apolloConfig = apolloConfig;

            _httpHelper = new HttpHelper(_apolloConfig.Timeout, _apolloConfig.ReadTimeout);

            _cachePath = _apolloConfig.CachePath;
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="appIDs"></param>
        /// <param name="service"></param>
        /// <param name="cluster"></param>
        public void GetConfig(string[] appIDs, ServiceConfig service, string cluster = "default")
        {
            RemoteConfigs remoteConfigs = new RemoteConfigs();

            foreach (var appID in appIDs)
            {
                var data = GetConfigFromServer(service.HomePageUrl, appID, _apolloConfig.Env);

                if (data != null)
                {
                    if (data.Configurations != null && data.Configurations.Any())
                        ConfigCollection.Set(_apolloConfig.Env, data);
                    else
                        ConfigCollection.Remove(_apolloConfig.Env, data.Cluster, data.AppId);

                    //cluster = data.Cluster; 移除本行代码，避免灰度异常

                    if (PathHelper.Create(_cachePath))
                    {
                        var fileName = FileHelper.GetCacheName(_cachePath, _apolloConfig.Env, data.Cluster, data.AppId);

                        FileHelper.Write(fileName, data);
                    }
                }
                else
                {
                    data = GetConfigFromLocal(appID, cluster);
                }
            }
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="serverUrl"></param>
        /// <param name="appID"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public RemoteConfig GetConfigFromServer(string serverUrl, string appID, string env)
        {
            var url = TaskUrlHelper.GetConfigUrl(serverUrl, appID, env);

            for (int i = 0; i < _apolloConfig.MaxRetries; i++)
            {
                var remoteConfig = _httpHelper.DoGet<RemoteConfig>(new HttpRequest($"{url}&rnd={Environment.TickCount}")).Body;

                if (remoteConfig != null)
                {
                    return remoteConfig;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取本地配置
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="cluster"></param>
        /// <returns></returns>
        public RemoteConfig GetConfigFromLocal(string appID, string cluster = "default")
        {
            var fileName = FileHelper.GetCacheName(_cachePath, _apolloConfig.Env, cluster, appID);
            return FileHelper.Read<RemoteConfig>(fileName);
        }
    }
}
