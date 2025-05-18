using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Configurator.API.Migrations
{
    /// <inheritdoc />
    public partial class SamboGearTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sambogears",
                schema: "configurator",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    modelcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    family = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    sizecode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    outputtorquenm = table.Column<int>(type: "integer", nullable: true),
                    maxthrustn = table.Column<int>(type: "integer", nullable: true),
                    stemboremm = table.Column<decimal>(type: "numeric", nullable: true),
                    stemboreinches = table.Column<decimal>(type: "numeric", nullable: true),
                    turnstoopen = table.Column<int>(type: "integer", nullable: true),
                    efficiencyrun = table.Column<decimal>(type: "numeric", nullable: true),
                    efficiencystall = table.Column<decimal>(type: "numeric", nullable: true),
                    handwheeldiametermm = table.Column<int>(type: "integer", nullable: true),
                    weightkg = table.Column<decimal>(type: "numeric", nullable: true),
                    keysize = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    mountingiso = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    createdbyid = table.Column<int>(type: "integer", nullable: false),
                    createddate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    modifiedbyid = table.Column<int>(type: "integer", nullable: false),
                    modifieddate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sambogears", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sambogearmountingbases",
                schema: "configurator",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sambogearid = table.Column<int>(type: "integer", nullable: false),
                    mountingbasecode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    createdbyid = table.Column<int>(type: "integer", nullable: false),
                    createddate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    modifiedbyid = table.Column<int>(type: "integer", nullable: false),
                    modifieddate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sambogearmountingbases", x => x.id);
                    table.ForeignKey(
                        name: "fk_sambogearmountingbases_sambogears_sambogearid",
                        column: x => x.sambogearid,
                        principalSchema: "configurator",
                        principalTable: "sambogears",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_sambogearmountingbases_sambogearid",
                schema: "configurator",
                table: "sambogearmountingbases",
                column: "sambogearid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sambogearmountingbases",
                schema: "configurator");

            migrationBuilder.DropTable(
                name: "sambogears",
                schema: "configurator");
        }
    }
}
