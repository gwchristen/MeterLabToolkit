using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using MeterLabToolkit.Models;

namespace MeterLabToolkit.Data;

public class AppDbContext : DbContext
{
    public DbSet<CreatedHistory> CreatedHistories { get; set; }
    public DbSet<CellularRegistration> CellularRegistrations { get; set; }
    public DbSet<RmaEntry> RmaEntries { get; set; }
    public DbSet<LookupCode> LookupCodes { get; set; }
    public DbSet<ManufacturerCode> ManufacturerCodes { get; set; }
    public DbSet<DeviceCode> DeviceCodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Get the application data directory
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbPath = Path.Combine(appDataPath, "MeterLabToolkit", "meterlab.db");
            
            // Ensure directory exists
            var directory = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entity properties as needed
        modelBuilder.Entity<CreatedHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitCost).HasPrecision(18, 2);
        });

        modelBuilder.Entity<CellularRegistration>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<RmaEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<LookupCode>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<ManufacturerCode>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<DeviceCode>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}
