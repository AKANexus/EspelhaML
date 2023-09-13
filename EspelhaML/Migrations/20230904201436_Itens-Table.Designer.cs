﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MlSuite.EntityFramework.EntityFramework;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MlSynch.Migrations
{
    [DbContext(typeof(TrilhaDbContext))]
    [Migration("20230904201436_Itens-Table")]
    partial class ItensTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EspelhaML.Domain.EspelhoLog", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Caller")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Uuid");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("EspelhaML.Domain.Item", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Permalink")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("PreçoVenda")
                        .HasColumnType("numeric");

                    b.Property<string>("PrimeiraFoto")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("QuantidadeÀVenda")
                        .HasColumnType("integer");

                    b.Property<decimal>("SellerId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Título")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("ÉVariação")
                        .HasColumnType("boolean");

                    b.HasKey("Uuid");

                    b.ToTable("Itens");
                });

            modelBuilder.Entity("EspelhaML.Domain.ItemVariação", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DescritorVariação")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<Guid?>("ItemUuid")
                        .HasColumnType("uuid");

                    b.Property<decimal>("PreçoVenda")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Uuid");

                    b.HasIndex("ItemUuid");

                    b.ToTable("ItemVariação");
                });

            modelBuilder.Entity("EspelhaML.Domain.MlUserAuthInfo", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ExpiresOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Uuid");

                    b.ToTable("MlUserAuthInfos");
                });

            modelBuilder.Entity("EspelhaML.Domain.Question", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AnswerStatus")
                        .HasColumnType("text");

                    b.Property<string>("AnswerText")
                        .HasColumnType("text");

                    b.Property<long>("AskerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateAsked")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DateReplied")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("ItemMlb")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("QuestionStatus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Uuid");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("EspelhaML.Domain.ItemVariação", b =>
                {
                    b.HasOne("EspelhaML.Domain.Item", null)
                        .WithMany("ItemVariação")
                        .HasForeignKey("ItemUuid");
                });

            modelBuilder.Entity("EspelhaML.Domain.Item", b =>
                {
                    b.Navigation("ItemVariação");
                });
#pragma warning restore 612, 618
        }
    }
}
