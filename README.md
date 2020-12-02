# Wenli.Config.ApolloClient

[![NuGet version (Wenli.Data.Es)](https://img.shields.io/nuget/v/Wenli.Data.Es.svg?style=flat-square)](https://www.nuget.org/packages?q=Wenli.Config.ApolloClient)
[![License](https://img.shields.io/badge/license-Apache%202-4EB1BA.svg)](https://www.apache.org/licenses/LICENSE-2.0.html)


Wenli.Config.ApolloClient 是根据了解官方驱动流程后，结合项目实际情况重构的apollo配置中心客户端，主要解决以下几个问题：

 1.无需在c盘配置目录 

 2.可以连接多个apollo服务中心、每一个中心可以使用多个appid的配置 

 3.不再使用onchange事件来通知更新，而是增加实体类生成工具，用类的属性来实现自动更新，以方便于项目中使用 

 4.增加更加详细的配置，以适应不同项目环境的需求，比如长轮询时间、次数等 5.根据实际情况修改配置读取顺序为：优先使用appsetting中的配置、其次使用apolloclient本地配置，最后使用apolloservice的配置

测试代码如下
```CSharp
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
                .SetEnv("UAT")
                .SetApolloServerUrl(new Uri("http://127.0.0.1:8080/"))
                .SetAppIDs("myfirstweb")
                .Build();

            ApolloServicePool.OnError += ApolloServicePool_OnError;

            ApolloServicePool.Init(apolloConfig);

            Console.WriteLine($"{Console.Title} 已初始化，正在读取配置...");

            var appIds = ApolloServicePool.GetAppIds(apolloConfig);

            Console.WriteLine("appIds:" + appIds?.Length);

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
            Console.WriteLine("error:" + Wenli.Config.ApolloClient.Newtonsoft.Json.JsonConvert.SerializeObject(apolloConfigException));
        }
    }
}

```