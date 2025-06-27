using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BidService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBidModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "material",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "technique",
                table: "Bids");

            migrationBuilder.RenameColumn(
                name: "userName",
                table: "Bids",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "result",
                table: "Bids",
                newName: "Result");

            migrationBuilder.CreateTable(
                name: "RequiredMaterials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BidId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequiredMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequiredMaterials_Bids_BidId",
                        column: x => x.BidId,
                        principalTable: "Bids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequiredTechniques",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BidId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequiredTechniques", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequiredTechniques_Bids_BidId",
                        column: x => x.BidId,
                        principalTable: "Bids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequiredMaterials_BidId",
                table: "RequiredMaterials",
                column: "BidId");

            migrationBuilder.CreateIndex(
                name: "IX_RequiredTechniques_BidId",
                table: "RequiredTechniques",
                column: "BidId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequiredMaterials");

            migrationBuilder.DropTable(
                name: "RequiredTechniques");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Bids",
                newName: "userName");

            migrationBuilder.RenameColumn(
                name: "Result",
                table: "Bids",
                newName: "result");

            migrationBuilder.AddColumn<string>(
                name: "material",
                table: "Bids",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "technique",
                table: "Bids",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
