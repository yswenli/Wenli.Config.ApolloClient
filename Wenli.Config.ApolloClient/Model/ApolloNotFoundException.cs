using System;

namespace Wenli.Config.ApolloClient.Model
{
    /// <summary>
    /// Apollo找不到对象异常
    /// </summary>
    public class ApolloNotFoundException : Exception
    {
        /// <summary>
        /// Apollo找不到对象异常
        /// </summary>
        /// <param name="message"></param>
        public ApolloNotFoundException(string message) : base(message) { }
        /// <summary>
        /// Apollo找不到对象异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public ApolloNotFoundException(string message, Exception ex) : base(message, ex) { }
    }
}
