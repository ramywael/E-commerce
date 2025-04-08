using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce510.Migrations
{
    /// <inheritdoc />
    public partial class removeTotalSum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSum",
                table: "Carts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalSum",
                table: "Carts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
