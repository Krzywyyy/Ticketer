using Microsoft.EntityFrameworkCore.Migrations;

namespace Ticketer.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Spectacles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spectacles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpectacleId = table.Column<int>(nullable: true),
                    Day = table.Column<string>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    Order = table.Column<string>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    Client = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Spectacles_SpectacleId",
                        column: x => x.SpectacleId,
                        principalTable: "Spectacles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SpectacleId",
                table: "Reservations",
                column: "SpectacleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Spectacles");
        }
    }
}
