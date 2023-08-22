using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataContext
{
    public class Stages
    {
        [Key]
        [JsonPropertyName("stagesId")]
        public int StagesId { get;  set; }
        [JsonPropertyName("name")]
        [MaxLength(100)]
        public string? Name { get; set; }
        [JsonPropertyName("color")]
        [Required]
        [MaxLength(20)]
        public string? Color { get; set; }
        [JsonPropertyName("order")]
        [Required]
        public int Order { get; set; }
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
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        public void SetId(int id)
        {
            this.StagesId = id;
        }

        public virtual Candidates? Candidates { get; set; }
    }
}
