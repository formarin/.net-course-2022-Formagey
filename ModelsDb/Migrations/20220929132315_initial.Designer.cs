﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModelsDb;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ModelsDb.Migrations
{
    [DbContext(typeof(ApplicationContextDb))]
    [Migration("20220929132315_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("ModelsDb.AccountDb", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision")
                        .HasColumnName("amount");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid")
                        .HasColumnName("client_id");

                    b.Property<string>("CurrencyName")
                        .HasColumnType("text")
                        .HasColumnName("currency_name");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("account");
                });

            modelBuilder.Entity("ModelsDb.ClientDb", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("BonusCount")
                        .HasColumnType("integer")
                        .HasColumnName("bonus_count");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<int>("PassportNumber")
                        .HasColumnType("integer")
                        .HasColumnName("passport_number");

                    b.Property<int>("PhoneNumber")
                        .HasColumnType("integer")
                        .HasColumnName("phone_number");

                    b.HasKey("Id");

                    b.ToTable("client");
                });

            modelBuilder.Entity("ModelsDb.EmployeeDb", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("BonusCount")
                        .HasColumnType("integer")
                        .HasColumnName("bonus_count");

                    b.Property<string>("Contract")
                        .HasColumnType("text")
                        .HasColumnName("contract");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<int>("PassportNumber")
                        .HasColumnType("integer")
                        .HasColumnName("passport_number");

                    b.Property<int>("PhoneNumber")
                        .HasColumnType("integer")
                        .HasColumnName("phone_number");

                    b.Property<int>("Salary")
                        .HasColumnType("integer")
                        .HasColumnName("salary");

                    b.HasKey("Id");

                    b.ToTable("employee");
                });

            modelBuilder.Entity("ModelsDb.AccountDb", b =>
                {
                    b.HasOne("ModelsDb.ClientDb", "ClientDb")
                        .WithMany("AccountCollection")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientDb");
                });

            modelBuilder.Entity("ModelsDb.ClientDb", b =>
                {
                    b.Navigation("AccountCollection");
                });
#pragma warning restore 612, 618
        }
    }
}
