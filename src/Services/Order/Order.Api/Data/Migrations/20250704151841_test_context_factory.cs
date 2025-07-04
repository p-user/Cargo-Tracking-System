using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class test_context_factory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "test",
                table: "DeliveryOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "test",
                table: "DeliveryOrders");
        }
    }
}
