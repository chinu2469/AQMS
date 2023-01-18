using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AQMS.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aQMSdatas",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    floor = table.Column<int>(type: "int", nullable: false),
                    O2 = table.Column<int>(type: "int", nullable: false),
                    co2 = table.Column<int>(type: "int", nullable: false),
                    SO2 = table.Column<int>(type: "int", nullable: false),
                    CO = table.Column<int>(type: "int", nullable: false),
                    C = table.Column<int>(type: "int", nullable: false),
                    Temp = table.Column<int>(type: "int", nullable: false),
                    PM = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aQMSdatas", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aQMSdatas");
        }
    }
}
