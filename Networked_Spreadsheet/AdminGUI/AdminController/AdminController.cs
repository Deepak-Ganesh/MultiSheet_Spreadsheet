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


namespace Backend
{

    public class AdminController
    {
        public delegate void ssHandler(string[] spreadsheets);
        public event ssHandler ssReceived;
        public delegate void editHandler(string owner, string edit, string ss);
        public event editHandler editReceived;
        public delegate void usersHandler(Dictionary<string, bool> users);
        public event usersHandler usersReceived;

        private bool flag;
        private Socket theServer;
        public AdminController(string hostname)
        {
            flag = false;
            theServer = Networking.ConnectToServer(FirstContact, hostname);
        }

        private void FirstContact(SocketState ss)
        {
            ss.callMe = callMe;
            Networking.GetData(ss);
        }

        private void callMe(SocketState ss)
        {

            //Perform handshake
            string data = ss.sb.ToString();
            string[] parts = Regex.Split(data, @"(?<=[\n])"); //string[] parts = Regex.Split(data, @"\n{2}");
            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 

                if (p != "\n")
                {
                    JObject obj = JObject.Parse(p);
                    JToken Type = obj["type"];
                    string type = "";

                    type = Type.ToString();

                    if (type == "list")
                    {
                        listMessage message = JsonConvert.DeserializeObject<listMessage>(p);
                        ssReceived(message.spreadsheets);
                        if (!flag)
                        {
                            loginMessage m = new loginMessage();
                            m.username = "admin";
                            m.password = "password";
                            string json = JsonConvert.SerializeObject(m);
                            Networking.Send(theServer, json + "\n\n");
                            flag = true;
                        }
                    }

                    else if (type == "edit overview")
                    {
                        editOverviewMessage message = JsonConvert.DeserializeObject<editOverviewMessage>(p);
                        editReceived(message.owner, message.edits, message.name);
                    }

                    else if (type == "error")
                    {
                        errorMessage message = JsonConvert.DeserializeObject<errorMessage>(p);
                    }

                    else if (type == "user list")
                    {
                        userListMessage message = JsonConvert.DeserializeObject<userListMessage>(p);
                        usersReceived(message.users);
                    }
                }
                ss.sb.Remove(0, p.Length);
            }

            Networking.GetData(ss);
        }

        public void registerHandlers(ssHandler ssH, editHandler editH, usersHandler usersH)
        {
            ssReceived += ssH;
            editReceived += editH;
            usersReceived += usersH;
        }

        

        public string createUser(string username, string password)
        {
            adminMessage m = new adminMessage();
            m.type = "CRU";
            m.username = username;
            m.password = password;
            string json = JsonConvert.SerializeObject(m);
            Networking.Send(theServer, json + "\n\n");
            return "";
        }

        public string deleteUser(string username)
        {
            adminMessage m = new adminMessage();
            m.type = "DLU";
            m.username = username;
            string json = JsonConvert.SerializeObject(m);
            Networking.Send(theServer, json + "\n\n");
            return "";
        }

        public string changePassword(string username, string password)
        {
            adminMessage m = new adminMessage();
            m.type = "CHP";
            m.username = username;
            m.password = password;
            string json = JsonConvert.SerializeObject(m);
            Networking.Send(theServer, json + "\n\n");
            return "";
        }

        public string removeSpreadsheet(string name)
        {
            adminMessage m = new adminMessage();
            m.type = "RMS";
            m.name = name;
            string json = JsonConvert.SerializeObject(m);
            Networking.Send(theServer, json + "\n\n");
            return "";
        }

        public string createSpreadsheet(string name)
        {
            adminMessage m = new adminMessage();
            m.type = "CRS";
            m.name = name;
            string json = JsonConvert.SerializeObject(m);
            Networking.Send(theServer, json + "\n\n");
            return "";
        }

        public string shutDownServer()
        {
            adminMessage m = new adminMessage();
            m.type = "SDS";
            string json = JsonConvert.SerializeObject(m);
            Networking.Send(theServer, json + "\n\n");
            return "";

        }
    }
}
