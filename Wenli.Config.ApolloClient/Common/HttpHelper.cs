
using Wenli.Config.ApolloClient.Model;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Wenli.Config.ApolloClient.Common
{
    /// <summary>
    /// http请求处理类
    /// </summary>
    public class HttpHelper
    {
        string _basicAuth;

        int _timeOut = 60 * 1000;

        int _readTimeOut = 60 * 1000;

        /// <summary>
        /// http请求处理类
        /// </summary>
        /// <param name="timeOut"></param>
        /// <param name="readTimeOut"></param>
        public HttpHelper(int timeOut, int readTimeOut)
        {
            _basicAuth = "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("user:"));

            _timeOut = timeOut;

            _readTimeOut = readTimeOut;
        }

        /// <summary>
        /// 处理get请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public HttpResponse<T> DoGet<T>(HttpRequest httpRequest)
        {
            int statusCode;
            try
            {
                //Console.WriteLine($"Wenli.Config.ApolloClient 正在开始请求{httpRequest.Url} ...");

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(httpRequest.Url);
                req.Method = "GET";
                req.Headers["Authorization"] = _basicAuth;

                int timeout = httpRequest.Timeout;
                if (timeout <= 0)
                {
                    timeout = _timeOut;
                }

                int readTimeout = httpRequest.ReadTimeout;
                if (readTimeout <= 0 && readTimeout != Timeout.Infinite)
                {
                    readTimeout = _readTimeOut;
                }

                req.Timeout = timeout;
                req.ReadWriteTimeout = readTimeout;

                using (HttpWebResponse res = (HttpWebResponse)req.BetterGetResponse())
                {
                    statusCode = (int)res.StatusCode;

                    //Console.WriteLine($"Wenli.Config.ApolloClient 请求{httpRequest.Url} 已完成，StatusCode:{statusCode}");

                    if (statusCode == 200)
                    {
                        using (var stream = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
                        {
                            var content = stream.ReadToEnd();
                            T body = JSONHelper.DeserializeObject<T>(content);
                            return new HttpResponse<T>(statusCode, body);
                        }
                    }

                    if (statusCode == 304 || statusCode == 404)
                    {
                        return new HttpResponse<T>(statusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Wenli.Config.ApolloClient Err:{ex.Message} type:{ex.GetType().Name}");

                throw new ApolloConfigException("Wenli.Config.ApolloClient Could not complete get operation,url:" + httpRequest.Url, ex);
            }

            throw new ApolloConfigStatusCodeException(statusCode, string.Format("Wenli.Config.ApolloClient Get operation failed for {0}", httpRequest.Url));
        }
    }


    /// <summary>
    /// http的回复模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpResponse<T>
    {
        /// <summary>
        /// http的回复模型
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="body"></param>
        public HttpResponse(int statusCode, T body)
        {
            StatusCode = statusCode;
            Body = body;
        }
        /// <summary>
        /// http的回复模型
        /// </summary>
        /// <param name="statusCode"></param>
        public HttpResponse(int statusCode)
        {
            StatusCode = statusCode;
            Body = default(T);
        }
        /// <summary>
        /// http状态码
        /// </summary>
        public int StatusCode
        {
            get;
            private set;

        }
        /// <summary>
        /// http的数据实体
        /// </summary>
        public T Body
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// http的请求模型
    /// </summary>
    public class HttpRequest
    {
        private string m_url;
        private int m_timeout;
        private int m_readTimeout;
        /// <summary>
        /// http的请求模型
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeOut"></param>
        public HttpRequest(string url, int timeOut = 60 * 1000)
        {
            m_url = url;
            m_timeout = timeOut;
            m_readTimeout = 0;
        }
        /// <summary>
        /// 请求的url
        /// </summary>
        public string Url
        {
            get
            {
                return m_url;
            }
        }
        /// <summary>
        /// 请求超时时间
        /// </summary>
        public int Timeout
        {
            get
            {
                return m_timeout;
            }
            set
            {
                this.m_timeout = value;
            }
        }

        /// <summary>
        /// 读取超时时间
        /// </summary>
        public int ReadTimeout
        {
            get
            {
                return m_readTimeout;
            }
            set
            {
                this.m_readTimeout = value;
            }
        }

    }
}
