using SmallSocket.SocketEngine.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SmallSocket.SocketEngine
{
    internal class AppChildServer
    {
        private CancellationTokenSource cts = new CancellationTokenSource();

        private ConcurrentQueue<TcpClient> queue = new ConcurrentQueue<TcpClient>();

        public int ID { get; set; }
        public readonly ServerConfiguration _config = null;

        public AppChildServer(int id, ServerConfiguration config)
        {
            this.ID = id;
            this._config = config;
        }
        Task task = null;

        /// <summary>
        /// 启动
        /// </summary>
        public AppChildServer Start()
        {
            if (task == null)
            {
                task = new Task(new Action(Hand), TaskCreationOptions.LongRunning);
                task.Start();
            }
            return this;
        }

        //关闭
        public void Close()
        {
            cts.Cancel();
        }

        public void Add(TcpClient client)
        {
            queue.Enqueue(client);
        }

        private async void Hand()
        {
            //会话出队并启动会话
            while (cts.Token.IsCancellationRequested != true)
            {
                TcpClient client = null;
                if (queue.TryDequeue(out client))
                {
                    client.NoDelay = true;//禁用延迟发送数据
                    client.SendTimeout = 60000;//60秒
                    client.ReceiveTimeout = 60000;//60秒
                    AppSession appSession = new AppSession(Guid.NewGuid(), client, this._config);//创建会话
                    AppServer.GetAppServer().RegisterSession(appSession);//注册会话
                    appSession.Start();
                }
                else
                {
                    await Task.Delay(100);//暂停100毫秒
                }
            }
        }

    }
}
