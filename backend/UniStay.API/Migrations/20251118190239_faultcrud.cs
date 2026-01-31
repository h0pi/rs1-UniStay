using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniStay.API.Migrations
{
    /// <inheritdoc />
    public partial class faultcrud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "LastName",
            //    table: "MyAuthInfo",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "",
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "FirstName",
            //    table: "MyAuthInfo",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "",
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsResolved",
                table: "Fault",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsResolved",
                table: "Fault");

            //    migrationBuilder.AlterColumn<string>(
            //        name: "LastName",
            //        table: "MyAuthInfo",
            //        type: "nvarchar(max)",
            //        nullable: true,
            //        oldClrType: typeof(string),
            //        oldType: "nvarchar(max)");

            //    migrationBuilder.AlterColumn<string>(
            //        name: "FirstName",
            //        table: "MyAuthInfo",
            //        type: "nvarchar(max)",
            //        nullable: true,
            //        oldClrType: typeof(string),
            //        oldType: "nvarchar(max)");
            }
        }
}
