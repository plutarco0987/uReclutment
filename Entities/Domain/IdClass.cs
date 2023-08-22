using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataContext
{
    public class IdClass
    {
        public IdClass()
        {            
        }

        public IdClass(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
                
        public string Name { get; set; }
        
    }
}
