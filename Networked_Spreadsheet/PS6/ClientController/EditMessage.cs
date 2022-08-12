using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{

    /// <summary>
    /// Edit message class that encapsulates the fields of an open message for the client and server to interpret.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class EditMessage
    {
        // NOTE: this class now probably functions as intended. its message should be formatted correctly.
        //      to test message functionality, refer to testclient's message sending functionality to a server,
        //      and examine the server's output to see the message's contents and how it is formatted.
        //      the wrapper class written here was removed, since simply naming the List<string> as a 
        //      JSON proptery seems to generate the correct output

        [JsonProperty]
        public string type { get; private set; }
        [JsonProperty]
        public string cell { get; private set; }
        [JsonProperty]
        public string value { get; private set; }

        [JsonProperty]
        public List<string> dependencies { get; private set; }


        /// <summary>
        /// Default constructor for JSON
        /// </summary>
        public EditMessage()
        {
        }

        /// <summary>
        /// Constructor that takes in a cell name (to be edited), the new value of the cell, and a list of dependencies for that cell.
        /// </summary>
        public EditMessage(string cell, string value, List<string> dependencies)
        {
            type = "edit";

            this.cell = cell;
            this.value = value;
            this.dependencies = dependencies;
        }

        /// <summary>
        /// Returns a JSON string representation of the editmessage.
        /// </summary>
        public override string ToString()
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(this).ToString();
            return ret + "\n\n";
        }
    }
}
