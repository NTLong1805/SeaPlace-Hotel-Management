using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstProjectNET.Migrations
{
    /// <inheritdoc />
    public partial class addAccountLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountLogin",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountLogin", x => new { x.AccountID, x.LoginProvider });
                    table.ForeignKey(
                        name: "FK_AccountLogin_Account_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountLogin");
        }
    }
}
