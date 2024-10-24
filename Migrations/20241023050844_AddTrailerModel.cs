using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrailerCompanyBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddTrailerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "operation_logs",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    entity_type = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    entity_id = table.Column<int>(type: "INTEGER", nullable: false),
                    operation_type = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    description = table.Column<string>(type: "VARCHAR(500)", nullable: false),
                    operation_time = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operation_logs", x => x.log_id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    StoreId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StoreName = table.Column<string>(type: "TEXT", nullable: false),
                    StoreAddress = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.StoreId);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    email = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    password = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    role = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    registration_date = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    status = table.Column<string>(type: "VARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "accessories",
                columns: table => new
                {
                    accessory_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    accessory_type = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    description = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    store_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accessories", x => x.accessory_id);
                    table.ForeignKey(
                        name: "FK_accessories_Stores_store_id",
                        column: x => x.store_id,
                        principalTable: "Stores",
                        principalColumn: "StoreId");
                });

            migrationBuilder.CreateTable(
                name: "trailer_models",
                columns: table => new
                {
                    trailer_model_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    model_name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    store_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trailer_models", x => x.trailer_model_id);
                    table.ForeignKey(
                        name: "FK_trailer_models_Stores_store_id",
                        column: x => x.store_id,
                        principalTable: "Stores",
                        principalColumn: "StoreId");
                });

            migrationBuilder.CreateTable(
                name: "accessory_sizes",
                columns: table => new
                {
                    size_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    accessory_id = table.Column<int>(type: "INTEGER", nullable: false),
                    size_name = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    detailed_specification = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    threshold_quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accessory_sizes", x => x.size_id);
                    table.ForeignKey(
                        name: "FK_accessory_sizes_accessories_accessory_id",
                        column: x => x.accessory_id,
                        principalTable: "accessories",
                        principalColumn: "accessory_id");
                });

            migrationBuilder.CreateTable(
                name: "trailers",
                columns: table => new
                {
                    trailer_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    vin = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    model_name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    size = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    rated_capacity = table.Column<double>(type: "FLOAT", nullable: true),
                    current_status = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    store_id = table.Column<int>(type: "INTEGER", nullable: false),
                    threshold_quantity = table.Column<int>(type: "INTEGER", nullable: true),
                    CustomFields = table.Column<string>(type: "TEXT", nullable: true),
                    trailer_model_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trailers", x => x.trailer_id);
                    table.ForeignKey(
                        name: "FK_trailers_Stores_store_id",
                        column: x => x.store_id,
                        principalTable: "Stores",
                        principalColumn: "StoreId");
                    table.ForeignKey(
                        name: "FK_trailers_trailer_models_trailer_model_id",
                        column: x => x.trailer_model_id,
                        principalTable: "trailer_models",
                        principalColumn: "trailer_model_id");
                });

            migrationBuilder.CreateTable(
                name: "alert_records",
                columns: table => new
                {
                    alert_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    trailer_id = table.Column<int>(type: "INTEGER", nullable: true),
                    accessory_size_id = table.Column<int>(type: "INTEGER", nullable: true),
                    current_quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    threshold_quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    alert_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    alert_type = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alert_records", x => x.alert_id);
                    table.ForeignKey(
                        name: "FK_alert_records_accessory_sizes_accessory_size_id",
                        column: x => x.accessory_size_id,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_alert_records_trailers_trailer_id",
                        column: x => x.trailer_id,
                        principalTable: "trailers",
                        principalColumn: "trailer_id");
                });

            migrationBuilder.CreateTable(
                name: "assembly_records",
                columns: table => new
                {
                    assembly_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    trailer_id = table.Column<int>(type: "INTEGER", nullable: false),
                    accessory_size_id = table.Column<int>(type: "INTEGER", nullable: true),
                    assembly_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    @operator = table.Column<string>(name: "operator", type: "VARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assembly_records", x => x.assembly_id);
                    table.ForeignKey(
                        name: "FK_assembly_records_accessory_sizes_accessory_size_id",
                        column: x => x.accessory_size_id,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_assembly_records_trailers_trailer_id",
                        column: x => x.trailer_id,
                        principalTable: "trailers",
                        principalColumn: "trailer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisposalRecords",
                columns: table => new
                {
                    DisposalId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrailerId = table.Column<int>(type: "INTEGER", nullable: true),
                    AccessorySizeId = table.Column<int>(type: "INTEGER", nullable: true),
                    DisposalTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Reason = table.Column<string>(type: "TEXT", nullable: false),
                    Operator = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisposalRecords", x => x.DisposalId);
                    table.ForeignKey(
                        name: "FK_DisposalRecords_accessory_sizes_AccessorySizeId",
                        column: x => x.AccessorySizeId,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_DisposalRecords_trailers_TrailerId",
                        column: x => x.TrailerId,
                        principalTable: "trailers",
                        principalColumn: "trailer_id");
                });

            migrationBuilder.CreateTable(
                name: "inventory_records",
                columns: table => new
                {
                    record_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    trailer_id = table.Column<int>(type: "INTEGER", nullable: true),
                    accessory_size_id = table.Column<int>(type: "INTEGER", nullable: true),
                    operation_type = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    operation_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    @operator = table.Column<string>(name: "operator", type: "VARCHAR(100)", nullable: false),
                    target_store_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_records", x => x.record_id);
                    table.ForeignKey(
                        name: "FK_inventory_records_Stores_target_store_id",
                        column: x => x.target_store_id,
                        principalTable: "Stores",
                        principalColumn: "StoreId");
                    table.ForeignKey(
                        name: "FK_inventory_records_accessory_sizes_accessory_size_id",
                        column: x => x.accessory_size_id,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_inventory_records_trailers_trailer_id",
                        column: x => x.trailer_id,
                        principalTable: "trailers",
                        principalColumn: "trailer_id");
                });

            migrationBuilder.CreateTable(
                name: "RepairRecords",
                columns: table => new
                {
                    RepairId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrailerId = table.Column<int>(type: "INTEGER", nullable: true),
                    AccessorySizeId = table.Column<int>(type: "INTEGER", nullable: true),
                    RepairTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RepairDetails = table.Column<string>(type: "TEXT", nullable: false),
                    Operator = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairRecords", x => x.RepairId);
                    table.ForeignKey(
                        name: "FK_RepairRecords_accessory_sizes_AccessorySizeId",
                        column: x => x.AccessorySizeId,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_RepairRecords_trailers_TrailerId",
                        column: x => x.TrailerId,
                        principalTable: "trailers",
                        principalColumn: "trailer_id");
                });

            migrationBuilder.CreateTable(
                name: "RestockRecords",
                columns: table => new
                {
                    RestockId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrailerId = table.Column<int>(type: "INTEGER", nullable: true),
                    AccessorySizeId = table.Column<int>(type: "INTEGER", nullable: true),
                    RestockTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RestockQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Operator = table.Column<string>(type: "TEXT", nullable: false),
                    RestockMethod = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestockRecords", x => x.RestockId);
                    table.ForeignKey(
                        name: "FK_RestockRecords_accessory_sizes_AccessorySizeId",
                        column: x => x.AccessorySizeId,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_RestockRecords_trailers_TrailerId",
                        column: x => x.TrailerId,
                        principalTable: "trailers",
                        principalColumn: "trailer_id");
                });

            migrationBuilder.CreateTable(
                name: "SalesRecords",
                columns: table => new
                {
                    SalesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrailerId = table.Column<int>(type: "INTEGER", nullable: true),
                    AccessorySizeId = table.Column<int>(type: "INTEGER", nullable: true),
                    SalesTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SalesPrice = table.Column<double>(type: "REAL", nullable: false),
                    InvNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Operator = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesRecords", x => x.SalesId);
                    table.ForeignKey(
                        name: "FK_SalesRecords_accessory_sizes_AccessorySizeId",
                        column: x => x.AccessorySizeId,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_SalesRecords_trailers_TrailerId",
                        column: x => x.TrailerId,
                        principalTable: "trailers",
                        principalColumn: "trailer_id");
                });

            migrationBuilder.CreateTable(
                name: "trailer_accessory_size_association",
                columns: table => new
                {
                    trailer_id = table.Column<int>(type: "INTEGER", nullable: false),
                    accessory_size_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trailer_accessory_size_association", x => new { x.trailer_id, x.accessory_size_id });
                    table.ForeignKey(
                        name: "FK_trailer_accessory_size_association_accessory_sizes_accessory_size_id",
                        column: x => x.accessory_size_id,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_trailer_accessory_size_association_trailers_trailer_id",
                        column: x => x.trailer_id,
                        principalTable: "trailers",
                        principalColumn: "trailer_id");
                });

            migrationBuilder.CreateTable(
                name: "transfer_records",
                columns: table => new
                {
                    transfer_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrailerId = table.Column<int>(type: "INTEGER", nullable: true),
                    AccessorySizeId = table.Column<int>(type: "INTEGER", nullable: true),
                    transfer_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    source_store_id = table.Column<int>(type: "INTEGER", nullable: false),
                    target_store_id = table.Column<int>(type: "INTEGER", nullable: false),
                    @operator = table.Column<string>(name: "operator", type: "VARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transfer_records", x => x.transfer_id);
                    table.ForeignKey(
                        name: "FK_transfer_records_Stores_source_store_id",
                        column: x => x.source_store_id,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transfer_records_Stores_target_store_id",
                        column: x => x.target_store_id,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transfer_records_accessory_sizes_AccessorySizeId",
                        column: x => x.AccessorySizeId,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_transfer_records_trailers_TrailerId",
                        column: x => x.TrailerId,
                        principalTable: "trailers",
                        principalColumn: "trailer_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_accessories_store_id",
                table: "accessories",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_accessory_sizes_accessory_id",
                table: "accessory_sizes",
                column: "accessory_id");

            migrationBuilder.CreateIndex(
                name: "IX_alert_records_accessory_size_id",
                table: "alert_records",
                column: "accessory_size_id");

            migrationBuilder.CreateIndex(
                name: "IX_alert_records_trailer_id",
                table: "alert_records",
                column: "trailer_id");

            migrationBuilder.CreateIndex(
                name: "IX_assembly_records_accessory_size_id",
                table: "assembly_records",
                column: "accessory_size_id");

            migrationBuilder.CreateIndex(
                name: "IX_assembly_records_trailer_id",
                table: "assembly_records",
                column: "trailer_id");

            migrationBuilder.CreateIndex(
                name: "IX_DisposalRecords_AccessorySizeId",
                table: "DisposalRecords",
                column: "AccessorySizeId");

            migrationBuilder.CreateIndex(
                name: "IX_DisposalRecords_TrailerId",
                table: "DisposalRecords",
                column: "TrailerId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_records_accessory_size_id",
                table: "inventory_records",
                column: "accessory_size_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_records_target_store_id",
                table: "inventory_records",
                column: "target_store_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_records_trailer_id",
                table: "inventory_records",
                column: "trailer_id");

            migrationBuilder.CreateIndex(
                name: "IX_RepairRecords_AccessorySizeId",
                table: "RepairRecords",
                column: "AccessorySizeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairRecords_TrailerId",
                table: "RepairRecords",
                column: "TrailerId");

            migrationBuilder.CreateIndex(
                name: "IX_RestockRecords_AccessorySizeId",
                table: "RestockRecords",
                column: "AccessorySizeId");

            migrationBuilder.CreateIndex(
                name: "IX_RestockRecords_TrailerId",
                table: "RestockRecords",
                column: "TrailerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesRecords_AccessorySizeId",
                table: "SalesRecords",
                column: "AccessorySizeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesRecords_TrailerId",
                table: "SalesRecords",
                column: "TrailerId");

            migrationBuilder.CreateIndex(
                name: "IX_trailer_accessory_size_association_accessory_size_id",
                table: "trailer_accessory_size_association",
                column: "accessory_size_id");

            migrationBuilder.CreateIndex(
                name: "IX_trailer_models_store_id",
                table: "trailer_models",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_trailers_store_id",
                table: "trailers",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_trailers_trailer_model_id",
                table: "trailers",
                column: "trailer_model_id");

            migrationBuilder.CreateIndex(
                name: "IX_trailers_vin",
                table: "trailers",
                column: "vin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transfer_records_AccessorySizeId",
                table: "transfer_records",
                column: "AccessorySizeId");

            migrationBuilder.CreateIndex(
                name: "IX_transfer_records_source_store_id",
                table: "transfer_records",
                column: "source_store_id");

            migrationBuilder.CreateIndex(
                name: "IX_transfer_records_target_store_id",
                table: "transfer_records",
                column: "target_store_id");

            migrationBuilder.CreateIndex(
                name: "IX_transfer_records_TrailerId",
                table: "transfer_records",
                column: "TrailerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alert_records");

            migrationBuilder.DropTable(
                name: "assembly_records");

            migrationBuilder.DropTable(
                name: "DisposalRecords");

            migrationBuilder.DropTable(
                name: "inventory_records");

            migrationBuilder.DropTable(
                name: "operation_logs");

            migrationBuilder.DropTable(
                name: "RepairRecords");

            migrationBuilder.DropTable(
                name: "RestockRecords");

            migrationBuilder.DropTable(
                name: "SalesRecords");

            migrationBuilder.DropTable(
                name: "trailer_accessory_size_association");

            migrationBuilder.DropTable(
                name: "transfer_records");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "accessory_sizes");

            migrationBuilder.DropTable(
                name: "trailers");

            migrationBuilder.DropTable(
                name: "accessories");

            migrationBuilder.DropTable(
                name: "trailer_models");

            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
