using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WpfClient
{
    public delegate void FileTransferProgressEventHandler(object sender, FileTransferProgressEventArgs args);
    public class FileTransferProgressEventArgs : EventArgs
    {
        public string ID { get; set; }
        public double Progress { get; set; }

        public FileTransferProgressEventArgs(string id, double progress)
        {
            this.ID = id;
            this.Progress = progress;
        }
    }

    /// <summary> State object for receiving data from remote peer.</summary>
    public class StateObjectFileRequest
    { 
        public Socket workSocket = null;                // Client socket
        public const int BufferSize = 10000;            // Size of receive buffer
        public byte[] buffer = new byte[BufferSize];    // Receive buffer
        public StringBuilder sb = new StringBuilder();  // Received data string
    }

    /// <summary> FileDownloadRequest class for requesting data from remote peer.</summary>
    public class FileRequest
    {
        public event FileTransferProgressEventHandler FileTransferProgressChangeEvent;

        public const string END_TOKEN = "<EOF>";

        // ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        // The response from the remote device. Saved to a file after the download ends
        private String response = String.Empty;

        int RemotePort; 
        string RemoteIP;
        string RequestedFilename;
        long fileSizeByte;
        string ID;

        public FileRequest(string requestId, string remoteIP, int remotePort, string requestedFilename, long fileSizeByte)
        {
            this.RemotePort = remotePort;
            this.RemoteIP = remoteIP;
            this.RequestedFilename = requestedFilename;
            this.fileSizeByte = fileSizeByte;
            this.ID = requestId;
        }

        public void StartFileDownload()
        {
            // Connect to a remote device.
            try
            {
                IPAddress ipAddress = IPAddress.Parse(RemoteIP);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, RemotePort);

                //  Create a TCP/IP  socket.
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

                // Send test data to the remote device.
                Send(client, RequestedFilename + END_TOKEN);
                sendDone.WaitOne();

                // Receive the response from the remote device.
                Receive(client);
                receiveDone.WaitOne();

                // Log the response to the console.
                Console.WriteLine("Response received, saving file");

                // Release the socket.
                //client.Shutdown(SocketShutdown.Both);
               // client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);
                Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                // Create the state object.
                StateObjectFileRequest state = new StateObjectFileRequest();
                state.workSocket = client;
                //state.buffer = new byte[StateObjectFileRequest.BufferSize];
                // Begin receiving the data from the remote device.
                client.BeginReceive(state.buffer, 0, StateObjectFileRequest.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Console.WriteLine("ReceiveCallback ent");
                // Retrieve the state object and the client socket from the async state object.
                StateObjectFileRequest state = (StateObjectFileRequest)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);
                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.Default.GetString(state.buffer, 0, bytesRead));
                    double progress = (double)state.sb.Length / this.fileSizeByte;
                    Console.WriteLine("Client received " + state.sb.Length + " bytes... \t\t" + progress);
                    FileTransferProgressChangeEvent(this, new FileTransferProgressEventArgs(this.ID, progress));

                    client.BeginReceive(state.buffer, 0, StateObjectFileRequest.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);

                    Console.WriteLine("after  client.BeginReceive");
                }
                else
                {
                    // All the data has arrived; put it in response.
                    if (state.sb.Length > 1)
                    {
                        Console.WriteLine("Client received all " + state.sb.Length + " bytes.");
                        response = state.sb.ToString();

                        // Save the file
                        string filepath = Path.Combine(Properties.Settings.Default.DownloadsPath, RequestedFilename);
                        File.WriteAllBytes(filepath, Encoding.Default.GetBytes(response));

                        // Signal that all bytes have been received.
                        receiveDone.Set();
                   } 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ReceiveCallback   ----  " + e.ToString());
            }
        }

        private void Send(Socket client, String data)
        {
            // Convert the string data to byte data using Default encoding.
            byte[] byteData = Encoding.Default.GetBytes(data);

            // Begin sending the data to the remote device.
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
