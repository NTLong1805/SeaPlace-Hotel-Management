using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstProjectNET.Migrations
{
    /// <inheritdoc />
    public partial class addcolumnimagetoCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageID",
                table: "Customer",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ImageID",
                table: "Customer",
                column: "ImageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Image_ImageID",
                table: "Customer",
                column: "ImageID",
                principalTable: "Image",
                principalColumn: "ImageID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Image_ImageID",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_ImageID",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ImageID",
                table: "Customer");
        }
    }
}
