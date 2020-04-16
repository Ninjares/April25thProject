using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPDbContext.Migrations
{
    public partial class ExtraLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationRoleId",
                table: "AspNetUserRoles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "AspNetUserRoles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BusId",
                table: "AspNetUsers",
                column: "BusId",
                unique: true,
                filter: "[BusId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_ApplicationRoleId",
                table: "AspNetUserRoles",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_ApplicationUserId",
                table: "AspNetUserRoles",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_ApplicationRoleId",
                table: "AspNetUserRoles",
                column: "ApplicationRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_ApplicationUserId",
                table: "AspNetUserRoles",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Buses_BusId",
                table: "AspNetUsers",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "BusId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_ApplicationRoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_ApplicationUserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Buses_BusId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BusId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_ApplicationRoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_ApplicationUserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "BusId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationRoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "AspNetUserRoles");
        }
    }
}
