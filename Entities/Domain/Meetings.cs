using Entities.Formats;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataContext
{
    public class Meetings
    {
        public Meetings()
        {

        }
        public Meetings(MeetingsFormat meetingsFormat)
        {
            this.MeetingsId = meetingsFormat.MeetingsId;
            this.CandidatesId= meetingsFormat.CandidatesId;
            this.NumberMeeting= meetingsFormat.NumberMeeting;
            this.Time= meetingsFormat.Time;
            this.Active= meetingsFormat.Active;
            this.NameCreated= meetingsFormat.NameCreated;
            this.DateCreated= meetingsFormat.DateCreated;
            this.NameModified= meetingsFormat.NameModified;
            this.DateModified= meetingsFormat.DateModified;
            this.Candidates = null;
        }


        public void MeetingsFormat(MeetingsFormat meetingsFormat)
        {            
            this.CandidatesId = meetingsFormat.CandidatesId;
            this.NumberMeeting = meetingsFormat.NumberMeeting;
            this.Time = meetingsFormat.Time;
            this.Active = meetingsFormat.Active;         
            this.NameModified = meetingsFormat.NameModified;
            this.DateModified = meetingsFormat.DateModified;            
        }


        public void SetId(int id)
        {
            this.MeetingsId = id;
        }
        [Key]
        [JsonPropertyName("meetingsId")]
        public int MeetingsId { get;  set; }
        [JsonPropertyName("candidatesId")]
        [ForeignKey("Candidates")]
        public int CandidatesId { get; set; }
        [JsonPropertyName("numberMeeting")]
        [Required]
        public int NumberMeeting { get; set; }
        [JsonPropertyName("time")]        
        public long? Time { get; set; }
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("NameCreated")]
        [Required]
        [MaxLength(100)]
        public string? NameCreated { get; set; }
        [JsonPropertyName("dateCreated")]
        [Required]
        public DateTime DateCreated { get; set; }
        [JsonPropertyName("NameModified")]
        [Required]
        [MaxLength(100)]
        public string? NameModified { get; set; }
        [JsonPropertyName("dateModified")]
        [Required]
        public DateTime DateModified { get; set; }

        public virtual Candidates? Candidates { get; set; }

    }
}
