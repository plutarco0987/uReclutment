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
    public class Comments
    {
        public Comments()
        {

        }
        public Comments(CommentsFormat comments)
        {
            this.CommentsId = comments.CommentsId;
            this.QuestionDetailsId = comments.QuestionDetailsId;
            this.Value = comments.Value;
            this.Active = comments.Active;
            this.NameCreated = comments.NameCreated;
            this.NameModified = comments.NameModified;
            this.DateCreated = comments.DateCreated;
            this.DateModified = comments.DateModified;
            this.QuestionDetails = null;
        }
        public void CommentsFormat(CommentsFormat comments)
        {            
            this.QuestionDetailsId = comments.QuestionDetailsId;
            this.Value = comments.Value;
            this.Active = comments.Active;            
            this.NameModified = comments.NameModified;                                    
        }
        public void SetId(int id)
        {
            this.CommentsId = id;
        }
        [Key]
        [JsonPropertyName("commentsId")]
        public int CommentsId { get;  set; }
        [JsonPropertyName("questionDetailsId")]
        [ForeignKey("QuestionDetails")]
        public int QuestionDetailsId { get; set; }
        [JsonPropertyName("value")]
        [MaxLength(int.MaxValue)]
        public string? Value { get; set; }
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

        public virtual QuestionDetails? QuestionDetails { get; set; }
    }
}
