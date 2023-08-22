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
    public class CandidatesFormat
    {
        public CandidatesFormat()
        {

        }
        public CandidatesFormat(int candidatesId, int stagesId, int vacancyId, string? name, int age, string? address, string? city, string? country, bool active, string? nameCreated, DateTime dateCreated, string? nameModified, DateTime dateModified,string notes,string recluterName,string tags,string contactSource,string rejectEmcor,string rejectCandidate)
        {
            CandidatesId = candidatesId;
            StagesId = stagesId;
            VacancyId = vacancyId;
            Name = name;
            Age = age;
            Address = address;
            City = city;
            Country = country;
            Active = active;
            NameCreated = nameCreated;
            DateCreated = dateCreated;
            NameModified = nameModified;
            DateModified = dateModified;
            Notes = notes;
            RecluterName = recluterName;
            Tags= tags;
            ContactSource = contactSource;
            RejectionCandidate= rejectCandidate;
            RejectionEmcor = rejectEmcor;
        }

        [JsonPropertyName("candidatesId")]
        public int CandidatesId { get; set; }

        [JsonPropertyName("stages")]        
        public int StagesId { get; set; }
        [JsonPropertyName("vacancy")]        
        public int VacancyId { get; set; }
        [JsonPropertyName("name")]
        
        public string? Name { get; set; }
        [JsonPropertyName("age")]
        public int Age { get; set; }
        [JsonPropertyName("address")]
        public string? Address { get; set; }
        [JsonPropertyName("city")]
        public string? City { get; set; }
        [JsonPropertyName("country")]
        public string? Country { get; set; }
        [JsonPropertyName("active")]
        public bool Active { get; set; }


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
