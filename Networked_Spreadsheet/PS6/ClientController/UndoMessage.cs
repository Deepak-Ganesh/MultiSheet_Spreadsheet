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
    public class UndoMessage
    {
        [JsonProperty]
        public string type { get; private set; }

        /// <summary>
        /// Default constructor for JSON, and only constructor since undo messages don't need anything editable parameters.
        /// </summary>
        public UndoMessage()
        {
            type = "undo";
        }

        /// <summary>
        /// Returns a JSON string representation of the undomessage.
        /// </summary>
        public override string ToString()
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(this).ToString();
            return ret + "\n\n";
        }
    }
}
