using SmallSocket.SocketEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SmallSocket.SocketEngine
{
    public class AppSession
    {
        public Guid Key { get; private set; }
        private readonly TcpClient _tcpClient = null;
        private readonly ServerConfiguration _config = null;
        public DateTime StartTime { get; private set; }

        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return (_tcpClient != null && _tcpClient.Client.Connected) ?
                    (IPEndPoint)_tcpClient.Client.RemoteEndPoint : null;
            }
        }
        public IPEndPoint LocalEndPoint
        {
            get
            {
                return (_tcpClient != null && _tcpClient.Client.Connected) ?
                    (IPEndPoint)_tcpClient.Client.LocalEndPoint : null;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">键key</param>
        /// <param name="client">客户端Client</param>
        internal AppSession(Guid key, TcpClient client, ServerConfiguration config)
        {
            this.Key = key;
            this._tcpClient = client;
            this._config = config;
            this.StartTime = DateTime.UtcNow;
        }

        public void Start()
        {
            this._config.Dispatcher.OnSessionStarted(this).Wait();

            Task.Run(async () =>
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int count = await this._tcpClient.GetStream().ReadAsync(buffer, 0, buffer.Length);
                    this._config.Dispatcher.OnSessionDataReceived(this, buffer, 0, count).Wait();
                }
            });
        }

        public async Task SendAsync(byte[] buffer, int offset, int count)
        {
            await this._tcpClient.GetStream().WriteAsync(buffer, offset, count);
        }

        public void Close()
        {
            this._tcpClient.Close();
            this._config.Dispatcher.OnSessionClosed(this).Wait();
        }
    }
}
