using Microsoft.EntityFrameworkCore.Migrations;

namespace ASDDbContext.Migrations
{
    public partial class ColorHEx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorHex",
                table: "BusLines",
                unicode: false,
                maxLength: 7,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorHex",
                table: "BusLines");
        }
    }
}
