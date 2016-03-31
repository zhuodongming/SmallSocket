using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallSocket.SocketEngine
{
    public static class AppSessionEx
    {
        public static async void Start(this AppSession appSession)
        {
            while (true)
            {
                byte[] buffer = Encoding.ASCII.GetBytes("1234567890");
                await appSession.Client.GetStream().WriteAsync(buffer, 0, buffer.Length);
                await Task.Delay(1000);
            }
        }

        public static void Close(this AppSession appSession)
        {
            appSession.Client.Client.Dispose();
            appSession.Client.GetStream().Dispose();
        }
    }
}
