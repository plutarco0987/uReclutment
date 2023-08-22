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
    public class Customers
    {
        public Customers()
        {
            this.Vacancies = new List<Vacancy>();
        }
        public void SetId(int id)
        {
            this.CustomersId = id;
        }
        [Key]
        [JsonPropertyName("customersId")]
        public int CustomersId { get; set; }
        [JsonPropertyName("name")]
        [MaxLength(100)]
        public string? Name { get; set; }
        [JsonPropertyName("address")]
        [MaxLength(250)]
        public string? Address { get; set; }
        [JsonPropertyName("city")]
        [MaxLength(50)]
        public string? City { get; set; }
        [JsonPropertyName("country")]
        [MaxLength(50)]
        public string? Country { get; set; }
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

        public virtual ICollection<Vacancy>? Vacancies { get; set; }
    }
}
