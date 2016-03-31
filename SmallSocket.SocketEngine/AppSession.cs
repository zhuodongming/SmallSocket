using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SmallSocket.SocketEngine
{
    public class AppSession
    {
        /// <summary>
        /// key
        /// </summary>
        public Guid Key { get; private set; }

        /// <summary>
        /// 客户端
        /// </summary>
        public TcpClient Client { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">键key</param>
        /// <param name="client">客户端Client</param>
        public AppSession(Guid key, TcpClient client)
        {
            Key = key;
            Client = client;
        }
    }
}
