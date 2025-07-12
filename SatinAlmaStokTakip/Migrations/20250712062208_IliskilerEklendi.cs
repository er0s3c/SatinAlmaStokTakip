using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatinAlmaStokTakip.Migrations
{
    /// <inheritdoc />
    public partial class IliskilerEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Loglar",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KullaniciAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Islem = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tarih = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loglar", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tuketimler",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StokID = table.Column<int>(type: "int", nullable: false),
                    Miktar = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Aciklama = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tuketimler", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tuketimler_Stoklar_StokID",
                        column: x => x.StokID,
                        principalTable: "Stoklar",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Talepler_KullaniciID",
                table: "Talepler",
                column: "KullaniciID");

            migrationBuilder.CreateIndex(
                name: "IX_Faturalar_TeklifID",
                table: "Faturalar",
                column: "TeklifID");

            migrationBuilder.CreateIndex(
                name: "IX_Tuketimler_StokID",
                table: "Tuketimler",
                column: "StokID");

            migrationBuilder.AddForeignKey(
                name: "FK_Faturalar_Teklifler_TeklifID",
                table: "Faturalar",
                column: "TeklifID",
                principalTable: "Teklifler",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Talepler_Kullanicilar_KullaniciID",
                table: "Talepler",
                column: "KullaniciID",
                principalTable: "Kullanicilar",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faturalar_Teklifler_TeklifID",
                table: "Faturalar");

            migrationBuilder.DropForeignKey(
                name: "FK_Talepler_Kullanicilar_KullaniciID",
                table: "Talepler");

            migrationBuilder.DropTable(
                name: "Loglar");

            migrationBuilder.DropTable(
                name: "Tuketimler");

            migrationBuilder.DropIndex(
                name: "IX_Talepler_KullaniciID",
                table: "Talepler");

            migrationBuilder.DropIndex(
                name: "IX_Faturalar_TeklifID",
                table: "Faturalar");
        }
    }
}
