using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataContext
{
    public class EnumType
    {
        public void SetId(int id)
        {
            this.EnumTypeId = id;
        }
        [Key]
        [JsonPropertyName("enumTypeId")]
        public int EnumTypeId { get;  set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
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

        public virtual Questions? Questions { get; set; }
    }
}
