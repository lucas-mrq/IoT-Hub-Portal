#nullable disable

namespace AzureIoTHub.Portal.Postgres.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class AddLayersPlanningSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<string>(
                name: "LayerId",
                table: "Devices",
                type: "text",
                nullable: true);

            _ = migrationBuilder.CreateTable(
                name: "Layers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Father = table.Column<string>(type: "text", nullable: false),
                    Planning = table.Column<string>(type: "text", nullable: false),
                    hasSub = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Layers", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Plannings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Start = table.Column<string>(type: "text", nullable: false),
                    End = table.Column<string>(type: "text", nullable: false),
                    Frequency = table.Column<bool>(type: "boolean", nullable: false),
                    DayOff = table.Column<string>(type: "text", nullable: false),
                    CommandId = table.Column<string>(type: "text", nullable: false),
                    DayExceptions = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Plannings", x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Start = table.Column<string>(type: "text", nullable: false),
                    End = table.Column<string>(type: "text", nullable: false),
                    CommandId = table.Column<string>(type: "text", nullable: false),
                    PlanningId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Schedules", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "Layers");

            _ = migrationBuilder.DropTable(
                name: "Plannings");

            _ = migrationBuilder.DropTable(
                name: "Schedules");

            _ = migrationBuilder.DropColumn(
                name: "LayerId",
                table: "Devices");
        }
    }
}
