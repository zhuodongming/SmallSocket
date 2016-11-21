using SmallSocket.SocketEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace SmallSocket.SocketEngine
{
    internal sealed class UdpSocketListener : ISocketListener
    {
        public IPEndPoint EndPoint
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
