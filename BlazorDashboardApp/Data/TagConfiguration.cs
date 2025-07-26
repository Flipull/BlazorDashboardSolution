using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardApp.Data
{
    class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");

            builder.HasKey(k => k.Id);
            builder.Property(t => t.TagString)
                   .IsRequired()
                   .HasMaxLength(32);
            builder.HasOne(t => t.Datum)
                   .WithMany(f => f.Tags)
                   .HasForeignKey(t => t.DatumId)
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