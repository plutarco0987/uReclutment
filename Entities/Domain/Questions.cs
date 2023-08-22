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
    public class Questions
    {
        public Questions()
        {
            this.EnumType = new EnumType();
            this.QuestionDetails = new List<QuestionDetails>();
        }
        public Questions(QuestionsFormat format)
        {
            QuestionsId= format.QuestionsId;
            EnumTypeId= format.EnumTypeId;
            VacancyId= format.VacancyId;
            Question= format.Question;
            Required= format.Required;
            MaxLength= format.MaxLength;
            Active= format.Active;
            NameCreated= format.NameCreated;
            DateCreated= format.DateCreated;
            NameModified= format.NameModified;
            DateModified= format.DateModified;
        }
        public void Format(QuestionsFormat format)
        {            
            EnumTypeId = format.EnumTypeId;
            VacancyId = format.VacancyId;
            Question = format.Question;
            Required = format.Required;
            MaxLength = format.MaxLength;
            Active = format.Active;
            NameCreated = format.NameCreated;
            DateCreated = format.DateCreated;
            NameModified = format.NameModified;            
        }
        public Questions(int questionsId, int enumTypeId, int vacancyId, string? question, bool required, int maxLength, bool active, string? nameCreated, DateTime dateCreated, string? nameModified, DateTime dateModified, Vacancy? vacancy, EnumType? enumType, ICollection<QuestionDetails> questionDetails)
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
            Vacancy = vacancy;
            EnumType = enumType;
            QuestionDetails = questionDetails;
        }

        public void SetId(int id)
        {
            this.QuestionsId = id;
        }
        [Key]
        [JsonPropertyName("questionsId")]
        public int QuestionsId { get;  set; }
        [JsonPropertyName("enumTypeId")]
        [ForeignKey("EnumType")]
        public int EnumTypeId { get; set; }
        [JsonPropertyName("vacancyId")]
        [ForeignKey("Vacancy")]
        public int VacancyId { get; set; }
        [JsonPropertyName("question")]
        [MaxLength(250)]
        public string? Question { get; set; }
        [JsonPropertyName("required")]
        public bool Required { get; set; }
        [JsonPropertyName("maxLength")]
        public int MaxLength { get; set; }
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


        public virtual Vacancy? Vacancy { get; set; }

        public virtual EnumType? EnumType { get; set; }

        public virtual ICollection<QuestionDetails> QuestionDetails { get; set; }

    }
}
