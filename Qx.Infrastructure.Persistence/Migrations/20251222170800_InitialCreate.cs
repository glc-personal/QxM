using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qx.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsumableType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    Rows = table.Column<int>(type: "integer", nullable: true),
                    Columns = table.Column<int>(type: "integer", nullable: true),
                    GeometryJson = table.Column<string>(type: "text", nullable: true),
                    DefaultIsReusable = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultMaxUses = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumableType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "location",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    XUs = table.Column<double>(type: "double precision", nullable: false),
                    YUs = table.Column<double>(type: "double precision", nullable: false),
                    ZUs = table.Column<double>(type: "double precision", nullable: false),
                    Frame = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "consumable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    TypeId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    IsReusable = table.Column<bool>(type: "boolean", nullable: false),
                    MaxUses = table.Column<int>(type: "integer", nullable: true),
                    Uses = table.Column<int>(type: "integer", nullable: false),
                    Barcode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consumable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_consumable_ConsumableType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ConsumableType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_consumable_inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_consumable_location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "consumable_column",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    ConsumableId = table.Column<Guid>(type: "uuid", nullable: false),
                    VolumeCapacityUl = table.Column<string>(type: "jsonb", nullable: false),
                    CurrentVolumeUl = table.Column<string>(type: "jsonb", nullable: false),
                    IsReusable = table.Column<bool>(type: "boolean", nullable: false),
                    MaxUses = table.Column<int>(type: "integer", nullable: true),
                    Uses = table.Column<int>(type: "integer", nullable: false),
                    IsSealed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consumable_column", x => x.Id);
                    table.ForeignKey(
                        name: "FK_consumable_column_consumable_ConsumableId",
                        column: x => x.ConsumableId,
                        principalTable: "consumable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_consumable_Barcode",
                table: "consumable",
                column: "Barcode");

            migrationBuilder.CreateIndex(
                name: "IX_consumable_InventoryId_Name",
                table: "consumable",
                columns: new[] { "InventoryId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_consumable_InventoryId_TypeId",
                table: "consumable",
                columns: new[] { "InventoryId", "TypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_consumable_LocationId",
                table: "consumable",
                column: "LocationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consumable_TypeId",
                table: "consumable",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_consumable_column_ConsumableId_Name",
                table: "consumable_column",
                columns: new[] { "ConsumableId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_location_Name",
                table: "location",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "consumable_column");

            migrationBuilder.DropTable(
                name: "consumable");

            migrationBuilder.DropTable(
                name: "ConsumableType");

            migrationBuilder.DropTable(
                name: "inventory");

            migrationBuilder.DropTable(
                name: "location");
        }
    }
}
