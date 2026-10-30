using System.Reflection;
using ClinicFlow.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Api.Data ;

    public class ClinicDbContext(DbContextOptions options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClinicDbContext).Assembly);
        }

        public DbSet<Doctor> Doctors { get; set; }
    }