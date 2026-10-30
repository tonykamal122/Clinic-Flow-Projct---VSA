using ClinicFlow.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicFlow.Api.Data.Configurations ;

    public class DoctorConfigurations : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.DoctorId);

            builder.Property(d => d.DoctorName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.DoctorSpecialization)
                .IsRequired()
                .HasMaxLength(100);
        }
    }