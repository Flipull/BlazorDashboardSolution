using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardApp.Data
{
    class TranscriptConfiguration : IEntityTypeConfiguration<Transcript>
    {
        public void Configure(EntityTypeBuilder<Transcript> builder)
        {
            builder.ToTable("Transcripts");

            builder.HasKey(k => k.Id);
            builder.Property(t => t.TranscriptString)
                   .IsRequired()
                   .HasMaxLength(128);
            builder.HasOne(t => t.Data)
                   .WithMany(f => f.Transcript)
                   .HasForeignKey(t => t.DataId)
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