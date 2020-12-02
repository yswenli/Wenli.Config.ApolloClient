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

            var apolloConfig = new ApolloConfigBuilder()
                .SetApolloServerUrl(new Uri("http://127.0.0.1:8080/"))
                .SetAppIDs("XC,WX,QQ")
                .Build();

            ApolloServicePool.OnError += ApolloServicePool_OnError;

            ApolloServicePool.Init(apolloConfig);

            while (true)
            {
                Console.ReadLine();

                foreach (var appId in appIds)
                {
                    var keys = ApolloServicePool.GetConfigKeys(sectionName, appId);

                    foreach (var key in keys)
                    {
                        Console.WriteLine($"sectionName:{sectionName},appId:{appId} value:{ApolloServicePool.GetConfig(sectionName, appId, key)}");
                    }
                }
            }
        }

        private static void ApolloServicePool_OnError(Model.ApolloConfigException apolloConfigException)
        {
            Console.WriteLine("error:" + apolloConfigException.Message);
        }
    }
}
