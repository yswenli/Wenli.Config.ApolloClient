/****************************************************************************
*项目名称：Wenli.Config.ApolloClient.Base
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：Wenli.Config.ApolloClient.Core
*类 名 称：ApolloService
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wenli.Config.ApolloClient.Core
{
    /// <summary>
    /// apollo服务
    /// </summary>
    internal class ApolloService
    {
        /// <summary>
        /// apollo组个把异常事件
        /// </summary>
        public event OnErrorHandler OnError;

        /// <summary>
        /// apollo配置
        /// </summary>
        public ApolloConfig Config
        {
            get; set;
        }

        ConfigConfirmTask _configConfirmTask;

        ApolloServiceConfigTask _apolloServiceConfigTask;

        LongPollingTask _longPollingTask;

        object _locker = new object();

        string[] _appIDs = null;

        string _cluster = "default";

        /// <summary>
        /// 使用指定的配置
        /// </summary>
        /// <param name="apolloConfig"></param>
        public ApolloService(ApolloConfig apolloConfig)
        {
            _appIDs = apolloConfig.AppIDs.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);

            if (_appIDs == null || _appIDs.Length < 1) throw new ApolloConfigException("ApolloConfig配置中AppIDs的内容或格式不正确");

            Config = apolloConfig;

            try
            {
                ConfigCollection.Init(Config.CachePath);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(new ApolloConfigException("Wenli.Config.ApolloClient 初始化过程中出现异常！", ex));
            }
        }


        /// <summary>
        /// 获取AppIds
        /// </summary>
        /// <returns></returns>
        public string[] GetAppIds()
        {
            return _appIDs;
        }


        /// <summary>
        /// 配置集合
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="cluster"></param>
        /// <returns></returns>
        public Dictionary<string, string> this[string appID, string cluster]
        {
            get
            {
                lock (_locker)
                {
                    var data = ConfigCollection.Get(Config.Env, cluster, appID);

                    if (data != null && data.Configurations != null && data.Configurations.Any())
                    {
                        return data.Configurations;
                    }
                    return null;
                }

            }
        }

        /// <summary>
        /// 配置集合
        /// </summary>
        /// <param name="appID"></param>
        /// <returns></returns>
        public Dictionary<string, string> this[string appID]
        {
            get
            {
                return this[appID, _cluster];
            }
        }


        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            try
            {
                if (_longPollingTask == null)
                {
                    _configConfirmTask = new ConfigConfirmTask(Config);

                    var serviceConfigs = _configConfirmTask.GetServices();

                    if (serviceConfigs != null && serviceConfigs.Any())
                    {
                        _apolloServiceConfigTask = new ApolloServiceConfigTask(Config);

                        foreach (var serviceConfig in serviceConfigs)
                        {
                            _apolloServiceConfigTask.GetConfig(_appIDs, serviceConfig, out _cluster);
                        }
                    }
                    else
                    {
                        throw new Exception("Wenli.Config.ApolloClient _configConfirmTask.GetServices 获取配置服务 is Null or Empty!");
                    }

                    _longPollingTask = new LongPollingTask(Config, serviceConfigs);

                    _longPollingTask.Start(_appIDs, _cluster);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(new ApolloConfigException("Wenli.Config.ApolloClient 长轮询过程中出现异常,正在等待重试！", ex));

                Thread.Sleep(10 * 1000);

                Start();
            }
        }

        /// <summary>
        /// 结束
        /// </summary>
        public void Stop()
        {
            _longPollingTask.Stop();
        }

        #region 代码生成工具类

        static string GetSpace(int num = 1)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append("    ");
            }
            return sb.ToString();
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
            var result = false;
            err = string.Empty;
            try
            {
                var apolloService = new ApolloService(apolloConfig);

                var appIDs = apolloConfig.AppIDs.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("/*此为ApolloService.GenerateClass方法生成代码，请勿手动修改*/");
                sb.AppendLine("using System;");
                sb.AppendLine("using Wenli.Config.ApolloClient;");
                sb.AppendLine("namespace " + nameSpace);
                sb.AppendLine("{");

                foreach (var appID in appIDs)
                {
                    sb.AppendLine($"{GetSpace(1)}/// <summary>");
                    sb.AppendLine($"{GetSpace(1)}/// ApolloService{appID.ToTitleCaseWithOutSpecial()} ,");
                    sb.AppendLine($"{GetSpace(1)}/// AppID:{appID}");
                    sb.AppendLine($"{GetSpace(1)}/// </summary>");
                    sb.AppendLine($"{GetSpace(1)}public static class ApolloService{appID.ToTitleCaseWithOutSpecial()}");
                    sb.AppendLine($"{GetSpace(1)}{{");

                    var data = apolloService[appID];

                    if (data != null)
                    {
                        foreach (var item in apolloService[appID].Keys)
                        {
                            sb.AppendLine($"{GetSpace(2)}/// <summary>");
                            sb.AppendLine($"{GetSpace(2)}/// Key:{item}");
                            sb.AppendLine($"{GetSpace(2)}/// </summary>");

                            var pName = item.ToTitleCaseWithOutSpecial();

                            if (pName.IsNumberic())
                            {
                                pName = "P_" + pName;
                            }
                            sb.AppendLine($"{GetSpace(2)}public static string {pName}");
                            sb.AppendLine($"{GetSpace(2)}{{");
                            sb.AppendLine($"{GetSpace(3)}get");
                            sb.AppendLine($"{GetSpace(3)}{{");
                            sb.AppendLine($"{GetSpace(4)}return ApolloServicePool.GetConfig(\"{apolloConfig.ServerUrl + apolloConfig.AppIDs}\",\"{appID}\",\"{item}\");");
                            sb.AppendLine($"{GetSpace(3)}}}");
                            sb.AppendLine($"{GetSpace(2)}}}");
                        }
                    }

                    sb.AppendLine($"{GetSpace(1)}}}");
                    sb.AppendLine("\r\n\r\n");
                }

                sb.AppendLine("}");

                var content = sb.ToString();

                FileHelper.Write(filePath, content);

                result = true;
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return result;
        }

        #endregion

    }
}
