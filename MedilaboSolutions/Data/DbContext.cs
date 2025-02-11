using MedilaboSolutionsBack1.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace MedilaboSolutionsBack1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Patient> Patients
        {
            get; set;
        }
    }
}
