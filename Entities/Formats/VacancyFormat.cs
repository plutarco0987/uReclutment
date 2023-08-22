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
    public class VacancyFormat
    {
        
        public VacancyFormat()
        {
        }
        public VacancyFormat(Vacancy vacancy,IEnumerable<IdClass> values)
        {
            this.VacancyId = vacancy.VacancyId;
            this.CustomersId = vacancy.CustomersId;
            this.Name = vacancy.Name;
            this.Description = vacancy.Description;
            this.NamePosition = vacancy.NamePosition;
            this.Responsabilitys = vacancy.Responsabilitys;
            this.Active = vacancy.Active;

            this.Questions = new List<string>();
            foreach (Questions item in vacancy.Questions)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                if(item.Active)
                    this.Questions.Add(item.Question);
#pragma warning restore CS8604 // Possible null reference argument.
            }
            this.QuestionsString= string.Join(",",this.Questions);

            this.NameCreated = vacancy.NameCreated;
            this.DateCreated = vacancy.DateCreated;
            this.NameModified = vacancy.NameModified;
            this.DateModified = vacancy.DateModified;


            var x = values.ToList().Find(x => x.Id == CustomersId);
            if (x!=null)
            {
                this.CustomerName = x.Name;
            }
            this.ContractType = vacancy.ContractType;
            this.Status = vacancy.Status;
            this.Departament= vacancy.Departament;
        }
        [JsonPropertyName("vacancyId")]
        public int VacancyId { get; set; }
        [JsonPropertyName("customersId")]
        public int CustomersId { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("namePosition")]
        public string? NamePosition { get; set; }
        [JsonPropertyName("responsabilitys")]
        public string? Responsabilitys { get; set; }
        [JsonPropertyName("active")]
        public bool Active { get; set; }
        [JsonPropertyName("nameCreated")]
        public string? NameCreated { get; set; }
        [JsonPropertyName("dateCreated")]
        public DateTime DateCreated { get; set; }
        [JsonPropertyName("nameModified")]
        public string? NameModified { get; set; }
        [JsonPropertyName("dateModified")]
        public DateTime DateModified { get; set; }
        [JsonPropertyName("questions")]
        public List<string> Questions { get; set; }
        [JsonPropertyName("questionsString")]
        public string? QuestionsString { get; set; }
        [JsonPropertyName("customerName")]
        public string? CustomerName { get; set; }
        [JsonPropertyName("contractType")]
        public string? ContractType { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [JsonPropertyName("departament")]
        public string? Departament { get; set; }
    }   
}