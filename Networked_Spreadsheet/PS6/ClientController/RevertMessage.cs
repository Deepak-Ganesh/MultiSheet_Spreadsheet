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
    public class RevertMessage
    {
        [JsonProperty]
        public string type { get; private set; }
        [JsonProperty]
        public string cell { get; private set; }

        /// <summary>
        /// Default constructor for JSON
        /// </summary>
        public RevertMessage()
        {
        }

        /// <summary>
        /// Constructor that takes in a cell name (to be reverted).
        /// </summary>
        public RevertMessage(string cell)
        {
            type = "revert";

            this.cell = cell;
        }

        /// <summary>
        /// Returns a JSON string representation of the revertmessage.
        /// </summary>
        public override string ToString()
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(this).ToString();
            return ret + "\n\n";
        }
    }
}
