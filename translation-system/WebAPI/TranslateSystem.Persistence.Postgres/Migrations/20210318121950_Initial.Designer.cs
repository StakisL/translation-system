﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TranslateSystem.Persistence;

namespace TranslateSystem.Persistence.Postgre.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20210318121950_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("TranslateSystem.Data.AccountData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<double>("Balance")
                        .HasColumnType("double precision")
                        .HasColumnName("balance");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("integer")
                        .HasColumnName("currency_id");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("account_data");
                });

            modelBuilder.Entity("TranslateSystem.Data.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("CurrencyType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("currency_type");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("last_update");

                    b.Property<double>("Ratio")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("currency");
                });

            modelBuilder.Entity("TranslateSystem.Data.TransferDataRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<double>("AmountTransaction")
                        .HasColumnType("double precision")
                        .HasColumnName("amount_transaction");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date");

                    b.Property<string>("DestinationCurrencyType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("destination_currency");

                    b.Property<int>("DestinationUserId")
                        .HasColumnType("integer")
                        .HasColumnName("destination_id");

                    b.Property<bool>("IsCancelled")
                        .HasColumnType("boolean")
                        .HasColumnName("is_cancelled");

                    b.Property<string>("SourceCurrencyType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("source_currency");

                    b.Property<int>("SourceUserId")
                        .HasColumnType("integer")
                        .HasColumnName("source_id");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("transfer_data");
                });

            modelBuilder.Entity("TranslateSystem.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FullName")
                        .HasColumnType("text")
                        .HasColumnName("full_name");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("TranslateSystem.Data.AccountData", b =>
                {
                    b.HasOne("TranslateSystem.Data.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TranslateSystem.Data.User", "User")
                        .WithOne("AccountData")
                        .HasForeignKey("TranslateSystem.Data.AccountData", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TranslateSystem.Data.TransferDataRequest", b =>
                {
                    b.HasOne("TranslateSystem.Data.User", null)
                        .WithMany("Transfers")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TranslateSystem.Data.User", b =>
                {
                    b.Navigation("AccountData");

                    b.Navigation("Transfers");
                });
#pragma warning restore 612, 618
        }
    }
}
