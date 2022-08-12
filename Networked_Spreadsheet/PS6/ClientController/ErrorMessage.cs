using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{

    /// <summary>
    /// Error message class that encapsulates the fields of an error message for the client and server to interpret.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ErrorMessage
    {
        [JsonProperty]
        public string type { get; private set; }
        [JsonProperty]
        public string code { get; private set; }
        [JsonProperty]
        public string source { get; private set; }

        /// <summary>
        /// Default constructor for JSON
        /// </summary>
        public ErrorMessage()
        {
        }

        /// <summary>
        /// Constructor that takes in a code (of the error), and a source (of the error).
        /// </summary>
        public ErrorMessage(string code, string source)
        {
            type = "error";

            this.code = code;
            this.source = source;
        }

        /// <summary>
        /// Returns a JSON string representation of the ErrorMessage.
        /// </summary>
        public override string ToString()
        {
            string ret = Newtonsoft.Json.JsonConvert.SerializeObject(this).ToString();
            return ret + "\n\n";
        }
    }
}
