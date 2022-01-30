using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfClient
{
    /// <summary> State object for receiving data from remote peer.</summary>
    public class StateObject
    {
        public Socket workSocket = null;       // Client  socket.
        public const int BufferSize = 10000;       // Size of receive buffer.
        public int counter = 0;       // counter of received bytes
        public int ReceiveCounte = 0;       // counter of received bytes
        public byte[] buffer = new byte[BufferSize]; // Receive buffer.
        public StringBuilder sb = new StringBuilder();  // Received data string.
                                                      
        public DateTime StartTime, EndTime;
        public bool FirstBit = true;
        public int GetBufferSize() { return BufferSize; }
    }

    public class FileRequestListener
    {
        public const string END_TOKEN = "<EOF>";
        public string DOWNLOADS_FOLDER = Properties.Settings.Default.DownloadsPath;

        public int port = 4080;       // portIn
                                      // Incoming data from client.
        public string data = null;
        public static int NumberOfConnections = 0;
        ArrayList ConnectionsList = new ArrayList();


        public FileRequestListener(int portIn)
        {
            this.port = portIn;
        }

        // Thread signal.
        public ManualResetEvent allDone = new ManualResetEvent(false);

        public void StartListening()
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[StateObject.BufferSize];

            // Establish the local endpoint for the  socket.
            //   The DNS name of the computer

            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            Console.WriteLine("Binding port {0} at local address {1}", port, ipAddress.ToString());
            // Create a TCP/IP  socket.
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Waiting for a connection...\n\n");
            // Bind the  socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    allDone.Reset();  // Set the event to  nonsignaled state.
                                      // Start  an asynchronous socket to listen for connections.
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener); // Non-Blocking
                    allDone.WaitOne();  // Wait until a connection is made before continuing.
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            Console.WriteLine("Connected to client at {0}\n ", handler.RemoteEndPoint.ToString());
            state.FirstBit = true;

            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            state.StartTime = DateTime.Now;

        }

        public void ReadCallback(IAsyncResult ar)
        {

            // Retrieve the state object and the handler socket
            // from the async state object.


            StateObject state = (StateObject)ar.AsyncState;

            Socket handler = state.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);
            //state.counter+=handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There might be more data, so store the data received so far.
                state.sb.Append(Encoding.Default.GetString(state.buffer, 0, bytesRead));
                // Check for end-of-file tag. If  it is not there, read more data.
                string content = state.sb.ToString();
                //Array.Copy(state.buffer, 0, state.fileDataBuffer, content.Length, bytesRead);
                Console.WriteLine("Server got {0} bytes", content.Length);
                if (content.IndexOf(END_TOKEN) > -1)
                //if (content.Length >= 25)
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    Console.WriteLine("\nClient #{0} at {1}", ++FileRequestListener.NumberOfConnections, handler.RemoteEndPoint.ToString());
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
                    string filename = content.Substring(0, content.Length - END_TOKEN.Length);
                    // Echo the data back to the client.

                    string fileFullPath = Path.Combine(DOWNLOADS_FOLDER, filename);
                    Send(handler, fileFullPath);
                }
                else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private void Send(Socket handler, String fileFullPath)
        {
            // Check if the file exists
            if (File.Exists(fileFullPath))
            {
                // Convert the string data to byte data using Default encoding.
                byte[] byteData = File.ReadAllBytes(fileFullPath);

                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }
            else
            {

            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                //allDone.Set();
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                Console.WriteLine("Waiting for a connection...\n\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
