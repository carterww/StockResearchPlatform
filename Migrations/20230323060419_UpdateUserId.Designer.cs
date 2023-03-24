﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockResearchPlatform.Data;

#nullable disable

namespace StockResearchPlatform.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230323060419_UpdateUserId")]
    partial class UpdateUserId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("StockResearchPlatform.Models.DividendLedger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("FK_UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FK_UserId");

                    b.ToTable("DividendLedgers");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.MutualFundClass", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("ClassID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("SeriesID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("MutualFunds");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.Portfolio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("FK_UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FK_UserId");

                    b.ToTable("Portfolios");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.Session", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("Creation")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("FK_UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastAccessed")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("FK_UserId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.Stock", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<ulong>("CIK")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.StockDividendLedger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("Amount")
                        .HasColumnType("double");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("FK_DividendLedgerId")
                        .HasColumnType("int");

                    b.Property<Guid>("FK_StockId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("FK_DividendLedgerId");

                    b.HasIndex("FK_StockId");

                    b.ToTable("StockDividendLedgers");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.StockPortfolio", b =>
                {
                    b.Property<Guid>("FK_Stock")
                        .HasColumnType("char(36)");

                    b.Property<int>("FK_Portfolio")
                        .HasColumnType("int");

                    b.Property<double>("CostBasis")
                        .HasColumnType("double");

                    b.Property<int>("NumberOfShares")
                        .HasColumnType("int");

                    b.HasKey("FK_Stock", "FK_Portfolio");

                    b.HasIndex("FK_Portfolio");

                    b.ToTable("StockPortfolios");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.DividendLedger", b =>
                {
                    b.HasOne("StockResearchPlatform.Models.User", "FK_User")
                        .WithMany("DividendLedgers")
                        .HasForeignKey("FK_UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FK_User");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.MutualFundClass", b =>
                {
                    b.HasOne("StockResearchPlatform.Models.Stock", "Stock")
                        .WithOne("MutualFund")
                        .HasForeignKey("StockResearchPlatform.Models.MutualFundClass", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.Portfolio", b =>
                {
                    b.HasOne("StockResearchPlatform.Models.User", "FK_User")
                        .WithMany("Portfolios")
                        .HasForeignKey("FK_UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FK_User");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.Session", b =>
                {
                    b.HasOne("StockResearchPlatform.Models.User", "FK_User")
                        .WithMany()
                        .HasForeignKey("FK_UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FK_User");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.StockDividendLedger", b =>
                {
                    b.HasOne("StockResearchPlatform.Models.DividendLedger", "FK_DividendLedger")
                        .WithMany("stockDividendLedgers")
                        .HasForeignKey("FK_DividendLedgerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StockResearchPlatform.Models.Stock", "FK_Stock")
                        .WithMany("StockDividendLedgers")
                        .HasForeignKey("FK_StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FK_DividendLedger");

                    b.Navigation("FK_Stock");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.StockPortfolio", b =>
                {
                    b.HasOne("StockResearchPlatform.Models.Portfolio", "Portfolio")
                        .WithMany("StockPortfolios")
                        .HasForeignKey("FK_Portfolio")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StockResearchPlatform.Models.Stock", "Stock")
                        .WithMany("StockPortfolios")
                        .HasForeignKey("FK_Stock")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Portfolio");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.DividendLedger", b =>
                {
                    b.Navigation("stockDividendLedgers");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.Portfolio", b =>
                {
                    b.Navigation("StockPortfolios");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.Stock", b =>
                {
                    b.Navigation("MutualFund")
                        .IsRequired();

                    b.Navigation("StockDividendLedgers");

                    b.Navigation("StockPortfolios");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.User", b =>
                {
                    b.Navigation("DividendLedgers");

                    b.Navigation("Portfolios");
                });
#pragma warning restore 612, 618
        }
    }
}
