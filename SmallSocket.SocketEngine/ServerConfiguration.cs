using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Authentication;
using System.Net;
using SmallSocket.SocketEngine.Interfaces;

namespace SmallSocket.SocketEngine
{
    public class ServerConfiguration
    {
        public ServerConfiguration(IServerMessageDispatcher dispatcher)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher", "argument dispatcher is not null");
            }

            this.Dispatcher = dispatcher;
        }
        public IServerMessageDispatcher Dispatcher { get; private set; }
        public IPEndPoint ListenedEndPoint { get; set; } = new IPEndPoint(IPAddress.Any, 12345);
        public bool IsTCP { get; set; } = true;

        //public ISegmentBufferManager BufferManager { get; set; }
        public int ReceiveBufferSize { get; set; } = 8192;
        public int SendBufferSize { get; set; } = 8192;
        public TimeSpan ReceiveTimeout { get; set; } = TimeSpan.Zero;
        public TimeSpan SendTimeout { get; set; } = TimeSpan.Zero;
        public bool NoDelay { get; set; } = true;
        public LingerOption LingerState { get; set; } = new LingerOption(false, 0);

        public int PendingConnectionBacklog { get; set; } = 200;
        public bool AllowNatTraversal { get; set; } = true;

        public bool SslEnabled { get; set; } = false;
        public X509Certificate2 SslServerCertificate { get; set; } = null;
        public EncryptionPolicy SslEncryptionPolicy { get; set; } = EncryptionPolicy.RequireEncryption;
        public SslProtocols SslEnabledProtocols { get; set; } = SslProtocols.Ssl3 | SslProtocols.Tls;
        public bool SslClientCertificateRequired { get; set; } = true;
        public bool SslCheckCertificateRevocation { get; set; } = false;
        public bool SslPolicyErrorsBypassed { get; set; } = false;

        public TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromSeconds(15);
        //public IFrameBuilder FrameBuilder { get; set; }
    }
}
