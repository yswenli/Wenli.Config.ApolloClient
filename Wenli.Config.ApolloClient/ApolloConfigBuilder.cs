/****************************************************************************
*项目名称：EMSupplier.Core.SmallLoan
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：Wenli.Config.Wenli.Config.ApolloClient
*类 名 称：ApolloConfigBuilder
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/11/4 15:30:33
*描述：
*=====================================================================
*修改时间：2020/11/4 15:30:33
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using Wenli.Config.ApolloClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Wenli.Config.ApolloClient
{
    /// <summary>
    /// 驱动配置
    /// </summary>
    public class ApolloConfigBuilder
    {

        ApolloConfig _apolloConfig;

        /// <summary>
        /// 驱动配置
        /// </summary>
        public ApolloConfigBuilder()
        {
            _apolloConfig = new ApolloConfig();
        }

        /// <summary>
        /// apollo配置中心服务器地址
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public ApolloConfigBuilder SetApolloServerUrl(Uri uri)
        {
            _apolloConfig.ServerUrl = uri.ToString();
            return this;
        }

        /// <summary>
        /// 使用环境
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public ApolloConfigBuilder SetEnv(string env)
        {
            _apolloConfig.Env = env;
            return this;
        }

        /// <summary>
        /// 业务标识，逗号分隔
        /// </summary>
        /// <param name="appIds"></param>
        /// <returns></returns>
        public ApolloConfigBuilder SetAppIDs(string appIds)
        {
            _apolloConfig.AppIDs = appIds;
            return this;
        }

        /// <summary>
        /// 缓存地址
        /// </summary>
        /// <param name="cachePath"></param>
        /// <returns></returns>
        public ApolloConfigBuilder SetCachePath(string cachePath)
        {
            if (string.IsNullOrEmpty(cachePath))
            {
                cachePath = Path.Combine(Directory.GetCurrentDirectory(), "ApolloCache");
            }
            _apolloConfig.CachePath = cachePath;
            return this;
        }

        /// <summary>
        /// 操作超时时间
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public ApolloConfigBuilder Timeout(int timeout = 60 * 1000)
        {
            _apolloConfig.Timeout = timeout;
            return this;
        }

        /// <summary>
        /// 读取配置超时时间
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public ApolloConfigBuilder ReadTimeout(int timeout = 60 * 1000)
        {
            _apolloConfig.ReadTimeout = timeout;
            return this;
        }

        /// <summary>
        /// 操作失败最大重试次数
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public ApolloConfigBuilder MaxRetries(int times = 5)
        {
            _apolloConfig.MaxRetries = times;
            return this;
        }

        /// <summary>
        /// 返回配置
        /// </summary>
        /// <returns></returns>
        public ApolloConfig Build()
        {
            return _apolloConfig;
        }
    }
}
