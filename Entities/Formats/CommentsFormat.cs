using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Formats
{
    public class CommentsFormat
    {
        public CommentsFormat()
        {

        }
        public CommentsFormat(int commentsId, int questionDetailsId, string? value, bool active, string? nameCreated, DateTime dateCreated, string? nameModified, DateTime dateModified)
        {
            CommentsId = commentsId;
            QuestionDetailsId = questionDetailsId;
            Value = value;
            Active = active;
            NameCreated = nameCreated;
            DateCreated = dateCreated;
            NameModified = nameModified;
            DateModified = dateModified;
        }

        [JsonPropertyName("commentsId")]
        public int CommentsId { get; set; }
        [JsonPropertyName("questionDetailsId")]        
        public int QuestionDetailsId { get; set; }
        [JsonPropertyName("value")]        
        public string? Value { get; set; }
        [JsonPropertyName("active")]
        public bool Active { get; set; }
        [JsonPropertyName("NameCreated")]        
        public string? NameCreated { get; set; }
        [JsonPropertyName("dateCreated")]        
        public DateTime DateCreated { get; set; }
        [JsonPropertyName("NameModified")]        
        public string? NameModified { get; set; }
        [JsonPropertyName("dateModified")]
        public DateTime DateModified { get; set; }
    }
}
