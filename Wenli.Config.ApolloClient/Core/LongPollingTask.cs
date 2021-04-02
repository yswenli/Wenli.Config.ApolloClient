/****************************************************************************
*项目名称：Wenli.Config.ApolloClient.Base
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：Wenli.Config.ApolloClient.Base
*类 名 称：LongPollingTask
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/1/8 16:41:25
*描述：
*=====================================================================
*修改时间：2019/1/8 16:41:25
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using Wenli.Config.ApolloClient.Common;
using Wenli.Config.ApolloClient.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Wenli.Config.ApolloClient.Core
{
    /// <summary>
    /// 长轮询通知任务
    /// </summary>
    class LongPollingTask
    {
        bool _start = false;

        ApolloConfig _apolloConfig;

        ServiceConfigs _serviceConfigs;

        HttpHelper _httpHelper;

        ApolloServiceConfigTask _getConfigTask;

        long _notificationId = -1;

        public event OnErrorHandler OnError;

        /// <summary>
        /// 长轮询通知任务
        /// </summary>
        /// <param name="apolloConfig"></param>
        /// <param name="serviceConfigs"></param>
        public LongPollingTask(ApolloConfig apolloConfig, ServiceConfigs serviceConfigs)
        {
            _apolloConfig = apolloConfig;

            _serviceConfigs = serviceConfigs;

            _httpHelper = new HttpHelper(_apolloConfig.Timeout, _apolloConfig.ReadTimeout);

            _getConfigTask = new ApolloServiceConfigTask(apolloConfig);
        }

        /// <summary>
        /// 长轮询
        /// </summary>
        /// <param name="appIDs"></param>
        /// <param name="cluster"></param>
        public void Start(string[] appIDs, string cluster = "default")
        {
            if (!_start)
            {
                _start = true;

                Task.Factory.StartNew(() =>
                {
                    while (_start)
                    {
                        GetApolloConfigs(appIDs, cluster);

                        Thread.Yield();
                    }

                }, TaskCreationOptions.LongRunning);
            }
        }

        /// <summary>
        /// 获取阻塞式的配置数据
        /// </summary>
        /// <param name="appIDs"></param>
        /// <param name="cluster"></param>
        void GetApolloConfigs(string[] appIDs, string cluster = "default")
        {
            var tasks = new List<Task>();

            foreach (var serviceConfig in _serviceConfigs)
            {
                var task = Task.Run(() =>
                {
                    try
                    {
                        foreach (var appID in appIDs)
                        {
                            var url = TaskUrlHelper.GetLongPollingUrl(serviceConfig.HomePageUrl, appID, cluster, _notificationId);

                            for (int i = 0; i < _apolloConfig.MaxRetries; i++)
                            {
                                var response = _httpHelper.DoGet<LongPollings>(new HttpRequest(url, 600 * 1000));

                                if (response.StatusCode == 200 && response.Body != null)
                                {
                                    _getConfigTask.GetConfig(appIDs, serviceConfig, cluster);

                                    var res = response.Body.FirstOrDefault();

                                    if (res != null)
                                    {
                                        _notificationId = res.NotificationId;
                                    }

                                    break;
                                }
                                else if (response.StatusCode == 304)
                                {
                                    break;
                                }
                                else
                                {
                                    throw new ApolloConfigException($"url:{url},response.StatusCode:{response.StatusCode}");
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        OnError?.Invoke(new ApolloConfigException("apollo轮询配置中心出现异常", ex));
                        Thread.Sleep(10 * 1000);
                    }
                });

                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        /// <summary>
        /// 停止长轮询
        /// </summary>
        public void Stop()
        {
            _start = false;
        }

    }
}
