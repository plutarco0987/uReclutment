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
    public class Requirements
    {
        public Requirements()
        {

        }
        public Requirements(RequirementsFormat requirementsFormat)
        {
            this.RequirementsId= requirementsFormat.RequirementsId;
            this.VacancyId= requirementsFormat.VacancyId;
            this.Name= requirementsFormat.Name;
            this.Description= requirementsFormat.Description;
            this.Required= requirementsFormat.Required;
            this.AgeExperience= requirementsFormat.AgeExperience;
            this.Active= requirementsFormat.Active;
            this.NameCreated= requirementsFormat.NameCreated;
            this.DateCreated = requirementsFormat.DateCreated;
            this.NameModified= requirementsFormat.NameModified;
            this.DateModified= requirementsFormat.DateModified;
            this.Benefits = requirementsFormat.Benefits;
            this.Vacancy = null;
        }

        public void SetId(int id)
        {
            this.RequirementsId = id;
        }
        [Key]
        [JsonPropertyName("requirementsId")]
        public int RequirementsId { get;  set; }
        [JsonPropertyName("vacancyId")]
        [ForeignKey("Vacancy")]
        public int VacancyId { get; set; }
        [JsonPropertyName("name")]
        [MaxLength(100)]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        [MaxLength(250)]
        public string? Description { get; set; }
        [JsonPropertyName("required")]
        public bool Required { get; set; }
        [JsonPropertyName("ageExperience")]
        [MaxLength(50)]
        public string? AgeExperience { get; set; }
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        


        [JsonPropertyName("NameCreated")]
        [MaxLength(100)]
        public string? NameCreated { get; set; }
        [JsonPropertyName("dateCreated")]
        public DateTime DateCreated { get; set; }
        [JsonPropertyName("NameModified")]
        [MaxLength(100)]
        public string? NameModified { get; set; }
        [JsonPropertyName("dateModified")]
        public DateTime DateModified { get; set; }
        [JsonPropertyName("benefits")]
        public bool Benefits { get; set; }


        public virtual Vacancy? Vacancy { get; set; }

    }
}
