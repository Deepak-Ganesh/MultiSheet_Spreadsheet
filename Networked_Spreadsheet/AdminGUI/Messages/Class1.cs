using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class errorMessage
    {
        public string type { get; set; }
        public string code { get; set; }
        public string source { get; set; }
    }

    public class listMessage
    {
        public string type { get; set; }
        public string[] spreadsheets { get; set; }
    }

    public class editOverviewMessage
    {
        public string type { get; set; }
        public string name { get; set; }
        public string owner { get; set; }
        public string edits { get; set; }
    }

    public class userListMessage
    {
        public string type;
        public Dictionary<string, bool> users;
    }

    public class loginMessage
    {
        public string type = "open";
        public string username;
        public string password;
    }

    public class adminMessage
    {
        public string type;
        public string username;
        public string password;
        public string name;
    }

}
