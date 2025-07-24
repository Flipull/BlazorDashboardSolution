using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BlazorDashboardApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new SubjectConfiguration());
            builder.ApplyConfiguration(new DataConfiguration());
            builder.ApplyConfiguration(new TranscriptConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());
        }

        public DbSet<Subject> Subject { get; set; }
        public DbSet<Data> Data { get; set; }
        public DbSet<Transcript> Transcript { get; set; }
        public DbSet<Tag> Tag { get; set; }

    }
}
