﻿// <auto-generated />
using System;
using BioShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BioShop.Migrations
{
    [DbContext(typeof(BioShopDataContext))]
    [Migration("20230612073944_RecipeCurrentProductIsOptionalAdded")]
    partial class RecipeCurrentProductIsOptionalAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BioShop.Data.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.Property<string>("Ingredients")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MadeInCountry")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("BioShop.Data.Models.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CurrentProductId")
                        .HasColumnType("int");

                    b.Property<string>("DesciptionStepByStepHowToBeMade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NecesseryProductsAndQuantity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Portions")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Size")
                        .HasColumnType("float");

                    b.Property<double>("TimeYouNeedToBeMade")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CurrentProductId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("BioShop.Data.Models.Recipe", b =>
                {
                    b.HasOne("BioShop.Data.Models.Product", "CurrentProduct")
                        .WithMany("Recipes")
                        .HasForeignKey("CurrentProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("CurrentProduct");
                });

            modelBuilder.Entity("BioShop.Data.Models.Product", b =>
                {
                    b.Navigation("Recipes");
                });
#pragma warning restore 612, 618
        }
    }
}
