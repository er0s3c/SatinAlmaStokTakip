﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SatinAlmaStokTakip.Models;

#nullable disable

namespace SatinAlmaStokTakip.Migrations
{
    [DbContext(typeof(VeritabaniContext))]
    [Migration("20250712054051_StokTablosu")]
    partial class StokTablosu
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("SatinAlmaStokTakip.Models.Fatura", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("FaturaNo")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FaturaTarihi")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("TeklifID")
                        .HasColumnType("int");

                    b.Property<decimal>("Tutar")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("ID");

                    b.ToTable("Faturalar");
                });

            modelBuilder.Entity("SatinAlmaStokTakip.Models.Kullanici", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("AdSoyad")
                        .HasColumnType("longtext");

                    b.Property<string>("Rol")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Kullanicilar");
                });

            modelBuilder.Entity("SatinAlmaStokTakip.Models.Stok", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("Adet")
                        .HasColumnType("int");

                    b.Property<DateTime>("GirisTarihi")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("MalzemeAdi")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Stoklar");
                });

            modelBuilder.Entity("SatinAlmaStokTakip.Models.Talep", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Durum")
                        .HasColumnType("longtext");

                    b.Property<int>("KullaniciID")
                        .HasColumnType("int");

                    b.Property<DateTime>("TalepTarihi")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.ToTable("Talepler");
                });

            modelBuilder.Entity("SatinAlmaStokTakip.Models.Teklif", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("FirmaAdi")
                        .HasColumnType("longtext");

                    b.Property<decimal>("Fiyat")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Notlar")
                        .HasColumnType("longtext");

                    b.Property<int>("TalepID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("TalepID");

                    b.ToTable("Teklifler");
                });

            modelBuilder.Entity("SatinAlmaStokTakip.Models.Teklif", b =>
                {
                    b.HasOne("SatinAlmaStokTakip.Models.Talep", null)
                        .WithMany("Teklifler")
                        .HasForeignKey("TalepID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SatinAlmaStokTakip.Models.Talep", b =>
                {
                    b.Navigation("Teklifler");
                });
#pragma warning restore 612, 618
        }
    }
}
