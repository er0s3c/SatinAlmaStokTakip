﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatinAlmaStokTakip.Migrations
{
    /// <inheritdoc />
    public partial class StokBirimFiyatEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BirimFiyat",
                table: "Stoklar",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirimFiyat",
                table: "Stoklar");
        }
    }
}
