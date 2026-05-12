using JobTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<IdentityUser> //identity is used because it provites built in auth fucntions
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<JobApplication>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.Company).IsRequired().HasMaxLength(100);
            e.Property(a => a.Role).IsRequired().HasMaxLength(100);
            e.Property(a => a.JobUrl).HasMaxLength(200);
            e.Property(a => a.Status).HasConversion<string>();
        });
    }

}