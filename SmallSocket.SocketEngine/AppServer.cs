using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SmallSocket.SocketEngine
{
    /// <summary>
    /// sealed密封类防止继承破坏封装性
    /// </summary>
    public sealed class AppServer
    {
        #region 单例设计模式
        private AppServer()
        {

        }

        private static readonly AppServer appServer = new AppServer();

        public static AppServer GetAppServer()
        {
            return appServer;
        }
        #endregion

        private long i = 0;

        private AppChildServer[] servers = null;

        public ISocketListener Listener { get; set; }

        /// <summary>
        /// appSession会话集合
        /// </summary>
        private readonly ConcurrentDictionary<Guid, AppSession> appSessions = new ConcurrentDictionary<Guid, AppSession>();

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="listener"></param>
        public void Start(ISocketListener listener)
        {
            servers = new AppChildServer[100];
            for (int i = 0; i < servers.Length; i++)
            {
                servers[i] = new AppChildServer(i);
            }
            foreach (var item in servers)
            {
                item.Start();
            }

            Listener = listener;
            Listener.Start();
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        public void Close()
        {
            Listener.Stop();//停止监听客户端
            var list = GetAllSession();//关闭所有客户端连接
            foreach (var item in list)
            {
                item.Close();
            }
        }

        /// <summary>
        /// 分发客户端
        /// </summary>
        /// <param name="client"></param>
        public void FetchClient(TcpClient client)
        {
            servers[i % servers.Length].Add(client);
            i++;
        }

        /// <summary>
        /// 注册会话
        /// </summary>
        /// <param name="session">会话</param>
        public void RegisterSession(AppSession session)
        {
            appSessions.TryAdd(session.Key, session);
        }

        /// <summary>
        /// 移除会话
        /// </summary>
        /// <param name="session">键</param>
        public void RemoveSession(Guid key)
        {
            AppSession session = null;
            appSessions.TryRemove(key, out session);
        }

        /// <summary>
        /// 获取会话数量
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return appSessions.Count;
        }

        /// <summary>
        /// 根据key获取会话
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public AppSession GetSession(Guid key)
        {
            AppSession session = null;
            if (appSessions.TryGetValue(key, out session))
            {
                return session;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取所有会话
        /// </summary>
        /// <returns></returns>
        public List<AppSession> GetAllSession()
        {
            return appSessions.Values.ToList();
        }

    }
}

