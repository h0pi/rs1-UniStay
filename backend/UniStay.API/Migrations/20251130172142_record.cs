using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniStay.API.Migrations
{
    /// <inheritdoc />
    public partial class record : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "EquipmentRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "EquipmentRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "EquipmentRecord",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "SerialNumber", table: "EquipmentRecord");
            migrationBuilder.DropColumn(name: "Location", table: "EquipmentRecord");
            migrationBuilder.DropColumn(name: "IsAvailable", table: "EquipmentRecord");
        }



    }
}
