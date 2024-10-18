using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TrailerCompanyBackend.Models
{
    public partial class TrailerCompanyDbContext : DbContext
    {
        public TrailerCompanyDbContext()
        {
        }

        public TrailerCompanyDbContext(DbContextOptions<TrailerCompanyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accessory> Accessories { get; set; }
        public virtual DbSet<AccessorySize> AccessorySizes { get; set; }
        public virtual DbSet<AlertRecord> AlertRecords { get; set; }
    
        public virtual DbSet<DisposalRecord> DisposalRecords { get; set; }
        public virtual DbSet<InventoryRecord> InventoryRecords { get; set; }
        public virtual DbSet<RepairRecord> RepairRecords { get; set; }
        public virtual DbSet<RestockRecord> RestockRecords { get; set; }
        public virtual DbSet<SalesRecord> SalesRecords { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Trailer> Trailers { get; set; }
        public virtual DbSet<TransferRecord> TransferRecords { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<OperationLog> OperationLogs { get; set; }  // 日志表
        public virtual DbSet<AssemblyRecord> AssemblyRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Accessory
            modelBuilder.Entity<Accessory>(entity =>
            {
                entity.ToTable("accessories");

                entity.Property(e => e.AccessoryId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("accessory_id");
                entity.Property(e => e.AccessoryType)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("accessory_type");
                entity.Property(e => e.Description)
                    .HasColumnType("VARCHAR(200)")
                    .HasColumnName("description");
                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Store).WithMany(p => p.Accessories)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // AccessorySize
            modelBuilder.Entity<AccessorySize>(entity =>
            {
                entity.HasKey(e => e.SizeId);

                entity.ToTable("accessory_sizes");

                entity.Property(e => e.SizeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("size_id");
                entity.Property(e => e.AccessoryId).HasColumnName("accessory_id");
                entity.Property(e => e.DetailedSpecification)
                    .HasColumnType("VARCHAR(200)")
                    .HasColumnName("detailed_specification");
                entity.Property(e => e.SizeName)
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("size_name");
                entity.Property(e => e.ThresholdQuantity).HasColumnName("threshold_quantity");

                entity.HasOne(d => d.Accessory).WithMany(p => p.AccessorySizes)
                    .HasForeignKey(d => d.AccessoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // AlertRecord
            modelBuilder.Entity<AlertRecord>(entity =>
            {
                entity.HasKey(e => e.AlertId);

                entity.ToTable("alert_records");

                entity.Property(e => e.AlertId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("alert_id");
                entity.Property(e => e.AccessorySizeId).HasColumnName("accessory_size_id");
                entity.Property(e => e.AlertTime)
                    .HasColumnType("DATETIME")
                    .HasColumnName("alert_time");
                entity.Property(e => e.AlertType)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("alert_type");
                entity.Property(e => e.CurrentQuantity).HasColumnName("current_quantity");
                entity.Property(e => e.ThresholdQuantity).HasColumnName("threshold_quantity");
                entity.Property(e => e.TrailerId).HasColumnName("trailer_id");

                entity.HasOne(d => d.AccessorySize).WithMany(p => p.AlertRecords).HasForeignKey(d => d.AccessorySizeId);

                entity.HasOne(d => d.Trailer).WithMany(p => p.AlertRecords).HasForeignKey(d => d.TrailerId);
            });

            // InventoryRecord (and other records should follow similar)
            modelBuilder.Entity<InventoryRecord>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.ToTable("inventory_records");

                entity.Property(e => e.RecordId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("record_id");
                entity.Property(e => e.AccessorySizeId).HasColumnName("accessory_size_id");
                entity.Property(e => e.OperationTime)
                    .HasColumnType("DATETIME")
                    .HasColumnName("operation_time");
                entity.Property(e => e.OperationType)
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("operation_type");
                entity.Property(e => e.Operator)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("operator");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.TargetStoreId).HasColumnName("target_store_id");
                entity.Property(e => e.TrailerId).HasColumnName("trailer_id");

                entity.HasOne(d => d.AccessorySize).WithMany(p => p.InventoryRecords).HasForeignKey(d => d.AccessorySizeId);
                entity.HasOne(d => d.TargetStore).WithMany(p => p.InventoryRecords).HasForeignKey(d => d.TargetStoreId);
                entity.HasOne(d => d.Trailer).WithMany(p => p.InventoryRecords).HasForeignKey(d => d.TrailerId);
            });

            // Trailer
            modelBuilder.Entity<Trailer>(entity =>
            {
                entity.ToTable("trailers");

                entity.HasIndex(e => e.Vin, "IX_trailers_vin").IsUnique();  // VIN唯一性检查

                entity.Property(e => e.TrailerId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("trailer_id");
                entity.Property(e => e.CurrentStatus)
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("current_status");
                entity.Property(e => e.ModelName)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("model_name");
                entity.Property(e => e.RatedCapacity)
                    .HasColumnType("FLOAT")
                    .HasColumnName("rated_capacity");
                entity.Property(e => e.Size)
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("size");
                entity.Property(e => e.StoreId).HasColumnName("store_id");
                entity.Property(e => e.ThresholdQuantity).HasColumnName("threshold_quantity");
                entity.Property(e => e.Vin)
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("vin");

                entity.HasOne(d => d.Store).WithMany(p => p.Trailers)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasMany(d => d.AccessorySizes).WithMany(p => p.Trailers)
                    .UsingEntity<Dictionary<string, object>>(
                        "TrailerAccessorySizeAssociation",
                        r => r.HasOne<AccessorySize>().WithMany()
                            .HasForeignKey("AccessorySizeId")
                            .OnDelete(DeleteBehavior.ClientSetNull),
                        l => l.HasOne<Trailer>().WithMany()
                            .HasForeignKey("TrailerId")
                            .OnDelete(DeleteBehavior.ClientSetNull),
                        j =>
                        {
                            j.HasKey("TrailerId", "AccessorySizeId");
                            j.ToTable("trailer_accessory_size_association");
                            j.IndexerProperty<int>("TrailerId").HasColumnName("trailer_id");
                            j.IndexerProperty<int>("AccessorySizeId").HasColumnName("accessory_size_id");
                        });
            });

            // TransferRecord - 添加双向外键关系
            modelBuilder.Entity<TransferRecord>(entity =>
            {
                entity.ToTable("transfer_records");

                entity.Property(e => e.TransferId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("transfer_id");
                
                entity.Property(e => e.TransferTime)
                    .HasColumnType("DATETIME")
                    .HasColumnName("transfer_time");

                entity.Property(e => e.SourceStoreId).HasColumnName("source_store_id");
                entity.Property(e => e.TargetStoreId).HasColumnName("target_store_id");
                entity.Property(e => e.Operator)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("operator");

                // 外键指向来源 Store
                entity.HasOne(d => d.SourceStore)
                    .WithMany(p => p.TransferRecordSourceStores)
                    .HasForeignKey(d => d.SourceStoreId)
                    .OnDelete(DeleteBehavior.Restrict);  // 禁止删除时的级联操作

                // 外键指向目标 Store
                entity.HasOne(d => d.TargetStore)
                    .WithMany(p => p.TransferRecordTargetStores)
                    .HasForeignKey(d => d.TargetStoreId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // OperationLog - Log table
            modelBuilder.Entity<OperationLog>(entity =>
            {
                entity.ToTable("operation_logs");

                entity.Property(e => e.LogId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("log_id");
                entity.Property(e => e.EntityType)
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("entity_type");
                entity.Property(e => e.EntityId).HasColumnName("entity_id");
                entity.Property(e => e.OperationType)
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("operation_type");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Description)
                    .HasColumnType("VARCHAR(500)")
                    .HasColumnName("description");
                entity.Property(e => e.OperationTime)
                    .HasColumnType("DATETIME")
                    .HasColumnName("operation_time");
            });

             modelBuilder.Entity<AssemblyRecord>(entity =>
            {
                entity.ToTable("assembly_records");

                entity.HasKey(e => e.AssemblyId);  // 设置 AssemblyId 为主键

                entity.Property(e => e.AssemblyId)
                    .ValueGeneratedOnAdd()  // 设置为自动递增
                    .HasColumnName("assembly_id");

                entity.Property(e => e.AssemblyTime)
                    .HasColumnType("DATETIME")
                    .HasColumnName("assembly_time");

                entity.Property(e => e.Operator)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("operator");

                entity.Property(e => e.AccessorySizeId).HasColumnName("accessory_size_id");
                entity.Property(e => e.TrailerId).HasColumnName("trailer_id");

                entity.HasOne(d => d.AccessorySize)
                    .WithMany(p => p.AssemblyRecords)
                    .HasForeignKey(d => d.AccessorySizeId);

                entity.HasOne(d => d.Trailer)
                    .WithMany(p => p.AssemblyRecords)
                    .HasForeignKey(d => d.TrailerId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=C:/工作日志/库存管理系统/backend/TrailerCompanyBackend/trailer_company.db");
            }
        }
    }
}
