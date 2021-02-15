﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RBC_GAM.Data;

namespace RBC_GAM.Migrations
{
    [DbContext(typeof(FinInstContext))]
    [Migration("20210215031318_FinancialInstrumentId_to_FinIntrumentId")]
    partial class FinancialInstrumentId_to_FinIntrumentId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("RBC_GAM.Model.FinancialInstrument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("CurrentPrice")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("FinancialInstrument");
                });

            modelBuilder.Entity("RBC_GAM.Model.FinancialInstrumentUser", b =>
                {
                    b.Property<int>("FinInstrumentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("FinInstrumentId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("FinancialInstrumentUser");
                });

            modelBuilder.Entity("RBC_GAM.Model.Threshold", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FinInstrumentId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FinancialInstrumentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FinancialInstrumentId");

                    b.HasIndex("UserId");

                    b.ToTable("Threshold");
                });

            modelBuilder.Entity("RBC_GAM.Model.Trigger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Action")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Direction")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Fluctuation")
                        .HasColumnType("REAL");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("ThresholdId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ThresholdId");

                    b.ToTable("Trigger");
                });

            modelBuilder.Entity("RBC_GAM.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("RBC_GAM.Model.FinancialInstrumentUser", b =>
                {
                    b.HasOne("RBC_GAM.Model.FinancialInstrument", "FinancialInstrument")
                        .WithMany("FinancialInstrumentUsers")
                        .HasForeignKey("FinInstrumentId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("RBC_GAM.Model.User", "User")
                        .WithMany("FinancialInstrumentUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("FinancialInstrument");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RBC_GAM.Model.Threshold", b =>
                {
                    b.HasOne("RBC_GAM.Model.FinancialInstrument", "FinancialInstrument")
                        .WithMany()
                        .HasForeignKey("FinancialInstrumentId");

                    b.HasOne("RBC_GAM.Model.User", "User")
                        .WithMany("Thresholds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FinancialInstrument");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RBC_GAM.Model.Trigger", b =>
                {
                    b.HasOne("RBC_GAM.Model.Threshold", "Threshold")
                        .WithMany("Triggers")
                        .HasForeignKey("ThresholdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Threshold");
                });

            modelBuilder.Entity("RBC_GAM.Model.FinancialInstrument", b =>
                {
                    b.Navigation("FinancialInstrumentUsers");
                });

            modelBuilder.Entity("RBC_GAM.Model.Threshold", b =>
                {
                    b.Navigation("Triggers");
                });

            modelBuilder.Entity("RBC_GAM.Model.User", b =>
                {
                    b.Navigation("FinancialInstrumentUsers");

                    b.Navigation("Thresholds");
                });
#pragma warning restore 612, 618
        }
    }
}