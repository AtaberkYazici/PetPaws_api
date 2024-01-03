using System;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PetPaws.Models;

namespace PetPaws.Repositories
{
	public class ApiDbContext : DbContext
    {
		public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
		{
		}
        public DbSet<User> Users { get; set; }
		public DbSet <Animal> Animals { get; set; }
        public DbSet<Vaccines> Vaccines { get; set; }

       
    }
}

