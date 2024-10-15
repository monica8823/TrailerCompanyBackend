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

        public virtual DbSet<AssemblyRecord> AssemblyRecords { get; set; }

        public virtual DbSet<DisposalRecord> DisposalRecords { get; set; }

        public virtual DbSet<InventoryRecord> InventoryRecords { get; set; }

        public virtual DbSet<RepairRecord> RepairRecords { get; set; }

        public virtual DbSet<RestockRecord> RestockRecords { get; set; }

        public virtual DbSet<SalesRecord> SalesRecords { get; set; }

        public virtual DbSet<Store> Stores { get; set; }

        public virtual DbSet<Trailer> Trailers { get; set; }

        public virtual DbSet<TransferRecord> TransferRecords { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accessory>(entity =>
            {
                entity.ToTable("accessories");

                entity.Property(e => e.AccessoryId)
                    .ValueGeneratedNever()
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

            modelBuilder.Entity<AccessorySize>(entity =>
            {
                entity.HasKey(e => e.SizeId);

                entity.ToTable("accessory_sizes");

                entity.Property(e => e.SizeId)
                    .ValueGeneratedNever()
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

            modelBuilder.Entity<AlertRecord>(entity =>
            {
                entity.HasKey(e => e.AlertId);

                entity.ToTable("alert_records");

                entity.Property(e => e.AlertId)
                    .ValueGeneratedNever()
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

            modelBuilder.Entity<AssemblyRecord>(entity =>
            {
                entity.HasKey(e => e.AssemblyId);

                entity.ToTable("assembly_records");

                entity.Property(e => e.AssemblyId)
                    .ValueGeneratedNever()
                    .HasColumnName("assembly_id");
                entity.Property(e => e.AccessorySizeId).HasColumnName("accessory_size_id");
                entity.Property(e => e.AssemblyTime)
                    .HasColumnType("DATETIME")
                    .HasColumnName("assembly_time");
                entity.Property(e => e.Operator)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("operator");
                entity.Property(e => e.TrailerId).HasColumnName("trailer_id");

                entity.HasOne(d => d.AccessorySize).WithMany(p => p.AssemblyRecords).HasForeignKey(d => d.AccessorySizeId);

                entity.HasOne(d => d.Trailer).WithMany(p => p.AssemblyRecords)
                    .HasForeignKey(d => d.TrailerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DisposalRecord>(entity =>
            {
                entity.HasKey(e => e.DisposalId);

                entity.ToTable("disposal_records");

                entity.Property(e => e.DisposalId)
                    .ValueGeneratedNever()
                    .HasColumnName("disposal_id");
                entity.Property(e => e.AccessorySizeId).HasColumnName("accessory_size_id");
                entity.Property(e => e.DisposalTime)
                    .HasColumnType("DATETIME")
                    .HasColumnName("disposal_time");
                entity.Property(e => e.Operator)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("operator");
                entity.Property(e => e.Reason)
                    .HasColumnType("VARCHAR(500)")
                    .HasColumnName("reason");
                entity.Property(e => e.TrailerId).HasColumnName("trailer_id");

                entity.HasOne(d => d.AccessorySize).WithMany(p => p.DisposalRecords).HasForeignKey(d => d.AccessorySizeId);

                entity.HasOne(d => d.Trailer).WithMany(p => p.DisposalRecords).HasForeignKey(d => d.TrailerId);
            });

            modelBuilder.Entity<InventoryRecord>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.ToTable("inventory_records");

                entity.Property(e => e.RecordId)
                    .ValueGeneratedNever()
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

            modelBuilder.Entity<RepairRecord>(entity =>
            {
                entity.HasKey(e => e.RepairId);

                entity.ToTable("repair_records");

                entity.Property(e => e.RepairId)
                    .ValueGeneratedNever()
                    .HasColumnName("repair_id");
                entity.Property(e => e.AccessorySizeId).HasColumnName("accessory_size_id");
                entity.Property(e => e.Operator)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("operator");
                entity.Property(e => e.RepairDetails)
                    .HasColumnType("VARCHAR(500)")
                    .HasColumnName("repair_details");
                entity.Property(e => e.RepairTime)
                    .HasColumnType("DATETIME")
                    .HasColumnName("repair_time");
                entity.Property(e => e.TrailerId).HasColumnName("trailer_id");

                entity.HasOne(d => d.AccessorySize).WithMany(p => p.RepairRecords).HasForeignKey(d => d.AccessorySizeId);

                entity.HasOne(d => d.Trailer).WithMany(p => p.RepairRecords).HasForeignKey(d => d.TrailerId);
            });

            modelBuilder.Entity<RestockRecord>(entity =>
            {
                entity.HasKey(e => e.RestockId);

                entity.ToTable("restock_records");

                entity.Property(e => e.RestockId)
                    .ValueGeneratedNever()
                    .HasColumnName("restock_id");
                entity.Property(e => e.AccessorySizeId).HasColumnName("accessory_size_id");
                entity.Property(e => e.Operator)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("operator");
                entity.Property(e => e.RestockQuantity).HasColumnName("restock_quantity");
                entity.Property(e => e.RestockTime)
                    .HasColumnType("DATETIME")
                    .HasColumnName("restock_time");
                entity.Property(e => e.TrailerId).HasColumnName("trailer_id");

                entity.HasOne(d => d.AccessorySize).WithMany(p => p.RestockRecords).HasForeignKey(d => d.AccessorySizeId);

                entity.HasOne(d => d.Trailer).WithMany(p => p.RestockRecords).HasForeignKey(d => d.TrailerId);
            });

            modelBuilder.Entity<SalesRecord>(entity =>
            {
                entity.HasKey(e => e.SalesId);

                entity.ToTable("sales_records");

                entity.Property(e => e.SalesId)
                    .ValueGeneratedNever()
                    .HasColumnName("sales_id");
                entity.Property(e => e.AccessorySizeId).HasColumnName("accessory_size_id");
                entity.Property(e => e.InvNumber)
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("inv_number");
                entity.Property(e => e.Operator)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("operator");
                entity.Property(e => e.SalesPrice)
                    .HasColumnType("FLOAT")
                    .HasColumnName("sales_price");
                entity.Property(e => e.SalesTime)
                    .HasColumnType("DATETIME")
                    .HasColumnName("sales_time");
                entity.Property(e => e.TrailerId).HasColumnName("trailer_id");

                entity.HasOne(d => d.AccessorySize).WithMany(p => p.SalesRecords).HasForeignKey(d => d.AccessorySizeId);

                entity.HasOne(d => d.Trailer).WithMany(p => p.SalesRecords).HasForeignKey(d => d.TrailerId);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("stores");

                entity.Property(e => e.StoreId)
                    .ValueGeneratedNever()
                    .HasColumnName("store_id");
                entity.Property(e => e.StoreAddress)
                    .HasColumnType("VARCHAR(200)")
                    .HasColumnName("store_address");
                entity.Property(e => e.StoreName)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("store_name");
            });

            modelBuilder.Entity<Trailer>(entity =>
            {
                entity.ToTable("trailers");

                entity.HasIndex(e => e.Vin, "IX_trailers_vin").IsUnique();

                entity.Property(e => e.TrailerId)
                    .ValueGeneratedNever()
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

            modelBuilder.Entity<TransferRecord>(entity =>
            {
                entity.HasKey(e => e.TransferId);

                entity.ToTable("transfer_records");

                entity.Property(e => e.TransferId)
                    .ValueGeneratedNever()
                    .HasColumnName("transfer_id");
                entity.Property(e => e.AccessorySizeId).HasColumnName("accessory_size_id");
                entity.Property(e => e.Operator)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("operator");
                entity.Property(e => e.SourceStoreId).HasColumnName("source_store_id");
                entity.Property(e => e.TargetStoreId).HasColumnName("target_store_id");
                entity.Property(e => e.TrailerId).HasColumnName("trailer_id");
                entity.Property(e => e.TransferTime)
                    .HasColumnType("DATETIME")
                    .HasColumnName("transfer_time");

                entity.HasOne(d => d.AccessorySize).WithMany(p => p.TransferRecords).HasForeignKey(d => d.AccessorySizeId);

                entity.HasOne(d => d.SourceStore).WithMany(p => p.TransferRecordSourceStores)
                    .HasForeignKey(d => d.SourceStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TargetStore).WithMany(p => p.TransferRecordTargetStores)
                    .HasForeignKey(d => d.TargetStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Trailer).WithMany(p => p.TransferRecords).HasForeignKey(d => d.TrailerId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "IX_users_email").IsUnique();

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("user_id");
                entity.Property(e => e.Email)
                    .HasColumnType("VARCHAR(120)")
                    .HasColumnName("email");
                entity.Property(e => e.Password)
                    .HasColumnType("VARCHAR(200)")
                    .HasColumnName("password");
                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("DATETIME")
                    .HasColumnName("registration_date");
                entity.Property(e => e.Role)
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("role");
                entity.Property(e => e.Status)
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("status");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // 仅在没有通过 DI 提供 options 的情况下使用
                optionsBuilder.UseSqlite("Data Source=C:/工作日志/库存管理系统/backend/TrailerCompanyBackend/trailer_company.db");
            }
        }
    }
}
