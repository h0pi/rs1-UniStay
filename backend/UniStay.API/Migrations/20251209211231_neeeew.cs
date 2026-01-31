using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniStay.API.Migrations
{
    /// <inheritdoc />
    public partial class neeeew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           // migrationBuilder.DropForeignKey(
           //     name: "FK_TwoFactorSettings_Role_RoleId",
           //     table: "TwoFactorSettings");

          //  migrationBuilder.DropIndex(
          //      name: "IX_TwoFactorSettings_RoleId",
          //      table: "TwoFactorSettings");

            //migrationBuilder.DropColumn(
            //    name: "RoleId",
            //    table: "TwoFactorSettings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "TwoFactorSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TwoFactorSettings_RoleId",
                table: "TwoFactorSettings",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_TwoFactorSettings_Role_RoleId",
                table: "TwoFactorSettings",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
