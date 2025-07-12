using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatinAlmaStokTakip.Migrations
{
    /// <inheritdoc />
    public partial class YorumSistemiVeOnayDurumuEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OnayDurumu",
                table: "Teklifler",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OnayNotu",
                table: "Teklifler",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "OnayTarihi",
                table: "Teklifler",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OnaylayanKullaniciID",
                table: "Teklifler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OnayDurumu",
                table: "Talepler",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OnayNotu",
                table: "Talepler",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "OnayTarihi",
                table: "Talepler",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OnaylayanKullaniciID",
                table: "Talepler",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Yorumlar",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Icerik = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tarih = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    KullaniciID = table.Column<int>(type: "int", nullable: false),
                    Tip = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KayitID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TalepID = table.Column<int>(type: "int", nullable: true),
                    TeklifID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yorumlar", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Yorumlar_Kullanicilar_KullaniciID",
                        column: x => x.KullaniciID,
                        principalTable: "Kullanicilar",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Yorumlar_Talepler_TalepID",
                        column: x => x.TalepID,
                        principalTable: "Talepler",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Yorumlar_Teklifler_TeklifID",
                        column: x => x.TeklifID,
                        principalTable: "Teklifler",
                        principalColumn: "ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Teklifler_OnaylayanKullaniciID",
                table: "Teklifler",
                column: "OnaylayanKullaniciID");

            migrationBuilder.CreateIndex(
                name: "IX_Talepler_OnaylayanKullaniciID",
                table: "Talepler",
                column: "OnaylayanKullaniciID");

            migrationBuilder.CreateIndex(
                name: "IX_Yorumlar_KullaniciID",
                table: "Yorumlar",
                column: "KullaniciID");

            migrationBuilder.CreateIndex(
                name: "IX_Yorumlar_TalepID",
                table: "Yorumlar",
                column: "TalepID");

            migrationBuilder.CreateIndex(
                name: "IX_Yorumlar_TeklifID",
                table: "Yorumlar",
                column: "TeklifID");

            migrationBuilder.AddForeignKey(
                name: "FK_Talepler_Kullanicilar_OnaylayanKullaniciID",
                table: "Talepler",
                column: "OnaylayanKullaniciID",
                principalTable: "Kullanicilar",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Teklifler_Kullanicilar_OnaylayanKullaniciID",
                table: "Teklifler",
                column: "OnaylayanKullaniciID",
                principalTable: "Kullanicilar",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Talepler_Kullanicilar_OnaylayanKullaniciID",
                table: "Talepler");

            migrationBuilder.DropForeignKey(
                name: "FK_Teklifler_Kullanicilar_OnaylayanKullaniciID",
                table: "Teklifler");

            migrationBuilder.DropTable(
                name: "Yorumlar");

            migrationBuilder.DropIndex(
                name: "IX_Teklifler_OnaylayanKullaniciID",
                table: "Teklifler");

            migrationBuilder.DropIndex(
                name: "IX_Talepler_OnaylayanKullaniciID",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "OnayDurumu",
                table: "Teklifler");

            migrationBuilder.DropColumn(
                name: "OnayNotu",
                table: "Teklifler");

            migrationBuilder.DropColumn(
                name: "OnayTarihi",
                table: "Teklifler");

            migrationBuilder.DropColumn(
                name: "OnaylayanKullaniciID",
                table: "Teklifler");

            migrationBuilder.DropColumn(
                name: "OnayDurumu",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "OnayNotu",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "OnayTarihi",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "OnaylayanKullaniciID",
                table: "Talepler");
        }
    }
}
