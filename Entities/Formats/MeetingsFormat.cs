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
    public class MeetingsFormat
    {
        public MeetingsFormat()
        {

        }

        public MeetingsFormat(int meetingsId, int candidatesId, int numberMeeting, long? time, bool active, string? nameCreated, DateTime dateCreated, string? nameModified, DateTime dateModified)
        {
            MeetingsId = meetingsId;
            CandidatesId = candidatesId;
            NumberMeeting = numberMeeting;
            Time = time;
            Active = active;
            NameCreated = nameCreated;
            DateCreated = dateCreated;
            NameModified = nameModified;
            DateModified = dateModified;
        }

        [JsonPropertyName("meetingsId")]
        public int MeetingsId { get; set; }
        [JsonPropertyName("candidatesId")]        
        public int CandidatesId { get; set; }
        [JsonPropertyName("numberMeeting")]        
        public int NumberMeeting { get; set; }
        [JsonPropertyName("time")]
        public long? Time { get; set; }
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
