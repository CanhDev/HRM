using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class _16_5v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "carryForward",
                table: "LeaveTypes");

            migrationBuilder.DropColumn(
                name: "maxCarryForwardDays",
                table: "LeaveTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "carryForward",
                table: "LeaveTypes",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "maxCarryForwardDays",
                table: "LeaveTypes",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
