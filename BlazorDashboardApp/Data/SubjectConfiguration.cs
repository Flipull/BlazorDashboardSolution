using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardApp.Data
{
    class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("Subjects");

            builder.HasKey(k => k.Id);
            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(64);
            builder.Property(f => f.Description)
                .IsRequired()
                .HasMaxLength(512);
            builder.Property(f => f.Photofile)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}