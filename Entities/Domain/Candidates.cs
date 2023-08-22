using Entities.Formats;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public class Candidates
    {        
        public Candidates()
        {
            this.Stages=new Stages();
            this.QuestionDetails = new List<QuestionDetails>();
            this.Meetings = new List<Meetings>();
        }
        public Candidates(CandidatesFormat candidatesFormat)
        {
            this.CandidatesId= candidatesFormat.CandidatesId;
            this.StagesId= candidatesFormat.StagesId;
            this.VacancyId= candidatesFormat.VacancyId;
            this.Name= candidatesFormat.Name;
            this.Age = candidatesFormat.Age;
            this.Address= candidatesFormat.Address;
            this.City= candidatesFormat.City;
            this.Country= candidatesFormat.Country;
            this.Active= candidatesFormat.Active;

            this.Notes = candidatesFormat.Notes;
            this.RecluterName = candidatesFormat.RecluterName;
            this.Tags = candidatesFormat.Tags;
            this.ContactSource= candidatesFormat.ContactSource;
            this.RejectionEmcor = candidatesFormat.RejectionEmcor;
            this.RejectionCandidate = candidatesFormat.RejectionCandidate;

            this.NameCreated    = candidatesFormat.NameCreated;
            this.NameModified = candidatesFormat.NameModified;
            this.DateCreated= candidatesFormat.DateCreated;
            this.DateModified= candidatesFormat.DateModified;
            this.Stages = null;
            this.QuestionDetails = null;
            this.Meetings = null;
        }
        public void SetId(int id)
        {
            this.CandidatesId = id;
        }
        [Key]
        [JsonPropertyName("candidatesId")]
        public int CandidatesId { get; set; }

        [JsonPropertyName("stages")]
        [ForeignKey("Stages")]
        public int StagesId { get; set; }
        [JsonPropertyName("vacancy")]
        [ForeignKey("Vacancy")]
        public int VacancyId { get; set; }
        [JsonPropertyName("name")]
        [MaxLength(100)]
        public string? Name { get; set; }
        [JsonPropertyName("age")]
        public int Age { get; set; }
        [JsonPropertyName("address")]
        [MaxLength(250)]
        public string? Address { get; set; }
        [JsonPropertyName("city")]
        [MaxLength(50)]
        public string? City { get; set; }
        [JsonPropertyName("country")]
        [MaxLength(50)]
        public string? Country { get; set; }
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


        [JsonPropertyName("notes")]
        [MaxLength(500)]
        public string Notes { get; set; }

        [JsonPropertyName("recluterName")]
        public string RecluterName { get; set; }


        [JsonPropertyName("tags")]
        public string Tags { get; set; }

        [JsonPropertyName("contactSource")]
        public string ContactSource { get; set; }

        [JsonPropertyName("rejectionEmcor")]
        public string RejectionEmcor { get; set; }

        [JsonPropertyName("rejectionCandidate")]
        public string RejectionCandidate { get; set; }



        public virtual ICollection<QuestionDetails> QuestionDetails { get; set; }
        public virtual ICollection<Meetings> Meetings { get; set; }
        public virtual Stages Stages { get; set; }
        public virtual Vacancy Vacancy { get; set; }
    }
}
