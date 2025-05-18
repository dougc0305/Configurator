using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Configurator.API.Migrations
{
    /// <inheritdoc />
    public partial class WormGearOperatorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "wormgearoperators",
                schema: "configurator",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    partnumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    family = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    size = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    torqueratingnm = table.Column<int>(type: "integer", nullable: false),
                    mountingstandard = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    material = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    operationtype = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    handwheelincluded = table.Column<bool>(type: "boolean", nullable: false),
                    weatherproof = table.Column<bool>(type: "boolean", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    createdbyid = table.Column<int>(type: "integer", nullable: false),
                    createddate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    modifiedbyid = table.Column<int>(type: "integer", nullable: false),
                    modifieddate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wormgearoperators", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wormgearoperators",
                schema: "configurator");
        }
    }
}
