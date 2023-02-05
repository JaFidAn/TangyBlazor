using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TangyDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addCarierAndTrackingToOrderHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Carier",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tracking",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Carier",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "Tracking",
                table: "OrderHeaders");
        }
    }
}
