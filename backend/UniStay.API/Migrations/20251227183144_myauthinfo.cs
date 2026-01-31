using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniStay.API.Migrations
{
    /// <inheritdoc />
    public partial class myauthinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "MyAuthInfo",
                newName: "RoleName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "MyAuthInfo",
                newName: "RoleName");
        }
    }
}
