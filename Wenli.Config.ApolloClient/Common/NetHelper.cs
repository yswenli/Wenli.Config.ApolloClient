using System.Net;
using System.Web;

namespace Wenli.Config.ApolloClient.Common
{
    /// <summary>
    /// 网络信息工具类
    /// </summary>
    public static class NetHelper
    {
        /// <summary>
        /// 获取本地ip
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
        {
            try
            {
                var hostName = Dns.GetHostName();

                IPHostEntry ipHostEntry = Dns.GetHostEntry(hostName);

                return GetIP(ipHostEntry);
            }
            catch { }
            return string.Empty;
        }

        /// <summary>
        /// 互取网络ip
        /// </summary>
        /// <param name="ipHostEntry"></param>
        /// <returns></returns>
        private static string GetIP(IPHostEntry ipHostEntry)
        {
            foreach (IPAddress ip in ipHostEntry.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return ipHostEntry.AddressList[0].ToString();
        }
    }
}
