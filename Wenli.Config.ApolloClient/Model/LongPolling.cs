using System.Collections.Generic;

namespace Wenli.Config.ApolloClient.Model
{
    /// <summary>
    /// 长轮询实体类
    /// </summary>
    public class LongPolling
    {
        private string namespaceName;

        private long notificationId;

        /// <summary>
        /// 长轮询实体类
        /// </summary>
        public LongPolling()
        {
        }
        /// <summary>
        /// 长轮询实体类
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <param name="notificationId"></param>
        public LongPolling(string namespaceName, long notificationId)
        {
            this.namespaceName = namespaceName;
            this.notificationId = notificationId;
        }
        /// <summary>
        /// 命名空间名称
        /// </summary>
        public string NamespaceName
        {
            get
            {
                return namespaceName;
            }
            set
            {
                this.namespaceName = value;
            }
        }
        /// <summary>
        /// 通知id
        /// </summary>
        public long NotificationId
        {
            get
            {
                return notificationId;
            }
            set
            {
                this.notificationId = value;
            }
        }
        /// <summary>
        /// 合并成格式字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ApolloConfigNotification{" + "namespaceName='" + namespaceName + '\'' + ", notificationId=" + notificationId + '}';
        }
    }
    /// <summary>
    /// 长轮询集合实体类
    /// </summary>
    public class LongPollings : List<LongPolling>
    {
    }
}
