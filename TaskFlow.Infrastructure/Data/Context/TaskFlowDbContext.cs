using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;

namespace TaskFlow.Infrastructure.Data.Context;

public class TaskFlowDbContext : DbContext
{
    public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options)
        : base(options) { }

    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<ProjectManager> ProjectManagers { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<Status> Statuses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entities
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProjectNumber).IsRequired();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");

            entity
                .HasOne(e => e.Status)
                .WithMany(s => s.Projects)
                .HasForeignKey(e => e.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(e => e.Customer)
                .WithMany(c => c.Projects)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(e => e.ProjectManager)
                .WithMany(pm => pm.Projects)
                .HasForeignKey(e => e.ProjectManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(e => e.Service)
                .WithMany(s => s.Projects)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<ProjectManager>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.HourlyRate).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();
        });
    }
}
