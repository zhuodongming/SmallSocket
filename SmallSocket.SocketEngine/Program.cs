using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmallSocket.SocketEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            //Start appServer
            IPAddress ipAddress = IPAddress.Any;
            int port = 12345;
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            TcpSocketListener tcpSocketListener = new TcpSocketListener(ipEndPoint);

            AppServer appServer = AppServer.GetAppServer();
            appServer.Start(tcpSocketListener);
            Console.WriteLine("The server started successfully, press 'quit' to stop it!");

            while (!Console.ReadLine().ToLower().Trim().Equals("quit"))
            {
                //Console.WriteLine();
            }

            //Stop the appServer
            appServer.Close();
            Console.WriteLine("The server was stopped!");
        }
    }
}
