using Microsoft.EntityFrameworkCore;
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
    public class Vacancy
    {
        public Vacancy()
        {
            this.Requirements = new List<Requirements>();
            this.Questions = new List<Questions>();
            this.Candidates = new List<Candidates>();
        }
        public void SetId(int id)
        {
            this.VacancyId = id;
        }
        public Vacancy(VacancyFormat vf)
        {
            this.CustomersId = vf.CustomersId;
            this.Name = vf.Name;
            this.Description = vf.Description;
            this.Responsabilitys = vf.Responsabilitys;
            this.Active = vf.Active;
            this.NamePosition = vf.NamePosition;

            this.NameCreated = vf.NameCreated;
            this.NameModified = vf.NameModified;
            this.DateCreated = vf.DateCreated;
            this.DateModified = vf.DateModified;
            this.ContractType = vf.ContractType;
            this.Requirements = new List<Requirements>();
            this.Questions = new List<Questions>();
            this.Candidates = new List<Candidates>();
            this.Customers = new Customers();

            this.Status = vf.Status;
            this.Departament = vf.Departament;
        }

        [Key]
        [JsonPropertyName("VacancyId")]
        public int VacancyId { get; set; }

        [ForeignKey("Customers")]
        public int CustomersId { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(250)]
        public string? Description { get; set; }
        [MaxLength(100)]
        public string? NamePosition { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 0)]
        public string? Responsabilitys { get; set; }
        public bool Active { get; set; }        

        [Required]
        [MaxLength(100)]
        public string? NameCreated { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        [MaxLength(100)]
        public string? NameModified { get; set; }
        [Required]
        public DateTime DateModified { get; set; }

        [MaxLength(100)]
        public string ContractType { get; set; }
        [MaxLength(100)]
        public string Status { get; set; }
        [MaxLength(100)]
        public string Departament { get; set; }

        public virtual ICollection<Requirements> Requirements { get; set; }
        public virtual ICollection<Questions> Questions { get; set; }
        public virtual ICollection<Candidates> Candidates { get; set; }

        public virtual Customers? Customers { get; set; }

    }
}
