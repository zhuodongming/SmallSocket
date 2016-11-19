using SmallSocket.SocketEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallSocket.SocketEngine
{
    public sealed class SimpleServerMessageDispatcher : IServerMessageDispatcher
    {
        public Task OnListenerStarted(ISocketListener listener)
        {
            throw new NotImplementedException();
        }

        public Task OnListenerStoped(ISocketListener listener)
        {
            throw new NotImplementedException();
        }

        public Task OnListenerError(ISocketListener listener, Exception ex)
        {
            throw new NotImplementedException();
        }

        public async Task OnSessionStarted(AppSession session)
        {
            Console.WriteLine(String.Format(" Session {0} has connected.", session.Key));
            await Task.FromResult<object>(null);
        }

        public async Task OnSessionDataReceived(AppSession session, byte[] data, int offset, int count)
        {
            var text = Encoding.UTF8.GetString(data, offset, count);
            Console.Write(String.Format("Client:{0}-->", session.RemoteEndPoint));
            if (count < 1024 * 1024 * 1)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.WriteLine("{0} Bytes", count);
            }
            await session.SendAsync(data, offset, count);
        }

        public async Task OnSessionClosed(AppSession session)
        {
            Console.WriteLine(String.Format(" Session {0} has disconnected.", session.Key));
            await Task.FromResult<object>(null);
        }

    }
}
