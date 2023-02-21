﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockResearchPlatform.Data;

#nullable disable

namespace StockResearchPlatform.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230221213421_update-mutual-fund")]
    partial class updatemutualfund
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("StockResearchPlatform.Models.MutualFundClass", b =>
                {
                    b.Property<string>("Ticker")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ClassID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("SeriesID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Ticker");

                    b.ToTable("MutualFunds");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.Stock", b =>
                {
                    b.Property<string>("Ticker")
                        .HasColumnType("varchar(255)");

                    b.Property<ulong>("CIK")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Ticker");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.MutualFundClass", b =>
                {
                    b.HasOne("StockResearchPlatform.Models.Stock", "Stock")
                        .WithOne("MutualFund")
                        .HasForeignKey("StockResearchPlatform.Models.MutualFundClass", "Ticker")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StockResearchPlatform.Models.Stock", b =>
                {
                    b.Navigation("MutualFund")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
