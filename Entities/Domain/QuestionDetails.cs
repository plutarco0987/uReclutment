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
    public class QuestionDetails
    {
        public QuestionDetails()
        {
            //this.Comments = new List<Comments>();
        }
        public void SetId(int id)
        {
            this.CandidatesId = id;
        }

        public QuestionDetails(QuestionDetailsFormat format)
        {
            this.QuestionDetailsId = format.QuestionDetailsId;

            this.CandidatesId = format.CandidatesId;
            this.QuestionsId=format.QuestionsId;
            this.Active= format.Active;
            this.DateCreated= format.DateCreated;
            this.DateModified= format.DateModified;
            this.Answer= format.Answer;
        }

        public void QuestionDetailsUpdate(QuestionDetailsFormat format)
        {            
            this.CandidatesId = format.CandidatesId;
            this.QuestionsId = format.QuestionsId;
            this.Active = format.Active;            
            this.DateModified = format.DateModified;
            this.Answer = format.Answer;
        }

        [JsonPropertyName("questionsId")]
        [ForeignKey("Questions")]
        public int QuestionsId { get; set; }
        [JsonPropertyName("candidatedId")]
        [ForeignKey("Candidates")]
        public int CandidatesId { get; set; }
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [Key]
        [JsonPropertyName("questionDetailsId")]
        public int QuestionDetailsId { get;  set; }
        
        public virtual Questions? Questions { get; set; }
        
        public virtual Candidates? Candidates { get; set; }
        public virtual ICollection<Comments>? Comments { get; set; }

        [JsonPropertyName("dateCreated")]
        [Required]
        public DateTime DateCreated { get; set; }
        [JsonPropertyName("dateModified")]
        [Required]
        public DateTime DateModified { get; set; }
        [JsonPropertyName("active")]

        [MaxLength(int.MaxValue)]
        public string? Answer { get; set; }
    }
}
