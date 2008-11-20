using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace CS266.SimCon.Controller.Networking
{
    /// <summary>
    /// State object for reading client data asynchronously
    /// </summary>
    public class StateObject
    {
        
        /// <summary>
        /// Client  socket.
        /// </summary>
        public Socket workSocket = null;

        /// <summary>
        /// Size of receive buffer.
        /// </summary>
        public const int BufferSize = 1024;
        
        /// <summary>
        /// Receive buffer.
        /// </summary>
        public byte[] buffer = new byte[BufferSize];
        

        /// <summary>
        /// Received data string.
        /// </summary>
        public StringBuilder sb = new StringBuilder();
    }
}
