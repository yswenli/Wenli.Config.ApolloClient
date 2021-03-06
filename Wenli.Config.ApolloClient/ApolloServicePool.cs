﻿/****************************************************************************
*项目名称：Wenli.Config.ApolloClient.Base
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：Wenli.Config.ApolloClient
*类 名 称：ApolloServicePool
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
using System;
using System.Collections.Concurrent;
using System.Linq;
using Wenli.Config.ApolloClient.Core;
using Wenli.Config.ApolloClient.Model;

namespace Wenli.Config.ApolloClient
{
    /// <summary>
    /// ApolloService缓存
    /// </summary>
    public sealed class ApolloServicePool
    {
        ConcurrentDictionary<string, ApolloService> _cache;

        /// <summary>
        /// apollo组件异常事件通知
        /// </summary>
        public event OnErrorHandler OnError;
        

        /// <summary>
        /// ApolloService缓存
        /// </summary>
        /// <param name="apolloConfigs"></param>
        internal ApolloServicePool(params ApolloConfig[] apolloConfigs)
        {
            try
            {
                if (apolloConfigs == null) throw new Exception("传入的apolloConfigs 不能为空!");

                _cache = new ConcurrentDictionary<string, ApolloService>();

                foreach (var apolloConfig in apolloConfigs)
                {
                    var uniqueId = apolloConfig.ServerUrl + apolloConfig.AppIDs;

                    if (!_cache.ContainsKey(uniqueId))
                    {
                        var service = new ApolloService(apolloConfig);
                        service.OnError += Service_OnError;
                        service.Start();
                        _cache.TryAdd(uniqueId, service);
                    }
                }

                
            }
            catch (Exception ex)
            {
                throw new ApolloConfigException($"ApolloServicePool 初始化失败,ex:{ex.Message}");
            }
        }

        #region static

        static ApolloServicePool _apolloServicePool = null;

        static object _locker = new object();

        /// <summary>
        /// 创建单例ApolloServicePool实例
        /// </summary>
        /// <param name="apolloConfigs"></param>
        /// <returns></returns>
        public static ApolloServicePool Create(params ApolloConfig[] apolloConfigs)
        {
            if (_apolloServicePool == null)
            {
                lock (_locker)
                {
                    if (_apolloServicePool == null)
                    {
                        _apolloServicePool = new ApolloServicePool(apolloConfigs);
                    }
                }
            }

            return _apolloServicePool;
        }


        /// <summary>
        /// 根据配置生成类文件
        /// </summary>
        /// <param name="apolloConfig">配置</param>
        /// <param name="nameSpace">ApolloService实例化空间名称</param>
        /// <param name="filePath">代码生成目录</param>
        /// <param name="err">代码生成时异常消息</param>
        /// <returns>成功/失败y</returns>
        public static bool GenerateClass(ApolloConfig apolloConfig, string nameSpace, string filePath, out string err)
        {
            return ApolloService.GenerateClass(apolloConfig, nameSpace, filePath, out err);
        }

        #endregion




        /// <summary>
        /// 获取AppIds
        /// </summary>
        /// <param name="apolloConfig"></param>
        /// <returns></returns>
        public string[] GetAppIds(ApolloConfig apolloConfig)
        {
            return _cache[apolloConfig.ServerUrl + apolloConfig.AppIDs].GetAppIds();
        }

        /// <summary>
        /// 获取配置中的全部keys
        /// </summary>
        /// <param name="apolloConfig"></param>
        /// <param name="appID"></param>
        /// <returns></returns>
        public string[] GetConfigKeys(ApolloConfig apolloConfig, string appID)
        {
            return _cache[apolloConfig.ServerUrl + apolloConfig.AppIDs][appID].Keys.ToArray();
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <param name="appid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfig(string uniqueId, string appid, string key)
        {
            if (_cache[uniqueId][appid].TryGetValue(key, out string value))
            {
                return value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="apolloConfig"></param>
        /// <param name="appid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfig(ApolloConfig apolloConfig, string appid, string key)
        {
            return GetConfig(apolloConfig.ServerUrl + apolloConfig.AppIDs, appid, key);
        }



        /// <summary>
        /// 获取配置
        /// 优先本地AppSetting配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apolloConfig"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetConfig<T>(string uniqueId, string appid, string key)
        {
            string val;

            _cache[uniqueId][appid].TryGetValue(key, out val);

            if (!string.IsNullOrEmpty(val))
            {
                return ConvertValue<T>(val);
            }

            return default(T);
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apolloConfig"></param>
        /// <param name="appid"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetConfig<T>(ApolloConfig apolloConfig, string appid, string key, T defaultValue)
        {
            var val = GetConfig(apolloConfig, appid, key);

            if (!string.IsNullOrEmpty(val))
            {
                return ConvertValue<T>(val);
            }

            return defaultValue;
        }


        private void Service_OnError(ApolloConfigException apolloConfigException)
        {
            OnError?.Invoke(apolloConfigException);
        }


        private T ConvertValue<T>(string val)
        {
            try
            {
                if (typeof(T) == typeof(bool))
                {
                    var lv = val.ToLower();
                    val = lv == "1" || lv == "true" ? "true" : "false";
                }
                return (T)Convert.ChangeType(val, typeof(T));
            }
            catch (Exception ex)
            {
                OnError?.Invoke(new ApolloConfigException("ApolloServicePool.GetConfig T类型转换失败，val:" + val, ex));
            }
            return default(T);
        }
    }
}
