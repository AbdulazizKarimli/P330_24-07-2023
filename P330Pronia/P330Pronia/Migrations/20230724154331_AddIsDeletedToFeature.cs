using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P330Pronia.Migrations
{
    public partial class AddIsDeletedToFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Features",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Features");
        }
    }
}
