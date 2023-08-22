using Entities.Formats;
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
    public class MigrationVacancy
    {
        public MigrationVacancy(List<VacancyFormat> vacancys)
        {
            Vacancys = vacancys;
        }

        [JsonPropertyName("vacancys")]
        public List<VacancyFormat> Vacancys { get; set; }

 

    }
}
