using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SmallSocket.SocketEngine
{
    public class TcpSocketListener : ISocketListener
    {
        /// <summary>
        /// TCP监听器
        /// </summary>
        private TcpListener tcpListener;

        public TcpSocketListener(IPEndPoint ipEndPoint)
        {
            tcpListener = new TcpListener(ipEndPoint);
            //tcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);//启用端口共享
        }

        public void Start()
        {
            Listener();
        }

        public void Stop()
        {
            tcpListener.Stop();
        }

        private async void Listener()
        {
            tcpListener.Start();
            while (true)
            {
                TcpClient tcpClient = null;
                try
                {
                    tcpClient = await tcpListener.AcceptTcpClientAsync();//开始监听socket客户端
                }
                catch
                {
                    throw;
                }
                AppServer.GetAppServer().FetchClient(tcpClient);
            }
        }
    }
}
