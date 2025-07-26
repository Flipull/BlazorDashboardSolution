using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardApp.Data
{
    class DatumConfiguration : IEntityTypeConfiguration<Datum>
    {
        public void Configure(EntityTypeBuilder<Datum> builder)
        {
            builder.ToTable("Datum");

            builder.HasKey(k => k.Id);
            builder.Property(f => f.Filetype)
                   .IsRequired()
                   .HasMaxLength(16);
            builder.Property(f => f.Filename)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.HasOne(f => f.Subject)
                   .WithMany(s => s.Datum)
                   .HasForeignKey(f => f.SubjectId)
                   .OnDelete(DeleteBehavior.NoAction);


            builder.Property(t => t.IsDeleted)
                    .HasDefaultValue(false)
                   .IsRequired();
            builder.Property(t => t.DeletedDate);
            builder.Property(t => t.DeletedByUserId)
                    .HasMaxLength(450); // Matches Identity default
            builder.HasOne(t => t.DeletedByUser)
                   .WithMany()
                   .HasForeignKey(t => t.DeletedByUserId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}