using SmallSocket.SocketEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

        public async void Stop()
        {
            this._tcpListener.Stop();
            await _config.Dispatcher.OnListenerStoped(this);
        }

        private async void Listener()
        {
            try
            {
                this._tcpListener = new TcpListener(_config.ListenedEndPoint);
                this._tcpListener.AllowNatTraversal(_config.AllowNatTraversal);
                //this._tcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);//启用端口共享
                this._tcpListener.Start(_config.PendingConnectionBacklog);
                await _config.Dispatcher.OnListenerStarted(this);

                while (true)
                {
                    var tcpClient = await this._tcpListener.AcceptTcpClientAsync();//开始监听socket客户端

                    AppServer.GetAppServer().FetchClient(tcpClient);
                }
            }
            catch (Exception ex)
            {
                await _config.Dispatcher.OnListenerError(this, ex);
            }
        }

    }
}
