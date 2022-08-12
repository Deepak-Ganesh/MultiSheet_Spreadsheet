using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text.RegularExpressions;
using static NetworkController.Networking;

namespace NetworkController
{

    /// <summary>
    /// This helps in determining:
    ///  1) what socket the data is on
    ///  2) what previous data has arrived
    ///  3) what function to call when data arrives. This function will be how the Network Controller interacts
    ///     with the Game Controller, since the Network Controller can not know anything about our specific game.
    /// </summary>
    public class SocketState
    {
        public Socket theSocket;
        public int ID;

        // This is the buffer where we will receive data from the socket
        public byte[] messageBuffer = new byte[4096];

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        public StringBuilder sb = new StringBuilder();

        // The action that the socket will utilize when the server wants to notify the client that data has been received
        public Action<SocketState> callMe;

        public SocketState(Socket s, int id)
        {
            theSocket = s;
            ID = id;
        }
    }

    //Made the same thing as a socketstate class here just for a tcp listener. This makes a little more sense. Super basic.
    public class ConnectionState
    {
        public Action<SocketState> callMe;

        public TcpListener listener;

        public ConnectionState(TcpListener listen, Action<SocketState> action)
        {
            listener = listen;
            callMe = action;
        }
    }


    /// <summary>
    /// Contains helper methods that handle networking (with the server).
    /// </summary>
    public static class Networking
    {
        public const int DEFAULT_PORT = 11000;

        /// <summary>
        /// ConnectToServer should attempt to connect to the server via a provided hostname.
        /// 
        /// It should save the callMe function in a socket state object for use when data arrives.
        /// 
        /// It will need to create a socket and then use the BeginConnect method.
        /// 
        /// Note this method takes the "state" object and "regurgitates" it back to you when a connection is made,
        ///  thus allowing communication between this function and the ConnectedCallback function (below).
        /// </summary>
        /// 
        /// <param name="callMe">
        ///  a delegate function to be called when a connection is made.
        ///   the SpaceWars client will provide this function when it invokes ConnectToServer.
        /// </param>
        /// <param name="hostname">Name of the server to connect to</param>
        /// <returns>the "state" object it was orginally given when the connection was made.</returns>
        public static Socket ConnectToServer(Action<SocketState> callMe, string hostName)
        {
            System.Diagnostics.Debug.WriteLine("connecting  to " + hostName);

            // Create a TCP/IP socket.
            Socket socket;
            IPAddress ipAddress;

            Networking.MakeSocket(hostName, out socket, out ipAddress);

            SocketState ss = new SocketState(socket, -1);
            ss.callMe = callMe;

            // for now the socket is hard coded. could probably change this later.
            //socket.BeginConnect(ipAddress, Networking.DEFAULT_PORT, ConnectedCallback, ss);
            socket.BeginConnect(ipAddress, 2112, ConnectedCallback, ss);
            return ss.theSocket;
        }

        /// <summary>
        /// This function is referenced by the BeginConnect method above and
        ///  is called by the OS when the socket connects to the server.
        ///  
        /// The "stateAsArObject" object contains a field "AsyncState" which
        ///  contains the "state" object saved away in the above function.
        ///  
        /// You will have to cast it from object to SocketState.
        /// 
        /// Once a connection is established the "saved away" callMe needs to called.
        /// 
        /// This function is saved in the socket state, and was originally passed in to ConnectToServer (above).
        /// </summary>
        /// <param name="stateAsArObject"></param>
        private static void ConnectedCallback(IAsyncResult ar)
        {
            SocketState ss = (SocketState)ar.AsyncState;

            try
            {
                // Complete the connection.
                ss.theSocket.EndConnect(ar);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }

            ss.callMe(ss);
        }


        /// <summary>
        /// This is a small helper function that the client code will call whenever it wants more data.
        /// Note: the client will probably want more data every time it gets data, and has finished processing it in its callMe.
        /// </summary>
        /// <param name="state"></param>
        public static void GetData(SocketState state)
        {
            state.theSocket.BeginReceive(state.messageBuffer, 0, state.messageBuffer.Length, SocketFlags.None, ReceiveCallback, state);
        }

