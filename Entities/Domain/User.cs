using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public class User
    {        
        public User()
        {           
        }
        public void SetId(int id)
        {
            this.UserId = id;
        }
        [Key]
        [JsonPropertyName("UserId")]
        public int UserId { get; set; }

        [JsonPropertyName("UserName")]
        [Required]
        [MaxLength(int.MaxValue)]
        public string UserName { get; set; }

        [JsonPropertyName("Password")]
        [Required]
        [MaxLength(int.MaxValue)]
        public string Password { get; set; }

        [JsonPropertyName("Email")]
        [Required]
        [MaxLength(int.MaxValue)]
        public string Email { get; set; }
    }
}
