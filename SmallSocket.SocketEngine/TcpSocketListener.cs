using SmallSocket.SocketEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SmallSocket.SocketEngine
{
    internal sealed class TcpSocketListener : ISocketListener
    {
        private TcpListener _tcpListener = null;
        private ServerConfiguration _config = null;

        public TcpSocketListener(ServerConfiguration config)
        {
            this._config = config;
        }

        public void Start()
        {
            Listener();
        }

        public void Stop()
        {
            this._tcpListener.Stop();
        }

        private async void Listener()
        {
            try
            {
                this._tcpListener = new TcpListener(_config.ListenedEndPoint);
                this._tcpListener.AllowNatTraversal(_config.AllowNatTraversal);
                this._tcpListener.Start(_config.PendingConnectionBacklog);
                await _config.Dispatcher.OnListenerStarted(this);

                while (AppServer.Instance.IsListening)
                {
                    var tcpClient = await this._tcpListener.AcceptTcpClientAsync();//开始监听socket客户端

                    AppServer.Instance.FetchClient(tcpClient);
                }
            }
            catch (Exception ex)
            {
                await _config.Dispatcher.OnListenerError(this, ex);
            }
            await _config.Dispatcher.OnListenerStoped(this);
        }

    }
}
