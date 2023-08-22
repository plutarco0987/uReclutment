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
    public class Files
    {
        public Files()
        {

        }          
        [Key]
        [JsonPropertyName("filesId")]
        public int FilesId { get;  set; }

        [JsonPropertyName("candidatesId")]
        [ForeignKey("CandidatesId")]
        public int CandidatesId { get; set; }
        [JsonPropertyName("name")]        
        public string? Name { get; set; }
        [JsonPropertyName("active")]
        public bool Active { get; set; }
        [JsonPropertyName("path")]
        public string Path { get; set; }

        
    }
}
