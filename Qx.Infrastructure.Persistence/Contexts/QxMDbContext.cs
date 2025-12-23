using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Qx.Infrastructure.Persistence.Entities;

namespace Qx.Infrastructure.Persistence.Contexts;

public class QxMDbContext : DbContext
{
    public QxMDbContext(DbContextOptions<QxMDbContext> options)
        : base(options)
    {
    }

    public DbSet<InventoryEntity> Inventory { get; set; } = null!;
    public DbSet<ConsumableEntity> Consumable { get; set; } = null!;
    public DbSet<ConsumableTypeEntity> ConsumableType { get; set; } = null!;
    public DbSet<ConsumableColumnEntity> ConsumableColumn { get; set; } = null!;
    public DbSet<LocationEntity> Location { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        ConfigureInventory(modelBuilder);
        ConfigureConsumable(modelBuilder);
        ConfigureConsumableColumn(modelBuilder);
        ConfigureLocation(modelBuilder);
    }

    private static void ConfigureInventory(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<InventoryEntity>();
        
        entity.ToTable("inventory");
        
        // Primary Key
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.CreatedAtDate)
            .IsRequired();
        entity.Property(e => e.UpdatedAtDate)
            .IsRequired();
        entity.Property(e => e.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();
        
        // Foreign Relationships
        entity.HasMany(e => e.Consumables)
            .WithOne(c => c.Inventory)
            .HasForeignKey(c => c.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureConsumable(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ConsumableEntity>();
        
        entity.ToTable("consumable");
        
        entity.HasKey(e => e.Id);
        entity.Property(e => e.TypeId)
            .IsRequired()
            .HasMaxLength(100);
        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);
        entity.Property(e => e.State)
            .IsRequired();
        entity.Property(e => e.Uses)
            .IsRequired();
        entity.Property(e => e.IsReusable)
            .IsRequired();
        entity.Property(e => e.MaxUses);
        entity.Property(e => e.Barcode)
            .HasMaxLength(200);
        
        // Optimized indexing
        entity.HasIndex(e => new { e.InventoryId, e.Name }); // consumables with name in inventory with id
        entity.HasIndex(e => new { e.InventoryId, e.TypeId }); // consumables of type id in inventory with id
        entity.HasIndex(e => e.Barcode); // consumables with a specific barcode
        entity.HasIndex(e => e.LocationId)
            .IsUnique();
        
        // Foreign Relationships
        entity.HasOne(e => e.Inventory)
            .WithMany(i => i.Consumables)
            .HasForeignKey(e => e.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);
        entity.HasOne(e => e.Type)
            .WithMany()
            .HasForeignKey(e => e.TypeId)
            .OnDelete(DeleteBehavior.Cascade);
        entity.HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.Cascade);
        entity.HasMany(e => e.Columns)
            .WithOne(c => c.Consumable)
            .HasForeignKey(c => c.ConsumableId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureConsumableColumn(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ConsumableColumnEntity>();
        
        entity.ToTable("consumable_column");
        
        // Primary Key
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Name)
            .HasMaxLength(8)
            .IsRequired();
        entity.Property(e => e.IsReusable)
            .IsRequired();
        entity.Property(e => e.MaxUses);
        entity.Property(e => e.Uses)
            .IsRequired();
        entity.Property(e => e.IsSealed)
            .IsRequired();
        
        // Optimized indexing
        entity.HasIndex(e => new {e.ConsumableId, e.Name}) // consumable column by name in a consumable with id
            .IsUnique();
        
        // TODO: find a better way (we are fixed to 8 channels so can use 8 columns)
        // map IList<double> to JSONB arrays (postgres-specific)
        var doubleListConverter = new ValueConverter<IList<double>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<IList<double>>(v, (JsonSerializerOptions?)null) 
                 ?? new List<double>());
        entity.Property(e => e.VolumeCapacityUl)
            .HasConversion(doubleListConverter)
            .HasColumnType("jsonb")
            .IsRequired();
        entity.Property(e => e.CurrentVolumeUl)
            .HasConversion(doubleListConverter)
            .HasColumnType("jsonb")
            .IsRequired();
    }

    private static void ConfigureLocation(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<LocationEntity>();
        
        entity.ToTable("location");
        
        // Primary Key
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);
        entity.Property(e => e.XUs)
            .IsRequired();
        entity.Property(e => e.YUs)
            .IsRequired();
        entity.Property(e => e.ZUs)
            .IsRequired();
        entity.Property(e => e.Frame)
            .IsRequired()
            .HasMaxLength(50);
        
        // Optimized indexing
        entity.HasIndex(e => e.Name) // locations by name (locations have unique names)
            .IsUnique();
    }

    private static void ConfigureConsumableType(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<ConsumableTypeEntity>();
        
        entity.ToTable("consumable_type");
        
        // Primary Key
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);
        entity.Property(e => e.Version)
            .IsRequired()
            .HasMaxLength(100);
        entity.Property(e => e.Rows)
            .IsRequired();
        entity.Property(e => e.Columns)
            .IsRequired();
        entity.Property(e => e.GeometryJson)
            .HasMaxLength(1000)
            .IsRequired();
        entity.Property(e => e.DefaultIsReusable)
            .IsRequired();
        entity.Property(e => e.DefaultMaxUses);
        
        // Optimized indexing
        entity.HasIndex(e => e.Name)
            .IsUnique();
        entity.HasIndex(e => new { e.Name, e.Version })
            .IsUnique();
    }
}