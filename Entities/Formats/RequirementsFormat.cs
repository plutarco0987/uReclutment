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
    public class RequirementsFormat
    {
        public RequirementsFormat()
        {

        }

        public RequirementsFormat(int requirementsId, int vacancyId, string? name, string? description, bool required, string? ageExperience, bool active, bool benefits, string? nameCreated, DateTime dateCreated, string? nameModified, DateTime dateModified)
        {
            RequirementsId = requirementsId;
            VacancyId = vacancyId;
            Name = name;
            Description = description;
            Required = required;
            AgeExperience = ageExperience;
            Active = active;
            Benefits = benefits;
            NameCreated = nameCreated;
            DateCreated = dateCreated;
            NameModified = nameModified;
            DateModified = dateModified;
        }

        [JsonPropertyName("requirementsId")]
        public int RequirementsId { get; set; }
        [JsonPropertyName("vacancyId")]        
        public int VacancyId { get; set; }
        [JsonPropertyName("name")]        
        public string? Name { get; set; }
        [JsonPropertyName("description")]        
        public string? Description { get; set; }
        [JsonPropertyName("required")]
        public bool Required { get; set; }
        [JsonPropertyName("ageExperience")]        
        public string? AgeExperience { get; set; }
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("benefits")]
        public bool Benefits { get; set; }

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
