using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lab3_5.Server.Migrations
{
    /// <inheritdoc />
    public partial class DeletedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_deliveries",
                table: "courier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "total_deliveries",
                table: "courier",
                type: "integer",
                nullable: true,
                defaultValue: 0);
        }
    }
}
