using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrailerCompanyBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "stores",
                columns: table => new
                {
                    store_id = table.Column<int>(type: "INTEGER", nullable: false),
                    store_name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    store_address = table.Column<string>(type: "VARCHAR(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stores", x => x.store_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    email = table.Column<string>(type: "VARCHAR(120)", nullable: false),
                    password = table.Column<string>(type: "VARCHAR(200)", nullable: false),
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
                    accessory_id = table.Column<int>(type: "INTEGER", nullable: false),
                    accessory_type = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    description = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    store_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accessories", x => x.accessory_id);
                    table.ForeignKey(
                        name: "FK_accessories_stores_store_id",
                        column: x => x.store_id,
                        principalTable: "stores",
                        principalColumn: "store_id");
                });

            migrationBuilder.CreateTable(
                name: "trailers",
                columns: table => new
                {
                    trailer_id = table.Column<int>(type: "INTEGER", nullable: false),
                    vin = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    model_name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    size = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    rated_capacity = table.Column<double>(type: "FLOAT", nullable: false),
                    current_status = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    store_id = table.Column<int>(type: "INTEGER", nullable: false),
                    threshold_quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trailers", x => x.trailer_id);
                    table.ForeignKey(
                        name: "FK_trailers_stores_store_id",
                        column: x => x.store_id,
                        principalTable: "stores",
                        principalColumn: "store_id");
                });

            migrationBuilder.CreateTable(
                name: "accessory_sizes",
                columns: table => new
                {
                    size_id = table.Column<int>(type: "INTEGER", nullable: false),
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
                name: "alert_records",
                columns: table => new
                {
                    alert_id = table.Column<int>(type: "INTEGER", nullable: false),
                    trailer_id = table.Column<int>(type: "INTEGER", nullable: true),
                    accessory_size_id = table.Column<int>(type: "INTEGER", nullable: true),
                    current_quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    threshold_quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    alert_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    alert_type = table.Column<string>(type: "VARCHAR(100)", nullable: false)
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
                    assembly_id = table.Column<int>(type: "INTEGER", nullable: false),
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
                        principalColumn: "trailer_id");
                });

            migrationBuilder.CreateTable(
                name: "disposal_records",
                columns: table => new
                {
                    disposal_id = table.Column<int>(type: "INTEGER", nullable: false),
                    trailer_id = table.Column<int>(type: "INTEGER", nullable: true),
                    accessory_size_id = table.Column<int>(type: "INTEGER", nullable: true),
                    disposal_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    reason = table.Column<string>(type: "VARCHAR(500)", nullable: false),
                    @operator = table.Column<string>(name: "operator", type: "VARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_disposal_records", x => x.disposal_id);
                    table.ForeignKey(
                        name: "FK_disposal_records_accessory_sizes_accessory_size_id",
                        column: x => x.accessory_size_id,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_disposal_records_trailers_trailer_id",
                        column: x => x.trailer_id,
                        principalTable: "trailers",
                        principalColumn: "trailer_id");
                });

            migrationBuilder.CreateTable(
                name: "inventory_records",
                columns: table => new
                {
                    record_id = table.Column<int>(type: "INTEGER", nullable: false),
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
                        name: "FK_inventory_records_accessory_sizes_accessory_size_id",
                        column: x => x.accessory_size_id,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_inventory_records_stores_target_store_id",
                        column: x => x.target_store_id,
                        principalTable: "stores",
                        principalColumn: "store_id");
                    table.ForeignKey(
                        name: "FK_inventory_records_trailers_trailer_id",
                        column: x => x.trailer_id,
                        principalTable: "trailers",
                        principalColumn: "trailer_id");
                });

            migrationBuilder.CreateTable(
                name: "repair_records",
                columns: table => new
                {
                    repair_id = table.Column<int>(type: "INTEGER", nullable: false),
                    trailer_id = table.Column<int>(type: "INTEGER", nullable: true),
                    accessory_size_id = table.Column<int>(type: "INTEGER", nullable: true),
                    repair_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    repair_details = table.Column<string>(type: "VARCHAR(500)", nullable: false),
                    @operator = table.Column<string>(name: "operator", type: "VARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_repair_records", x => x.repair_id);
                    table.ForeignKey(
                        name: "FK_repair_records_accessory_sizes_accessory_size_id",
                        column: x => x.accessory_size_id,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_repair_records_trailers_trailer_id",
                        column: x => x.trailer_id,
                        principalTable: "trailers",
                        principalColumn: "trailer_id");
                });

            migrationBuilder.CreateTable(
                name: "restock_records",
                columns: table => new
                {
                    restock_id = table.Column<int>(type: "INTEGER", nullable: false),
                    trailer_id = table.Column<int>(type: "INTEGER", nullable: true),
                    accessory_size_id = table.Column<int>(type: "INTEGER", nullable: true),
                    restock_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    restock_quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    @operator = table.Column<string>(name: "operator", type: "VARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restock_records", x => x.restock_id);
                    table.ForeignKey(
                        name: "FK_restock_records_accessory_sizes_accessory_size_id",
                        column: x => x.accessory_size_id,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_restock_records_trailers_trailer_id",
                        column: x => x.trailer_id,
                        principalTable: "trailers",
                        principalColumn: "trailer_id");
                });

            migrationBuilder.CreateTable(
                name: "sales_records",
                columns: table => new
                {
                    sales_id = table.Column<int>(type: "INTEGER", nullable: false),
                    trailer_id = table.Column<int>(type: "INTEGER", nullable: true),
                    accessory_size_id = table.Column<int>(type: "INTEGER", nullable: true),
                    sales_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    sales_price = table.Column<double>(type: "FLOAT", nullable: false),
                    inv_number = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    @operator = table.Column<string>(name: "operator", type: "VARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sales_records", x => x.sales_id);
                    table.ForeignKey(
                        name: "FK_sales_records_accessory_sizes_accessory_size_id",
                        column: x => x.accessory_size_id,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_sales_records_trailers_trailer_id",
                        column: x => x.trailer_id,
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
                    transfer_id = table.Column<int>(type: "INTEGER", nullable: false),
                    trailer_id = table.Column<int>(type: "INTEGER", nullable: true),
                    accessory_size_id = table.Column<int>(type: "INTEGER", nullable: true),
                    transfer_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    source_store_id = table.Column<int>(type: "INTEGER", nullable: false),
                    target_store_id = table.Column<int>(type: "INTEGER", nullable: false),
                    @operator = table.Column<string>(name: "operator", type: "VARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transfer_records", x => x.transfer_id);
                    table.ForeignKey(
                        name: "FK_transfer_records_accessory_sizes_accessory_size_id",
                        column: x => x.accessory_size_id,
                        principalTable: "accessory_sizes",
                        principalColumn: "size_id");
                    table.ForeignKey(
                        name: "FK_transfer_records_stores_source_store_id",
                        column: x => x.source_store_id,
                        principalTable: "stores",
                        principalColumn: "store_id");
                    table.ForeignKey(
                        name: "FK_transfer_records_stores_target_store_id",
                        column: x => x.target_store_id,
                        principalTable: "stores",
                        principalColumn: "store_id");
                    table.ForeignKey(
                        name: "FK_transfer_records_trailers_trailer_id",
                        column: x => x.trailer_id,
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
                name: "IX_disposal_records_accessory_size_id",
                table: "disposal_records",
                column: "accessory_size_id");

            migrationBuilder.CreateIndex(
                name: "IX_disposal_records_trailer_id",
                table: "disposal_records",
                column: "trailer_id");

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
                name: "IX_repair_records_accessory_size_id",
                table: "repair_records",
                column: "accessory_size_id");

            migrationBuilder.CreateIndex(
                name: "IX_repair_records_trailer_id",
                table: "repair_records",
                column: "trailer_id");

            migrationBuilder.CreateIndex(
                name: "IX_restock_records_accessory_size_id",
                table: "restock_records",
                column: "accessory_size_id");

            migrationBuilder.CreateIndex(
                name: "IX_restock_records_trailer_id",
                table: "restock_records",
                column: "trailer_id");

            migrationBuilder.CreateIndex(
                name: "IX_sales_records_accessory_size_id",
                table: "sales_records",
                column: "accessory_size_id");

            migrationBuilder.CreateIndex(
                name: "IX_sales_records_trailer_id",
                table: "sales_records",
                column: "trailer_id");

            migrationBuilder.CreateIndex(
                name: "IX_trailer_accessory_size_association_accessory_size_id",
                table: "trailer_accessory_size_association",
                column: "accessory_size_id");

            migrationBuilder.CreateIndex(
                name: "IX_trailers_store_id",
                table: "trailers",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_trailers_vin",
                table: "trailers",
                column: "vin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transfer_records_accessory_size_id",
                table: "transfer_records",
                column: "accessory_size_id");

            migrationBuilder.CreateIndex(
                name: "IX_transfer_records_source_store_id",
                table: "transfer_records",
                column: "source_store_id");

            migrationBuilder.CreateIndex(
                name: "IX_transfer_records_target_store_id",
                table: "transfer_records",
                column: "target_store_id");

            migrationBuilder.CreateIndex(
                name: "IX_transfer_records_trailer_id",
                table: "transfer_records",
                column: "trailer_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alert_records");

            migrationBuilder.DropTable(
                name: "assembly_records");

            migrationBuilder.DropTable(
                name: "disposal_records");

            migrationBuilder.DropTable(
                name: "inventory_records");

            migrationBuilder.DropTable(
                name: "repair_records");

            migrationBuilder.DropTable(
                name: "restock_records");

            migrationBuilder.DropTable(
                name: "sales_records");

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
                name: "stores");
        }
    }
}
