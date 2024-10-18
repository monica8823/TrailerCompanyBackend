using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrailerCompanyBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRoleEnum",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRoleEnum",
                table: "users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
