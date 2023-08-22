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
    public class QuestionsFormat
    {
        public QuestionsFormat()
        {

        }

        public QuestionsFormat(int questionsId, int enumTypeId, int vacancyId, string? question, bool required, int maxLength, bool active, string? nameCreated, DateTime dateCreated, string? nameModified, DateTime dateModified)
        {
            QuestionsId = questionsId;
            EnumTypeId = enumTypeId;
            VacancyId = vacancyId;
            Question = question;
            Required = required;
            MaxLength = maxLength;
            Active = active;
            NameCreated = nameCreated;
            DateCreated = dateCreated;
            NameModified = nameModified;
            DateModified = dateModified;
        }

        [JsonPropertyName("questionsId")]
        public int QuestionsId { get; set; }
        [JsonPropertyName("enumTypeId")]        
        public int EnumTypeId { get; set; }
        [JsonPropertyName("vacancyId")]        
        public int VacancyId { get; set; }
        [JsonPropertyName("question")]        
        public string? Question { get; set; }
        [JsonPropertyName("required")]
        public bool Required { get; set; }
        [JsonPropertyName("maxLength")]
        public int MaxLength { get; set; }
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
