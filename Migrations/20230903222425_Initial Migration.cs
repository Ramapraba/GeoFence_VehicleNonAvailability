using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoFence_VehicleNonAvailability.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeoFencePeriods",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    vehicleid = table.Column<int>(type: "int", nullable: false),
                    entertime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    exittime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoFencePeriods", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoFencePeriods");
        }
    }
}
