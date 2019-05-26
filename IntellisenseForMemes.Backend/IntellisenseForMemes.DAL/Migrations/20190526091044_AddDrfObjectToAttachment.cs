using Microsoft.EntityFrameworkCore.Migrations;

namespace IntellisenseForMemes.DAL.Migrations
{
    public partial class AddDrfObjectToAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ObjectFromDtfInJson",
                table: "Attachments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObjectFromDtfInJson",
                table: "Attachments");
        }
    }
}
