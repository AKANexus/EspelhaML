﻿// <auto-generated />
using System;
using MlSuite.EntityFramework.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MlSynch.Migrations
{
    [DbContext(typeof(TrilhaDbContext))]
    [Migration("20230906151201_DestinatarioNullable")]
    partial class DestinatarioNullable
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

                    b.Property<string>("AccountNickname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AccountRegistry")
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

            modelBuilder.Entity("EspelhaML.Domain.Pedido", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("EnvioUuid")
                        .HasColumnType("uuid");

                    b.Property<decimal?>("Frete")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Uuid");

                    b.HasIndex("EnvioUuid");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("EspelhaML.Domain.PedidoDestinatário", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CEP")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Distrito")
                        .HasColumnType("text");

                    b.Property<decimal>("Id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Número")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UF")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("ÉResidencial")
                        .HasColumnType("boolean");

                    b.HasKey("Uuid");

                    b.ToTable("PedidoDestinatário");
                });

            modelBuilder.Entity("EspelhaML.Domain.PedidoEnvio", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal?>("Altura")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("Comprimento")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("CriaçãoDoPedido")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CódRastreamento")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("DestinatárioUuid")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal?>("Largura")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("Peso")
                        .HasColumnType("numeric");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int?>("SubStatus")
                        .HasColumnType("integer");

                    b.Property<string>("SubStatusDescrição")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TipoEnvio")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal?>("ValorDeclarado")
                        .HasColumnType("numeric");

                    b.HasKey("Uuid");

                    b.HasIndex("DestinatárioUuid");

                    b.ToTable("PedidoEnvio");
                });

            modelBuilder.Entity("EspelhaML.Domain.PedidoItem", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DescritorVariação")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ItemUuid")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ItemVariaçãoUuid")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PedidoUuid")
                        .HasColumnType("uuid");

                    b.Property<decimal>("PreçoUnitário")
                        .HasColumnType("numeric");

                    b.Property<int>("QuantidadeVendida")
                        .HasColumnType("integer");

                    b.Property<string>("Título")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Uuid");

                    b.HasIndex("ItemUuid");

                    b.HasIndex("ItemVariaçãoUuid");

                    b.HasIndex("PedidoUuid");

                    b.ToTable("PedidoItem");
                });

            modelBuilder.Entity("EspelhaML.Domain.PedidoPagamento", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("Id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<int>("Parcelas")
                        .HasColumnType("integer");

                    b.Property<Guid?>("PedidoUuid")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TotalPago")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("ValorFrete")
                        .HasColumnType("numeric");

                    b.Property<decimal>("ValorRessarcido")
                        .HasColumnType("numeric");

                    b.Property<decimal>("ValorTransação")
                        .HasColumnType("numeric");

                    b.HasKey("Uuid");

                    b.HasIndex("PedidoUuid");

                    b.ToTable("PedidoPagamento");
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
                        .WithMany("Variações")
                        .HasForeignKey("ItemUuid");
                });

            modelBuilder.Entity("EspelhaML.Domain.Pedido", b =>
                {
                    b.HasOne("EspelhaML.Domain.PedidoEnvio", "Envio")
                        .WithMany()
                        .HasForeignKey("EnvioUuid");

                    b.Navigation("Envio");
                });

            modelBuilder.Entity("EspelhaML.Domain.PedidoEnvio", b =>
                {
                    b.HasOne("EspelhaML.Domain.PedidoDestinatário", "Destinatário")
                        .WithMany()
                        .HasForeignKey("DestinatárioUuid");

                    b.Navigation("Destinatário");
                });

            modelBuilder.Entity("EspelhaML.Domain.PedidoItem", b =>
                {
                    b.HasOne("EspelhaML.Domain.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemUuid");

                    b.HasOne("EspelhaML.Domain.ItemVariação", "ItemVariação")
                        .WithMany()
                        .HasForeignKey("ItemVariaçãoUuid");

                    b.HasOne("EspelhaML.Domain.Pedido", null)
                        .WithMany("Itens")
                        .HasForeignKey("PedidoUuid");

                    b.Navigation("Item");

                    b.Navigation("ItemVariação");
                });

            modelBuilder.Entity("EspelhaML.Domain.PedidoPagamento", b =>
                {
                    b.HasOne("EspelhaML.Domain.Pedido", null)
                        .WithMany("Pagamentos")
                        .HasForeignKey("PedidoUuid");
                });

            modelBuilder.Entity("EspelhaML.Domain.Item", b =>
                {
                    b.Navigation("Variações");
                });

            modelBuilder.Entity("EspelhaML.Domain.Pedido", b =>
                {
                    b.Navigation("Itens");

                    b.Navigation("Pagamentos");
                });
#pragma warning restore 612, 618
        }
    }
}
