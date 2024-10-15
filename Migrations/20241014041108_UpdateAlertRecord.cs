using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrailerCompanyBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAlertRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRoleEnum",
                table: "users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "alert_records",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRoleEnum",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "alert_records");
        }
    }
}
