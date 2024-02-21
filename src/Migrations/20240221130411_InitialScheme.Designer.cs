﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RandomLoadoutGenerator.Database;

#nullable disable

namespace RandomLoadoutGenerator.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240221130411_InitialScheme")]
    partial class InitialScheme
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("LoadoutCombinationWeapon", b =>
                {
                    b.Property<int>("LoadoutCombosID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WeaponsID")
                        .HasColumnType("INTEGER");

                    b.HasKey("LoadoutCombosID", "WeaponsID");

                    b.HasIndex("WeaponsID");

                    b.ToTable("LoadoutCombinationWeapon");
                });

            modelBuilder.Entity("RandomLoadoutGenerator.Models.LoadoutCombination", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Class")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Slot")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.ToTable("LoadoutCombinations");
                });

            modelBuilder.Entity("RandomLoadoutGenerator.Models.ReskinGroup", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("ReskinGroups");
                });

            modelBuilder.Entity("RandomLoadoutGenerator.Models.Weapon", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageURI")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsStock")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ReskinGroupID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("ReskinGroupID");

                    b.ToTable("Weapons");
                });

            modelBuilder.Entity("LoadoutCombinationWeapon", b =>
                {
                    b.HasOne("RandomLoadoutGenerator.Models.LoadoutCombination", null)
                        .WithMany()
                        .HasForeignKey("LoadoutCombosID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RandomLoadoutGenerator.Models.Weapon", null)
                        .WithMany()
                        .HasForeignKey("WeaponsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RandomLoadoutGenerator.Models.Weapon", b =>
                {
                    b.HasOne("RandomLoadoutGenerator.Models.ReskinGroup", "ReskinGroup")
                        .WithMany("Weapons")
                        .HasForeignKey("ReskinGroupID");

                    b.Navigation("ReskinGroup");
                });

            modelBuilder.Entity("RandomLoadoutGenerator.Models.ReskinGroup", b =>
                {
                    b.Navigation("Weapons");
                });
#pragma warning restore 612, 618
        }
    }
}
