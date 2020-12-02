using System;

namespace Wenli.Config.ApolloClient.Model
{
    /// <summary>
    /// Apollo异常
    /// </summary>
    public class ApolloConfigException : Exception
    {
        /// <summary>
        /// Apollo异常
        /// </summary>
        /// <param name="message"></param>
        public ApolloConfigException(string message) : base(message) { }
        /// <summary>
        /// Apollo异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public ApolloConfigException(string message, Exception ex) : base(message, ex) { }
    }
}
