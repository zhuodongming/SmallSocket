using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SmallSocket.SocketEngine
{
    public interface ISocketListener
    {
        /// <summary>
        /// Starts to listen
        /// </summary>
        /// <returns></returns>
        void Start();

        /// <summary>
        /// Stops listening
        /// </summary>
        void Stop();
    }
}
