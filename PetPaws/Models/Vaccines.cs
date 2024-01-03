using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetPaws.Models
{
	public class Vaccines
	{
        [Key]
        public int Id { get; set; }

        public String Name { get; set; }

        [JsonIgnore]
        public List<Animal> Animals { get; set; } 

    }
}

