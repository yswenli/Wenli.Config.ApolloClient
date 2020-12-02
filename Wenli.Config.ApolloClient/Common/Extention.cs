using System.Collections.Generic;
using System.Globalization;
using System.Net;
using static System.Collections.Specialized.NameObjectCollectionBase;

namespace Wenli.Config.ApolloClient.Common
{
    /// <summary>
    /// 扩展信息
    /// </summary>
    public static class Extention
    {
        /// <summary>
        /// 获取httpresponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static WebResponse BetterGetResponse(this WebRequest request)
        {
            try
            {
                return request.GetResponse();
            }
            catch (WebException wex)
            {
                if (wex.Response == null || wex.Status != WebExceptionStatus.ProtocolError)
                    throw;

                return wex.Response;
            }
        }

        /// <summary>
        /// 转换成大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// 转换成大写并替换掉_
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTitleCaseWithOutSpecial(this string str)
        {
            str = str.Replace("_");

            return str.ToTitleCase();
        }
        /// <summary>
        /// 转换成大写并替换掉_
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ToTitleCaseWithOutSpecial(this object key)
        {
            var str = key.ToString().Replace("_");

            return str.ToTitleCase();
        }

        /// <summary>
        /// 特殊符号
        /// </summary>
        public static readonly string[] SpecialSts = new string[] { "~", "`", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "=", "+", "[", "]", ";", ":", "'", "\"", "\\", "|", ",", "<", ".", ">", "?", "/" };

       /// <summary>
       /// 替换方法
       /// </summary>
       /// <param name="str"></param>
       /// <param name="newStr"></param>
       /// <returns></returns>
        public static string Replace(this string str, string newStr)
        {
            foreach (var item in SpecialSts)
            {
                str = str.Replace(item, newStr);
            }

            return str;
        }

        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumberic(this string str)
        {
            if (long.TryParse(str, out long result))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 将集合转换成列表
        /// </summary>
        /// <param name="keysCollection"></param>
        /// <returns></returns>
        public static List<string> ToList(this KeysCollection keysCollection)
        {
            if (keysCollection != null)
            {
                List<string> list = new List<string>();

                foreach (var item in keysCollection)
                {
                    list.Add(item.ToString());
                }

                return list;
            }

            return null;
        }
    }
}
