using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Cargo_HeightCm",
                table: "DeliveryOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Cargo_LengthCm",
                table: "DeliveryOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Cargo_WidthCm",
                table: "DeliveryOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cargo_HeightCm",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "Cargo_LengthCm",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "Cargo_WidthCm",
                table: "DeliveryOrders");
        }
    }
}
