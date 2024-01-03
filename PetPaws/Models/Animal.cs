using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetPaws.Models
{
	public class Animal
    {
        [Key]
        public int AnimalId { get; set; }

        public String Name { get; set; }

        public String Type { get; set; }

        public int Age { get; set; }

        public String? ExtraExplanations { get; set; }

        public String? imagePath { get; set; }

        public String UserPhone { get; set; }

        public String UserName { get; set; }

        public List<Vaccines>? Vaccines { get; set; } = new List<Vaccines>();

        public List<User>? Users { get; set; } = new List<User>();

    }
}

