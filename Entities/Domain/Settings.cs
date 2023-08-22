using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataContext
{
    public class Settings
    {
        public void SetId(int id)
        {
            this.SettingsId = id;
        }
        [Key]
        [JsonPropertyName("SettingsId")]        
        public int SettingsId { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
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
    }
}
