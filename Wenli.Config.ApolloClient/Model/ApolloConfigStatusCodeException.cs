namespace Wenli.Config.ApolloClient.Model
{
    /// <summary>
    /// Apollo状态异常
    /// </summary>
    public class ApolloConfigStatusCodeException : ApolloConfigException
    {
        private readonly int m_statusCode;

        /// <summary>
        /// Apollo状态异常
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        public ApolloConfigStatusCodeException(int statusCode, string message)
            : base(string.Format("[status code: {0:D}] {1}", statusCode, message))
        {
            this.m_statusCode = statusCode;
        }
        /// <summary>
        /// 状态码
        /// </summary>
        public virtual int StatusCode
        {
            get
            {
                return m_statusCode;
            }
        }
    }
}
