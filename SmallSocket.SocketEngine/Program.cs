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
            AppServer appServer = AppServer.GetAppServer();

            SimpleServerMessageDispatcher dispatcher = new SimpleServerMessageDispatcher();
            ServerConfiguration config = new ServerConfiguration(dispatcher);
            appServer.Start(config);
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
