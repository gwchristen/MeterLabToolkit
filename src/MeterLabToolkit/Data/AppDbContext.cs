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
    public DbSet<OpCo> OpCos { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<PurchaseCode> PurchaseCodes { get; set; }

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

        // Seed data
        modelBuilder.Entity<OpCo>().HasData(
            new OpCo { Id = 1, Code = "Ohio", Description = "Ohio Power Company" },
            new OpCo { Id = 2, Code = "I&M", Description = "Indiana & Michigan Power" }
        );

        modelBuilder.Entity<Status>().HasData(
            new Status { Id = 1, Code = "RC", Description = "Received" },
            new Status { Id = 2, Code = "PD", Description = "Pending" },
            new Status { Id = 3, Code = "CM", Description = "Complete" },
            new Status { Id = 4, Code = "RJ", Description = "Rejected" }
        );

        modelBuilder.Entity<ManufacturerCode>().HasData(
            new ManufacturerCode { Id = 1, Code = "GE", IndustryCode = "G", Name = "General Electric" },
            new ManufacturerCode { Id = 2, Code = "IT", IndustryCode = "I", Name = "Itron" },
            new ManufacturerCode { Id = 3, Code = "LG", IndustryCode = "L", Name = "Landis+Gyr" }
        );

        modelBuilder.Entity<DeviceCode>().HasData(
            new DeviceCode { Id = 1, Code = "NMD06", Description = "Network Meter Device", DeviceType = "Meter" },
            new DeviceCode { Id = 2, Code = "CF62K", Description = "Current Former 62K", DeviceType = "Transformer" }
        );

        modelBuilder.Entity<LookupCode>().HasData(
            new LookupCode { Id = 1, Code = "1N", Description = "Standard Network" },
            new LookupCode { Id = 2, Code = "2N", Description = "Alternate Network" }
        );

        modelBuilder.Entity<PurchaseCode>().HasData(
            new PurchaseCode { Id = 1, Code = "PUR01", Description = "Standard Purchase" },
            new PurchaseCode { Id = 2, Code = "PUR02", Description = "Emergency Purchase" }
        );
    }
}