        /// <summary>
        /// The ReceiveCallback method is called by the OS when new data arrives.
        /// 
        /// This method should check to see how much data has arrived. 
        /// 
        /// If 0, the connection has been closed (presumably by the server). 
        /// 
        /// On greater than zero data, this method should get the SocketState object out of the IAsyncResult (just as above in 2),
        ///  and call the callMe provided in the SocketState.
        /// </summary>
        /// <param name="state"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            // Checks for a disconnected socket. Usually by the client closing out while connected to the server.
            try
            {
                SocketState ss = (SocketState)ar.AsyncState;
                int bytesRead = ss.theSocket.EndReceive(ar);

                // If the socket is still open
                if (bytesRead > 0)
                {
                    string theMessage = Encoding.UTF8.GetString(ss.messageBuffer, 0, bytesRead);
                    // Append the received data to the growable buffer.
                    // It may be an incomplete message, so we need to start building it up piece by piece
                    ss.sb.Append(theMessage);
                    ss.callMe(ss);
                }
            }
            catch (System.Net.Sockets.SocketException e)
            {
                // Will write to the server's console whenever a (client) disconnection occurs.
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// This function (along with its helper 'SendCallback') will allow a program to send data over a socket.
        /// This function needs to convert the data into bytes and then send them using "socket.BeginSend."
        /// </summary>
        /// <param name="state"></param>
        public static void Send(Socket socket, string data)
        {
            if (socket.Connected)
            {
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data + "\n");
                    socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, SendCallback, socket);
                }
                catch (Exception)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
        }

        /// <summary>
        /// A callback invoked when a send operation completes.
        /// This function assists the Send function.
        /// It should extract the Socket out of the IAsyncResult, and then call socket.EndSend.
        /// We may, when first prototyping this program, put a WriteLine in here to see when data goes out.
        /// </summary>
        /// <param name="state"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            Socket state = (Socket)ar.AsyncState;
            try
            {
                state.EndSend(ar);
            }
            catch (Exception)
            {
                state.Shutdown(SocketShutdown.Both);
                state.Close();
            }
        }


        /// <summary>
        /// Creates a Socket object for the given host string
        /// </summary>
        /// <param name="hostName">The host name or IP address</param>
        /// <param name="socket">The created Socket</param>
        /// <param name="ipAddress">The created IPAddress</param>
        public static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
        {
            ipAddress = IPAddress.None;
            socket = null;
            try
            {
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;

                // Determine if the server address is a URL or an IP
                try
                {
                    ipHostInfo = Dns.GetHostEntry(hostName);
                    bool foundIPV4 = false;
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            foundIPV4 = true;
                            ipAddress = addr;
                            break;
                        }
                    // Didn't find any IPV4 addresses
                    if (!foundIPV4)
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid addres: " + hostName);
                        throw new ArgumentException("Invalid address");
                    }
                }
                catch (Exception)
                {
                    // see if host name is actually an ipaddress, i.e., 155.99.123.456
                    System.Diagnostics.Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(hostName);
                }

                // Create a TCP/IP socket.
                socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                // Disable Nagle's algorithm - can speed things up for tiny messages, 
                // such as for a game
                socket.NoDelay = true;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create socket. Error occured: " + e);
                throw new ArgumentException("Invalid address");
            }
        }


        /*
        //------------------------- SERVER ----------------------------

        /// <summary>
        /// Main method the server uses to listen for clients attempting to connect to it.
        /// </summary>
        public static void ServerAwaitingClientLoop(Action<SocketState> callMe)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 11000);

            tcpListener.Start();
            ConnectionState cs = new ConnectionState(tcpListener, callMe);
            tcpListener.BeginAcceptSocket(new AsyncCallback(AcceptNewClient), cs);
        }

        /// <summary>
        /// Method that is called whenever a new client is found. Lets the server know when this happens, invoking
        ///  its callMe to begin dialoguing with the newly connected client.
        /// </summary>
        public static void AcceptNewClient(IAsyncResult ar)
        {
            ConnectionState cs = (ConnectionState)ar.AsyncState;
            Socket socket = cs.listener.EndAcceptSocket(ar);
            SocketState newClient = new SocketState(socket, -1);

            //set all the things that were connected.
            newClient.theSocket = socket;
            newClient.callMe = cs.callMe;
            newClient.callMe(newClient);
            cs.listener.BeginAcceptSocket(new AsyncCallback(AcceptNewClient), cs);
        }
        */
    }
}


