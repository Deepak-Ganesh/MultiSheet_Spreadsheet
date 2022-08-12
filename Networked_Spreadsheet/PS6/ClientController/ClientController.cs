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
using NetworkController;
using System.Threading;
using System.Text.RegularExpressions;
using System.Timers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

namespace SS
{
    public class ClientController
    {
        // the clientcontroller keeps a socket that connects to the server to keep contact
        private Socket theServer;

        // the username and password that the gui takes in, and passes to this controller
        public string username { get; set; }
        public string password { get; set; }

        // Delegates and events used to let the form knows what's happening.
        // lets the form know that a connection has successfully taken place
        public delegate void ConnectionHandler();
        public event ConnectionHandler ConnectionArrived;

        // lets the form know that a list of spreadsheets has been received from the server
        public delegate void SpreadsheetListUpdateHandler(SpreadsheetListMessage SSLM);
        public event SpreadsheetListUpdateHandler SpreadsheetsArrived;

        // lets the form know that a full spreadsheet has been received from the server
        // note that fullsend may not actually send the full spreadsheet again, only its
        //  deltas.
        // alternatively may send an errormessage
        public delegate void FullSheetHandler(FullSendMessage FSM);
        public event FullSheetHandler FullSheetArrived;

        public delegate void ErrorHandler(ErrorMessage EM);
        public event ErrorHandler ErrorArrived;

        /// <summary>
        /// used when a client begins connection to a server. at this point, the client should have 
        ///  entered their username and password, saved for this clientcontroller's use.
        /// </summary>
        /// <param name="ip"></param>
        public void ConnectToServer(string ip)
        {
            theServer = Networking.ConnectToServer(FirstContact, ip);
        }

        /// <summary>
        /// Registers various events to let the GUI know what is happening
        /// </summary>
        /// <param name="h"></param>
        public void RegisterHandlers(ConnectionHandler ch, FullSheetHandler fsh, SpreadsheetListUpdateHandler ssl, ErrorHandler err)
        {
            ConnectionArrived += ch;
            FullSheetArrived += fsh;
            SpreadsheetsArrived += ssl;
            ErrorArrived += err;
        }

        public void ClearHandlers()
        {
            ConnectionArrived = null;
            FullSheetArrived = null;
            SpreadsheetsArrived = null;
            ErrorArrived = null;
        }

        /// <summary>
        /// Registers only fsm and er messages for the spreadsheetgui's use.
        /// </summary>
        /// <param name="fsh"></param>
        /// <param name="err"></param>
        public void RegisterSheetHandlers(FullSheetHandler fsh, ErrorHandler err)
        {
            FullSheetArrived += fsh;
            ErrorArrived += err;
        }

        /// <summary>
        /// The delegate used to initially handshake with the server.
        /// </summary>
        private void FirstContact(SocketState ss)
        {
            // set up the delegate to be called when the server sends a message
            ss.callMe = ReceiveSpreadsheets;

            // let the form know the connection has succeeded
            ConnectionArrived?.Invoke();

            // no messages to be sent at the moment; continue receiving data until
            //  the server sends the client a list of available spreadsheets
            Networking.GetData(ss);
        }

        /// <summary>
        /// This method is used as a delegate for when the server initially sends the list of spreadsheets.
        /// </summary>
        public void ReceiveSpreadsheets(SocketState ss)
        {
            // parse the initial data send
            string totalData = ss.sb.ToString();
            // this might be wrong, but should parse upon seeing \n\n
            string[] parts = Regex.Split(totalData, @"(?<=[\n\n])");

            // we might not need this code inside of a for loop. keeping it here just in case.
            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;

                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p != "\n")
                {

                    dynamic msgObj = JObject.Parse(p);
                    JToken Type = msgObj["type"];

                    if (Type.ToString() != "list")
                        throw new FormatException("Incorrect format of message sent by the server");
                    else
                    {
                        SpreadsheetListMessage SSLM = JsonConvert.DeserializeObject<SpreadsheetListMessage>(p);
                        SpreadsheetsArrived?.Invoke(SSLM);
                    }
                }
                ss.sb.Remove(0, p.Length);
            }

