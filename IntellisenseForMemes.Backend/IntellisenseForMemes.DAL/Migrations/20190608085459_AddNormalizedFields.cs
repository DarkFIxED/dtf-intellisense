using Microsoft.EntityFrameworkCore.Migrations;

namespace IntellisenseForMemes.DAL.Migrations
{
    public partial class AddNormalizedFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Memes",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE Memes SET
                    NormalizedName = Name");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedAlias",
                table: "MemeAliases",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE MemeAliases SET
                    NormalizedAlias = Alias");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Memes");

            migrationBuilder.DropColumn(
                name: "NormalizedAlias",
                table: "MemeAliases");
        }
    }
}
