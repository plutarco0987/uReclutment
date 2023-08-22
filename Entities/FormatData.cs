using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities
{
    public class FormatData<T>
    {
        public FormatData()
        {

        }

        public FormatData(IEnumerable<T> data,  bool result, int statusCode, string messageToFrontEnd, string errorMessage="", string errorLocation = "", string errorStackTrace="")
        {
            Data = data;
            ErrorMessage = errorMessage;
            ErrorLocation = errorLocation;
            Result = result;
            StatusCode = statusCode;
            ErrorStackTrace = errorStackTrace;
            MessageToFrontEnd = messageToFrontEnd;
        }





        /// <summary>
        /// Generic object,this could be null if is sucess one method that is not getall, this is for get all
        /// or get one, or put or delete if is required return the value deleted
        /// </summary>
        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; set; }
        /// <summary>
        /// Short error message
        /// </summary>
        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Error location, everything methods will have some points for know what could be the issue
        /// </summary>
        [JsonPropertyName("errorLocation")]
        public string ErrorLocation { get; set; }
        /// <summary>
        /// this will be the result of the process, True if everythin is good or False if exist one issue
        /// </summary>
        [JsonPropertyName("result")]
        public bool Result { get; set; }
        /// <summary>
        /// Resultstatus of the call
        /// </summary>
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; } 
        /// <summary>
        /// Everything of the error
        /// </summary>
        [JsonPropertyName("errorStackTrace")]
        public string ErrorStackTrace { get; set; }
        /// <summary>
        /// this is the message to display in the front end
        /// </summary>
        [JsonPropertyName("messageToFrontEnd")]
        public string MessageToFrontEnd { get; set; }   
    }
}
