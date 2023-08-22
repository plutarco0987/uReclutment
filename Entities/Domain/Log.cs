using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataContext
{
    public class Log
    {
        public Log(int logId, string? errorMessage, string? error, DateTime errorDate, bool active)
        {
            LogId = logId;
            ErrorMessage = errorMessage;
            Error = error;
            ErrorDate = errorDate;
            Active = active;
        }

        public void SetId(int id)
        {
            this.LogId = id;
        }
        [Key]
        [JsonPropertyName("logId")]
        public int LogId { get;  set; }
        [MaxLength(int.MaxValue)]
        [JsonPropertyName("errorMessage")]
        public string? ErrorMessage { get; set; }
        [JsonPropertyName("error")]
        [MaxLength(int.MaxValue)]
        public string? Error { get; set; }
        [JsonPropertyName("errorDate")]
        [Required]
        public DateTime ErrorDate { get; set; }
        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }
}
