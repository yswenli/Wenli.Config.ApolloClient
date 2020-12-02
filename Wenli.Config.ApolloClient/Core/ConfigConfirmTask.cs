/****************************************************************************
*项目名称：Wenli.Config.ApolloClient.Base
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：Wenli.Config.ApolloClient.Base
*类 名 称：ConfigConfirmTask
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/1/8 15:12:52
*描述：
*=====================================================================
*修改时间：2019/1/8 15:12:52
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using Wenli.Config.ApolloClient.Common;
using Wenli.Config.ApolloClient.Model;
using System.Linq;

namespace Wenli.Config.ApolloClient.Core
{
    /// <summary>
    /// 配置变动确认任务
    /// </summary>
    class ConfigConfirmTask
    {
        HttpHelper _httpHelper;

        ApolloConfig _apolloConfig;

        /// <summary>
        /// 配置变动确认任务
        /// </summary>
        /// <param name="apolloConfig"></param>
        public ConfigConfirmTask(ApolloConfig apolloConfig)
        {
            _apolloConfig = apolloConfig;

            _httpHelper = new HttpHelper(_apolloConfig.Timeout, _apolloConfig.ReadTimeout);
        }
        /// <summary>
        /// 获取服务集合
        /// </summary>
        /// <returns></returns>
        public ServiceConfigs GetServices()
        {
            var url = TaskUrlHelper.GetServiceUrl(_apolloConfig.ServerUrl);

            for (int i = 0; i < _apolloConfig.MaxRetries; i++)
            {
                var services = _httpHelper.DoGet<ServiceConfigs>(new HttpRequest(url)).Body;

                if (services != null && services.Any())
                {
                    return services;
                }

                if (i == _apolloConfig.MaxRetries - 1)
                {
                    throw new ApolloConfigException("ConfigConfirmTask.GetServices 已超出重试次数！");
                }
            }

            return null;
        }

    }
}
