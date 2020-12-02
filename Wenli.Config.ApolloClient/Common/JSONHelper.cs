
using Wenli.Config.ApolloClient.Newtonsoft.Json;
using System;
using System.Text;

namespace Wenli.Config.ApolloClient.Common
{
    /// <summary>
    /// json处理工具类
    /// </summary>
    public class JSONHelper
    {
        private static JsonSerializerSettings settings;

        /// <summary>
        /// json处理工具类
        /// </summary>
        static JSONHelper()
        {
            settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            //settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, settings);
        }
        /// <summary>
        /// 反序列化json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(String json)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
        /// <summary>
        /// 反序列化json数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(byte[] json)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(json), settings);
        }
        /// <summary>
        /// 反序列化json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeserializeObject(byte[] json, Type type)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(json), type, settings);
        }
    }
}
