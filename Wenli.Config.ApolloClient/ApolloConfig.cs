/****************************************************************************
*项目名称：Wenli.Config.ApolloClient.Base
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：Wenli.Config.ApolloClient
*类 名 称：ApolloConfig
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
using System.IO;

namespace Wenli.Config.ApolloClient
{
    /// <summary>
    /// 驱动配置
    /// </summary>
    public class ApolloConfig
    {
        /// <summary>
        /// apollo配置中心服务器地址
        /// </summary>
        public string ServerUrl
        {
            get; set;
        }

        /// <summary>
        /// 使用环境
        /// </summary>
        public string Env
        {
            get; set;
        }

        /// <summary>
        /// 业务标识，逗号分隔
        /// </summary>
        public string AppIDs
        {
            get; set;
        }

        /// <summary>
        /// 缓存地址
        /// </summary>
        public string CachePath
        {
            get; set;
        } = Path.Combine(Directory.GetCurrentDirectory(), "ApolloCache");


        /// <summary>
        /// 操作超时时间
        /// </summary>
        public int Timeout
        {
            get; set;
        } = 60 * 1000;


        /// <summary>
        /// 读取配置超时时间
        /// </summary>
        public int ReadTimeout
        {
            get; set;
        } = 60 * 1000;

        /// <summary>
        /// 操作失败最大重试次数
        /// </summary>
        public int MaxRetries
        {
            get; set;
        } = 5;


        /// <summary>
        /// 获取配置字符串
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            return JSONHelper.SerializeObject(this);
        }

        /// <summary>
        /// 将字符串转换成ApolloConfig
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static ApolloConfig Parse(string json)
        {
            return JSONHelper.DeserializeObject<ApolloConfig>(json);
        }

        /// <summary>
        /// 驱动配置
        /// </summary>
        internal ApolloConfig()
        {

        }

    }
}
