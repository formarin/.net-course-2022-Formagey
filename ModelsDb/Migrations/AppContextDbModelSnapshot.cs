﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModelsDb;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ModelsDb.Migrations
{
    [DbContext(typeof(AppContextDb))]
    partial class AppContextDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<Guid?>("ClientDbId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid")
                        .HasColumnName("client_id");

                    b.Property<string>("CurrencyName")
                        .HasColumnType("text")
                        .HasColumnName("currency_name");

                    b.HasKey("Id");

                    b.HasIndex("ClientDbId");

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
                        .WithMany("AccountDbCollection")
                        .HasForeignKey("ClientDbId");

                    b.Navigation("ClientDb");
                });

            modelBuilder.Entity("ModelsDb.ClientDb", b =>
                {
                    b.Navigation("AccountDbCollection");
                });
#pragma warning restore 612, 618
        }
    }
}
