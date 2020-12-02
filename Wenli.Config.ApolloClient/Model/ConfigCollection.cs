using Wenli.Config.ApolloClient.Common;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace Wenli.Config.ApolloClient.Model
{
    /// <summary>
    /// apollo配置集合类
    /// </summary>
    public static class ConfigCollection
    {
        static ConcurrentDictionary<string, RemoteConfig> _cache = new ConcurrentDictionary<string, RemoteConfig>();

        /// <summary>
        /// 从本地文件加载Apollo配置
        /// </summary>
        /// <param name="cachePath"></param>
        public static void Init(string cachePath)
        {
            var collection = FileHelper.ReadCollection<RemoteConfig>(cachePath);

            if (collection != null && collection.Any())
            {
                foreach (var item in collection)
                {
                    _cache.TryAdd(item.Key, item.Value);
                }
            }
        }
        /// <summary>
        /// 获取key
        /// </summary>
        /// <param name="env"></param>
        /// <param name="cluster"></param>
        /// <param name="appID"></param>
        /// <returns></returns>
        static string GetKey(string env, string cluster, string appID)
        {
            return $"{env}_{cluster}_{appID}";
        }
        /// <summary>
        /// 设置apollo服务配置
        /// </summary>
        /// <param name="env"></param>
        /// <param name="remoteConfig"></param>
        public static void Set(string env, RemoteConfig remoteConfig)
        {
            var dKey = GetKey(env, remoteConfig.Cluster, remoteConfig.AppId);

            _cache.AddOrUpdate(dKey, remoteConfig, (k, v) => { return remoteConfig; });
        }
        /// <summary>
        /// 获取apollo服务配置
        /// </summary>
        /// <param name="env"></param>
        /// <param name="cluster"></param>
        /// <param name="appID"></param>
        /// <returns></returns>
        public static RemoteConfig Get(string env, string cluster, string appID)
        {
            var dKey = GetKey(env, cluster, appID);

            while (!_cache.Any())
            {
                Thread.Sleep(1000);
            }

            if (_cache.TryGetValue(dKey, out RemoteConfig remoteConfig))
            {
                return remoteConfig;
            }
            return null;
        }
        /// <summary>
        /// 移除apollo服务配置
        /// </summary>
        /// <param name="env"></param>
        /// <param name="cluster"></param>
        /// <param name="appID"></param>
        public static void Remove(string env, string cluster, string appID)
        {
            var dKey = GetKey(env, cluster, appID);
            _cache.TryRemove(dKey, out RemoteConfig _);
        }

    }
}
