using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniStay.API.Migrations
{
    /// <inheritdoc />
    public partial class newmigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Requires2FA",
                table: "TwoFactorSettings",
                newName: "RequiresTwoFactor");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "EquipmentRecord",
                newName: "RecordSerialNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequiresTwoFactor",
                table: "TwoFactorSettings",
                newName: "Requires2FA");

            migrationBuilder.RenameColumn(
                name: "RecordSerialNumber",
                table: "EquipmentRecord",
                newName: "SerialNumber");
        }
    }
}