            // this is the method that will be called when the client sends the openmessage to the server.
            //  it is not immediate and is done through senddata, so wait until then.
            ss.callMe = ReceiveRequestedSpreadsheet;
            Networking.GetData(ss);
        }

        /// <summary>
        /// This is the method that begins parsing the spreadsheet the server gave to the client 
        ///  that it requested earlier.
        /// This is in the form of a fullsend message, even though it may not be a full
        ///  spreadsheet, only its deltas.
        /// </summary>
        /// <param name="ss"></param>
        public void ReceiveRequestedSpreadsheet(SocketState ss)
        {
            string totalData = ss.sb.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            // might not need this in a for loop, but here just in case.
            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;

                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p != "\n")
                {
                    // do work here
                    dynamic msgObj = JObject.Parse(p);
                    JToken Type = msgObj["type"];

                    if (Type.ToString() != "full send" && Type.ToString() != "error" && Type.ToString() != "list")
                        throw new FormatException("Incorrect format of message sent by the server");

                    if (Type.ToString() == "full send")
                    {
                        // the request was successful, so proceed to the spreadsheet gui's update functionality.
                        FullSendMessage FSM = JsonConvert.DeserializeObject<FullSendMessage>(p);
                        ss.callMe = ReceiveSpreadsheetUpdates;

                        Networking.GetData(ss);
                        FullSheetArrived?.Invoke(FSM);
                    }
                    else if (Type.ToString() == "error")
                    {
                        // send the client the error, try again
                        ErrorMessage EM = JsonConvert.DeserializeObject<ErrorMessage>(p);
                        ErrorArrived?.Invoke(EM);
                    }
                    else
                    {
                        // send the client the list object
                        SpreadsheetListMessage SLM = JsonConvert.DeserializeObject<SpreadsheetListMessage>(p);
                        SpreadsheetsArrived?.Invoke(SLM);
                    }
                }

                // Then remove it from the SocketState's growable buffer
                ss.sb.Remove(0, p.Length);
            }
            
            Networking.GetData(ss);
        }

        public void ReceiveSpreadsheetUpdates(SocketState ss)
        {
            string totalData = ss.sb.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");
            //Console.WriteLine("==> ReceiveSpreadsheetUpdates()");

            // might not need this in a for loop, but here just in case.
            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;

                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p != "\n")
                {
                    // do work here
                    dynamic msgObj = JObject.Parse(p);
                    JToken Type = msgObj["type"];

                    if (Type.ToString() != "full send" && Type.ToString() != "error")
                        throw new FormatException("Incorrect format of message sent by the server");

                    if (Type.ToString() == "full send")
                    {
                        FullSendMessage FSM = JsonConvert.DeserializeObject<FullSendMessage>(p);

                        Networking.GetData(ss);
                        FullSheetArrived?.Invoke(FSM);
                    }
                    else if (Type.ToString() == "error")
                    {
                        ErrorMessage EM = JsonConvert.DeserializeObject<ErrorMessage>(p);
                        ErrorArrived?.Invoke(EM);
                    }
                }

                // Then remove it from the SocketState's growable buffer
                ss.sb.Remove(0, p.Length);
            }

            //Console.WriteLine("<== ReceiveSpreadsheetUpdates()");
            Networking.GetData(ss);
        }

        /// <summary>
        /// sends a message, a string, to the server
        /// </summary>
        public void SendData(string data)
        {
            Networking.Send(theServer, data);
        }

        /// <summary>
        /// Returns a boolean values that indicates whether the client is connected to the server or not.
        ///  Not particularly useful function. Just for testing purposes.
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            if (theServer != null)
                return theServer.Connected;
            else
                return false;
        }

        private void ConnectionFailed()
        {

        }
    }
}
