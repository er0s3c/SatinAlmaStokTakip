using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatinAlmaStokTakip.Migrations
{
    /// <inheritdoc />
    public partial class TalepTeklifIliskisi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Teklifler_TalepID",
                table: "Teklifler",
                column: "TalepID");

            migrationBuilder.AddForeignKey(
                name: "FK_Teklifler_Talepler_TalepID",
                table: "Teklifler",
                column: "TalepID",
                principalTable: "Talepler",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teklifler_Talepler_TalepID",
                table: "Teklifler");

            migrationBuilder.DropIndex(
                name: "IX_Teklifler_TalepID",
                table: "Teklifler");
        }
    }
}
