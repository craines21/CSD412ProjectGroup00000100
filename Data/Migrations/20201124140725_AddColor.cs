using Microsoft.EntityFrameworkCore.Migrations;

namespace CSD412ProjectGroup00000100.Data.Migrations
{
    public partial class AddColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorValue",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorValue",
                table: "Items");
        }
    }
}
