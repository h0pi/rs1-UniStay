using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniStay.API.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // TwoFactorSettings
            migrationBuilder.CreateTable(
                name: "TwoFactorSettings",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    EnabledAt = table.Column<DateTime>(nullable: true),
                    Method = table.Column<string>(nullable: true),
                    Requires2FA = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoFactorSettings", x => x.UserID);
                });
            // BackupCodes
            migrationBuilder.CreateTable(
                name: "BackupCode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(nullable: false),
                    CodeHash = table.Column<string>(nullable: false),
                    Used = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackupCode", x => x.Id);
                });

            //// PasswordResetTokens
            //migrationBuilder.CreateTable(
            //    name: "PasswordResetTokens",
            //    columns: table => new
            //    {
            //        PasswordResetTokenID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserID = table.Column<int>(nullable: false),
            //        Token = table.Column<string>(nullable: false),
            //        ExpiresAt = table.Column<DateTime>(nullable: false),
            //        Used = table.Column<bool>(nullable: false),
            //        CreatedAt = table.Column<DateTime>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PasswordResetTokens", x => x.PasswordResetTokenID);
            //    });

            //// SecurityQuestions
            //migrationBuilder.CreateTable(
            //    name: "SecurityQuestions",
            //    columns: table => new
            //    {
            //        SecurityQuestionID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Text = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SecurityQuestions", x => x.SecurityQuestionID);
            //    });



            // TwoFactorCodes
            migrationBuilder.CreateTable(
                name: "TwoFactorCode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(nullable: false),
                    CodeHash = table.Column<string>(nullable: false),
                    ExpiresAt = table.Column<DateTime>(nullable: false),
                    Used = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoFactorCode", x => x.Id);
                });

            //// UserSecurityAnswers
            //migrationBuilder.CreateTable(
            //    name: "UserSecurityAnswers",
            //    columns: table => new
            //    {
            //        UserSecurityAnswerID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserID = table.Column<int>(nullable: false),
            //        SecurityQuestionID = table.Column<int>(nullable: false),
            //        AnswerHash = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UserSecurityAnswers", x => x.UserSecurityAnswerID);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "BackupCode");
            migrationBuilder.DropTable(name: "PasswordResetTokens");
            migrationBuilder.DropTable(name: "SecurityQuestions");
            migrationBuilder.DropTable(name: "UserSecurityAnswers");
            migrationBuilder.DropTable(name: "TwoFactorCode");
            migrationBuilder.DropTable(name: "TwoFactorSettings");
        }
    }
}
