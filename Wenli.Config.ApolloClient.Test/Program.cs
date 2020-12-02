using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wenli.Config.ApolloClient.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ApolloClient Test";

            Console.WriteLine($"{Console.Title} 回车开始...");
            Console.ReadLine();
            Console.WriteLine($"{Console.Title} 正在初始化...");

            var apolloConfig = new ApolloConfigBuilder()
                .SetApolloServerUrl(new Uri("http://test.ecm.em/"))
                .SetAppIDs("pointcpf_supplier")
                .Build();

            ApolloServicePool.OnError += ApolloServicePool_OnError;

            ApolloServicePool.Init(apolloConfig);

            Console.WriteLine($"{Console.Title} 已初始化，正在读取配置...");            

            var appIds = ApolloServicePool.GetAppIds(apolloConfig);

            while (true)
            {
                foreach (var appId in appIds)
                {
                    var keys = ApolloServicePool.GetConfigKeys(apolloConfig, appId);

                    foreach (var key in keys)
                    {
                        Console.WriteLine($"appId:{appId},key:{key} value:{ApolloServicePool.GetConfig(apolloConfig, appId, key)}");
                    }
                }

                //生成代码
                ApolloServicePool.GenerateClass(apolloConfig, "Wenli.Config.ApolloClient", @"D:\tools\test\ApolloConfig.cs", out string err);

                Console.WriteLine($"{Console.Title} 测试已完成");

                Console.ReadLine();
            }
        }

        private static void ApolloServicePool_OnError(Model.ApolloConfigException apolloConfigException)
        {
            Console.WriteLine("error:" +Wenli.Config.ApolloClient.Newtonsoft.Json.JsonConvert.SerializeObject(apolloConfigException));
        }
    }
}
