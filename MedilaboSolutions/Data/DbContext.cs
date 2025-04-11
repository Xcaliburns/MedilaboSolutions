using MedilaboSolutionsBack1.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace MedilaboSolutionsBack1.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(u => new { u.LoginProvider, u.ProviderKey }); 

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Adresse)
                .WithMany(a => a.Patients)
                .HasForeignKey(p => p.AdresseId)
                .OnDelete(DeleteBehavior.SetNull);
        }


        public DbSet<Patient> Patients { get; set; }
        public DbSet<Adresse> Adresses { get; set; } 
    }


}
