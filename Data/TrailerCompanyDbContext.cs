using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using TrailerCompanyBackend.Models;


namespace TrailerCompanyBackend.Models
{
    public partial class TrailerCompanyDbContext : DbContext
    {
   

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
        public virtual DbSet<ContainerEntryRecord> ContainerEntryRecord { get; set; }
        public virtual DbSet<SalesRecord> SalesRecords { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Trailer> Trailers { get; set; }
        public virtual DbSet<TrailerModel> TrailerModels { get; set; }
        public virtual DbSet<TransferRecord> TransferRecords { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<OperationLog> OperationLogs { get; set; }
        public virtual DbSet<BackupRecord> BackupRecords { get; set; }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<InvalidToken> InvalidTokens { get; set; } = null!;

        public DbSet<Month> Months { get; set; }

        public virtual DbSet<ContainerEntryRecord> ContainerEntryRecords { get; set; }

        public virtual DbSet<TransactionAccessoryRecord> TransactionAccessoryRecords { get; set; }
        public virtual DbSet<TransactionTrailerRecord> TransactionTrailerRecords { get; set; }

        public virtual DbSet<ContainerStagingTrailerRecord> ContainerStagingTrailerRecords { get; set; }
        public virtual DbSet<ContainerStagingAccessoryRecord> ContainerStagingAccessoryRecords { get; set; }

        public virtual DbSet<AccessorySizeRelation> AccessorySizeRelations { get; set; }



//表与表之间通常存在关系（如外键关系）。OnModelCreating 方法允许在代码中定义这些关系,而不是依赖于 Entity Framework Core 的默认约定。通过这种方式，可以更精确地控制数据库的生成和操作。



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Accessory configuration
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

            // AccessorySize configuration
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

            // AlertRecord configuration
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

            // InventoryRecord configuration
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

            // TrailerModel configuration
            modelBuilder.Entity<TrailerModel>(entity =>
            {
                entity.ToTable("trailer_models");

                entity.Property(e => e.TrailerModelId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("trailer_model_id");
                entity.Property(e => e.ModelName)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("model_name");
                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.TrailerModels)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull);


            });

            // Trailer configuration
            modelBuilder.Entity<Trailer>(entity =>
            {
                entity.ToTable("trailers");

                // 为 VIN 添加唯一索引，当 Vin 有值时，它在数据库中是唯一的
                entity.HasIndex(e => e.Vin, "IX_trailers_vin").IsUnique();

                // TrailerId - 主键，自增
                entity.Property(e => e.TrailerId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("trailer_id");

                // VIN
                entity.Property(e => e.Vin)
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("vin");
                    
                entity.Property(e => e.CurrentStatus)
                    .IsRequired()
                    .HasColumnType("VARCHAR(50)")
                    .HasDefaultValue("NotAvailable")
                    .HasColumnName("current_status");

                // CustomFields
                entity.Property(e => e.CustomFields)
                    .HasColumnType("TEXT")
                    .HasColumnName("custom_fields");

                // TrailerModelId - 外键
                entity.Property(e => e.TrailerModelId).HasColumnName("trailer_model_id");

                entity.HasOne(d => d.TrailerModel)
                    .WithMany(p => p.Trailers)
                    .HasForeignKey(d => d.TrailerModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                // 配置与 AccessorySize 的多对多关系
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


            // TransferRecord configuration
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

                entity.HasOne(d => d.SourceStore)
                    .WithMany(p => p.TransferRecordSourceStores)
                    .HasForeignKey(d => d.SourceStoreId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TargetStore)
                    .WithMany(p => p.TransferRecordTargetStores)
                    .HasForeignKey(d => d.TargetStoreId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // OperationLog configuration
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

            // AssemblyRecord configuration
            modelBuilder.Entity<AssemblyRecord>(entity =>
            {
                entity.ToTable("assembly_records");

                entity.HasKey(e => e.AssemblyId);

                entity.Property(e => e.AssemblyId)
                    .ValueGeneratedOnAdd()
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

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("VARCHAR(255)")
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("VARCHAR(255)")
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("role");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("status");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("DATETIME")
                    .HasColumnName("registration_date");

                entity.HasMany(u => u.RefreshTokens)
                      .WithOne(rt => rt.User)
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // RefreshToken 配置
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("refresh_tokens");

                entity.HasKey(e => e.TokenId);

                entity.Property(e => e.TokenId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("token_id");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasColumnType("VARCHAR(500)")
                    .HasColumnName("token");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("DATETIME")
                    .HasColumnName("created_at");

                entity.Property(e => e.ExpiresAt)
                    .IsRequired()
                    .HasColumnType("DATETIME")
                    .HasColumnName("expires_at");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");
            });



            //month 配置
            modelBuilder.Entity<Month>(entity =>
            {
                entity.ToTable("months");

                entity.Property(e => e.MonthId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("month_id");

                entity.Property(e => e.MonthName)
                    .IsRequired()
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("month_name");

                entity.Property(e => e.StoreId)
                    .IsRequired()
                    .HasColumnName("store_id");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Months)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            //store 配置
            modelBuilder.Entity<Store>(entity =>
                {
                    entity.ToTable("stores");

                    entity.Property(e => e.StoreId)
                        .ValueGeneratedOnAdd()
                        .HasColumnName("store_id");

                    entity.Property(e => e.StoreName)
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("store_name");

                    entity.HasMany(s => s.TrailerModels)
                        .WithOne(tm => tm.Store)
                        .HasForeignKey(tm => tm.StoreId)
                        .OnDelete(DeleteBehavior.Cascade);
                });


            //
            // ✅ 配置 ContainerEntryRecord
            modelBuilder.Entity<ContainerEntryRecord>(entity =>
            {
                entity.ToTable("container_entry_records");

                entity.Property(e => e.ContainerEntryId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("container_entry_id");

                entity.Property(e => e.ContainerNumber)
                    .IsRequired()
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("container_number");

                entity.Property(e => e.EntryTime)
                    .IsRequired()
                    .HasColumnType("DATETIME")
                    .HasColumnName("entry_time");

                entity.Property(e => e.StoreId)
                    .IsRequired()
                    .HasColumnName("store_id");

                entity.Property(e => e.EntryStatus)
                    .IsRequired()
                    .HasColumnType("VARCHAR(20)")
                    .HasDefaultValue("Pending")
                    .HasColumnName("entry_status");

                entity.Property(e => e.Remarks)
                    .HasColumnType("TEXT")
                    .HasColumnName("remarks");

                entity.HasOne(d => d.Store)
                    .WithMany()
                    .HasForeignKey(d => d.StoreId);
            
});


            // 关系表： 标识业务和拖车的关系，但不储存实际值。
            // ✅ 配置 TransactionTrailerRecord
                        modelBuilder.Entity<TransactionTrailerRecord>(entity =>
            {
                entity.ToTable("transaction_trailer_records");

                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.TransactionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("transaction_id");

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("transaction_type");

                entity.Property(e => e.TrailerId)
                    .IsRequired()
                    .HasColumnName("trailer_id");

                entity.Property(e => e.TransactionTime)
                    .IsRequired()
                    .HasColumnType("DATETIME")
                    .HasColumnName("transaction_time");

                entity.Property(e => e.Remarks)
                    .HasColumnType("TEXT")
                    .HasColumnName("remarks");

                entity.HasOne(d => d.Trailer)
                    .WithMany()
                    .HasForeignKey(d => d.TrailerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


                        // 关系表： 标识业务和配件得关系，但不储存实际值。
                    modelBuilder.Entity<TransactionAccessoryRecord>(entity =>
            {
                entity.ToTable("transaction_accessory_records");

                entity.HasKey(e => e.TransactionId);

                entity.Property(e => e.TransactionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("transaction_id");

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("transaction_type");

                entity.Property(e => e.AccessorySizeId)
                    .IsRequired()
                    .HasColumnName("accessory_size_id");

                entity.Property(e => e.Quantity)
                    .IsRequired()
                    .HasColumnName("quantity");

                entity.Property(e => e.TransactionTime)
                    .IsRequired()
                    .HasColumnType("DATETIME")
                    .HasColumnName("transaction_time");

                entity.Property(e => e.Remarks)
                    .HasColumnType("TEXT")
                    .HasColumnName("remarks");

                entity.HasOne(d => d.AccessorySize)
                    .WithMany()
                    .HasForeignKey(d => d.AccessorySizeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 关系表： 标识业务和配件得关系，但不储存实际值。

            modelBuilder.Entity<ContainerStagingTrailerRecord>(entity =>
            {
                entity.ToTable("container_staging_trailer_records");
            
                entity.HasKey(e => e.Id);
            
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");
            
                entity.Property(e => e.ContainerEntryId)
                    .IsRequired()
                    .HasColumnName("container_entry_id");
            
                entity.Property(e => e.ModelName)
                    .HasColumnType("VARCHAR(100)")
                    .HasColumnName("model_name");
            
                entity.Property(e => e.Vin)
                    .HasColumnType("VARCHAR(50)")
                    .HasColumnName("vin");
            
                entity.Property(e => e.Remarks)
                    .HasColumnType("TEXT")
                    .HasColumnName("remarks");
            
                entity.HasOne(d => d.ContainerEntryRecord)
                    .WithMany()
                    .HasForeignKey(d => d.ContainerEntryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // 预入库 Accessory 表
            modelBuilder.Entity<ContainerStagingAccessoryRecord>(entity =>
            {
                entity.ToTable("container_staging_accessory_records");
            
                entity.HasKey(e => e.Id);
            
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");
            
                entity.Property(e => e.ContainerEntryId)
                    .IsRequired()
                    .HasColumnName("container_entry_id");
            
                entity.Property(e => e.AccessorySizeId)
                    .IsRequired()
                    .HasColumnName("accessory_size_id");
            
                entity.Property(e => e.Quantity)
                    .IsRequired()
                    .HasColumnName("quantity");
            
                entity.Property(e => e.Remarks)
                    .HasColumnType("TEXT")
                    .HasColumnName("remarks");
            
                entity.HasOne(d => d.ContainerEntryRecord)
                    .WithMany()
                    .HasForeignKey(d => d.ContainerEntryId)
                    .OnDelete(DeleteBehavior.Cascade);
            
                entity.HasOne(d => d.AccessorySize)
                    .WithMany()
                    .HasForeignKey(d => d.AccessorySizeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AccessorySizeRelation>(entity =>
            {
                entity.ToTable("accessory_size_relations");

                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.ParentAccessorySize)
                .WithMany()
                .HasForeignKey(e => e.ParentAccessorySizeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ChildAccessorySize)
                .WithMany()
                .HasForeignKey(e => e.ChildAccessorySizeId)
                .OnDelete(DeleteBehavior.Cascade);
                
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

            using (var connection = new SqliteConnection("Data Source=C:/工作日志/库存管理系统/backend/TrailerCompanyBackend/trailer_company.db"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "PRAGMA foreign_keys = ON;";
                    command.ExecuteNonQuery();
                }
            }
        }


        
    }
}
