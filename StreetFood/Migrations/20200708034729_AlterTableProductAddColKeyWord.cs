using Microsoft.EntityFrameworkCore.Migrations;

namespace StreetFood.Migrations
{
    public partial class AlterTableProductAddColKeyWord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeySearch",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeySearch",
                table: "Products");
        }
    }
}
