using System;
using System.ComponentModel.DataAnnotations;


namespace PetPaws.Models
{
	public class User
	{
		[Key]
		public int UserId { get; set; }

        public String Username { get; set; }

        public String Email { get; set; }

        public String Phone { get; set; } 

        public String? Password { get; set; } 

        public String? imagePath { get; set; }

        public List<Animal> Animals { get; set; }

    }
}

