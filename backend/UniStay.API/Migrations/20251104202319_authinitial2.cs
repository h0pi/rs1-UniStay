using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniStay.API.Migrations
{
    /// <inheritdoc />
    public partial class authinitial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "Role",
            //    table: "MyAuthInfo",
            //    newName: "RoleName");

            //migrationBuilder.AddColumn<int>(
            //    name: "RoleId",
            //    table: "MyAuthInfo",
            //    type: "int",
            //    nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "RoleId",
            //    table: "MyAuthInfo");

            //migrationBuilder.RenameColumn(
            //    name: "RoleName",
            //    table: "MyAuthInfo",
            //    newName: "Role");
        }
    }
}
