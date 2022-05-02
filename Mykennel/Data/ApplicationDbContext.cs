using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mykennel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mykennel.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
    }
        // Ide kerülnek az osztályokhoz tartozó gyűjtemények, hogy aztán el tudjuk érni a context objektumból
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Dog> Dogs { get; set; }
        public DbSet<Kennel> Kennels { get; set; }
        public DbSet<Litter> Litters { get; set; }
        public DbSet<Puppy> Puppies { get; set; }
    }
}
