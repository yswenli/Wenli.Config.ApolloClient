/*此为ApolloService.GenerateClass方法生成代码，请勿手动修改*/
using System;
using Wenli.Config.ApolloClient;
namespace Wenli.Config.ApolloClient
{
    /// <summary>
    /// ApolloServicePointcpf_Supplier ,
    /// AppID:pointcpf_supplier
    /// </summary>
    public static class ApolloServicePointcpf_Supplier
    {

        static ApolloServicePool _apolloServicePool;

        static ApolloServicePointcpf_Supplier()
        {
            var config = new ApolloConfigBuilder()
                .SetApolloServerUrl(new Uri("http://127.0.0.1"))
                .SetAppIDs("test")
                .SetEnv("UAT")
                .Build();

            _apolloServicePool = ApolloServicePool.Create(config);

            _apolloServicePool.OnError += _apolloServicePool_OnError;
        }

        private static void _apolloServicePool_OnError(Model.ApolloConfigException apolloConfigException)
        {
            Console.WriteLine("_apolloServicePool_OnError:" + apolloConfigException.Message);
        }

        /// <summary>
        /// Key:SmallLoanUrl
        /// </summary>
        public static string Api
        {
            get
            {
                return _apolloServicePool.GetConfig("http://127.0.0.1test", "test", "Api");
            }
        }
    }



}
