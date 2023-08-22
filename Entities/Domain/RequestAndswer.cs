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
    public class RequestAndswer
    {
        public RequestAndswer()
        {            
        }

        public RequestAndswer(string andswers, string file, int vacancyId)
        {
            Andswers = andswers;
            File = file;
            VacancyId = vacancyId;
        }

        [JsonPropertyName("andswers")]
        public string Andswers { get; set; }
        [JsonPropertyName("file")]        
        public string File { get; set; }        

        [JsonPropertyName("vacancyId")]
        public int VacancyId { get; set; }

    }
}
