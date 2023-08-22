using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Entities.DataContext;

namespace Entities.Formats
{
    public class QuestionDetailsFormat
    {
        public QuestionDetailsFormat()
        {

        }
        public QuestionDetailsFormat(int questionDetailsId, string? candidateName, string? question, string? answer, bool active, int questionsId, int candidatesId, DateTime dateCreated, DateTime dateModified)
        {
            QuestionDetailsId = questionDetailsId;
            CandidateName = candidateName;
            Question = question;
            Answer = answer;
            Active = active;
            QuestionsId = questionsId;
            CandidatesId = candidatesId;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        [JsonPropertyName("questionDetailsId")]
        public int QuestionDetailsId { get; set; }

        [JsonPropertyName("candidateName")]
        [MaxLength(int.MaxValue)]
        public string? CandidateName { get; set; }
        [JsonPropertyName("question")]
        [MaxLength(int.MaxValue)]
        public string? Question { get; set; }
        [JsonPropertyName("answer")]
        [MaxLength(int.MaxValue)] 
        public string? Answer { get; set; }
        [JsonPropertyName("active")]        
        public bool Active { get; set; }
        [JsonPropertyName("questionsId")]        
        public int QuestionsId { get; set; }
        [JsonPropertyName("candidatedId")]        
        public int CandidatesId { get; set; }
                
        [JsonPropertyName("dateCreated")]        
        public DateTime DateCreated { get; set; }
        [JsonPropertyName("dateModified")]        
        public DateTime DateModified { get; set; }
        

        
    }
}
