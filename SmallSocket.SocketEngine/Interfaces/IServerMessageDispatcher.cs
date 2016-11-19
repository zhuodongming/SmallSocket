using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallSocket.SocketEngine.Interfaces
{
    public interface IServerMessageDispatcher
    {
        Task OnListenerStarted(ISocketListener listener);
        Task OnListenerStoped(ISocketListener listener);
        Task OnListenerError(ISocketListener listener, Exception ex);

        Task OnSessionStarted(AppSession session);
        Task OnSessionDataReceived(AppSession session, byte[] data, int offset, int count);
        Task OnSessionClosed(AppSession session);
    }
}
