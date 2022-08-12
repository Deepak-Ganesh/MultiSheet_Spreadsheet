using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{

    /// <summary>
    /// Open message class that encapsulates the fields of an open message for the client and server to interpret.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class OpenMessage
    {
        [JsonProperty]
        public string type { get; private set; }
        [JsonProperty]
        public string name { get; private set; }
        [JsonProperty]
        public string username { get; private set; }
        [JsonProperty]
        public string password { get; private set; }

        /// <summary>
        /// Default constructor for JSON
        /// </summary>
        public OpenMessage()
        {
        }

        /// <summary>
        /// Constructor that takes in a name (of the spreadsheet), a username, and a user password.
        /// </summary>
        public OpenMessage(string name, string username, string password)
        {
            type = "open";

            this.name = name;
            this.username = username;
            this.password = password;
        }

        /// <summary>
        /// Returns a JSON string representation of the openmessage.
        /// </summary>
        public override string ToString()
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(this).ToString();
            return ret + "\n\n";
        }
    }
}
