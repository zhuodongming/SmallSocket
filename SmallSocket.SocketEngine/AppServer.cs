using SmallSocket.SocketEngine.Interfaces;
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
        private AppServer() { }

        private static readonly AppServer _appServer = new AppServer();
        public static AppServer GetAppServer()
        {
            return _appServer;
        }
        #endregion

        private readonly ConcurrentDictionary<Guid, AppSession> _appSessions = new ConcurrentDictionary<Guid, AppSession>();
        private long i = 0;
        private AppChildServer[] _appChildServers = null;
        private ServerConfiguration _config = null;
        private ISocketListener _listener = null;

        //启动服务
        public void Start(ServerConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config", "argument config is not null");
            }
            this._config = config;

            StartChildServers();
            StartListen();
        }

        //启动子服务
        private void StartChildServers()
        {
            this._appChildServers = new AppChildServer[10];
            for (int i = 0; i < _appChildServers.Length; i++)
            {
                this._appChildServers[i] = new AppChildServer(i, _config).Start();
            }
        }

        //启动监听
        private void StartListen()
        {
            if (_config.IsTCP == true)
            {
                this._listener = new TcpSocketListener(_config);
            }
            else
            {
                this._listener = new TcpSocketListener(_config);
            }
            this._listener.Start();
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        public void Close()
        {
            this._listener.Stop();//停止监听客户端
            var sessions = GetAllSession();//关闭所有客户端连接
            foreach (var session in sessions)
            {
                session.Close();
            }
        }

        /// <summary>
        /// 分发客户端
        /// </summary>
        /// <param name="client"></param>
        internal void FetchClient(TcpClient client)
        {
            this._appChildServers[this.i % _appChildServers.Length].Add(client);
            this.i++;
        }

        /// <summary>
        /// 注册会话
        /// </summary>
        /// <param name="session">会话</param>
        internal void RegisterSession(AppSession session)
        {
            this._appSessions.TryAdd(session.Key, session);
        }

        /// <summary>
        /// 移除会话：根据key
        /// </summary>
        /// <param name="session">键</param>
        public void RemoveSession(Guid key)
        {
            AppSession session = null;
            this._appSessions.TryRemove(key, out session);
        }

        /// <summary>
        /// 移除会话：根据session
        /// </summary>
        /// <param name="session">会话</param>
        public void RemoveSession(AppSession session)
        {
            this.RemoveSession(session.Key);
        }

        /// <summary>
        /// 获取会话数量
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return this._appSessions.Count;
        }

        /// <summary>
        /// 根据key获取会话
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public AppSession GetSession(Guid key)
        {
            AppSession session = null;
            this._appSessions.TryGetValue(key, out session);
            return session;
        }

        /// <summary>
        /// 获取所有会话
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AppSession> GetAllSession()
        {
            return this._appSessions.Values;
        }

    }
}

