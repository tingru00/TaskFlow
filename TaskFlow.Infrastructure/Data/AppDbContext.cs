using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TaskFlow.Domain.Entities;

// DbContext används för att prata med databasen via EF Core.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Tabell för kategorier.
    public DbSet<Category> Categories => Set<Category>();

    // Tabell för uppgifter.
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // En kategori kan ha många uppgifter.
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Tasks)
            .WithOne(t => t.Category!)
            .HasForeignKey(t => t.CategoryId);

        base.OnModelCreating(modelBuilder);
    }
}
