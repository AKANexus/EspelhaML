﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MlSuite.EntityFramework.EntityFramework;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MlSynch.Migrations
{
    [DbContext(typeof(TrilhaDbContext))]
    partial class TrilhaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MlSuite.Domain.EspelhoLog", b =>
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

            modelBuilder.Entity("MlSuite.Domain.Item", b =>
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

                    b.Property<Guid>("SellerUuid")
                        .HasColumnType("uuid");

                    b.Property<string>("Título")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("ÉVariação")
                        .HasColumnType("boolean");

                    b.HasKey("Uuid");

                    b.HasAlternateKey("Id");

                    b.HasIndex("SellerUuid");

                    b.ToTable("Itens");
                });

            modelBuilder.Entity("MlSuite.Domain.ItemVariação", b =>
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

                    b.HasAlternateKey("Id");

                    b.HasIndex("ItemUuid");

                    b.ToTable("ItemVariação");
                });

            modelBuilder.Entity("MlSuite.Domain.MlUserAuthInfo", b =>
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

                    b.Property<decimal>("UserId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Uuid");

                    b.HasAlternateKey("UserId");

                    b.ToTable("MlUserAuthInfos");
                });

            modelBuilder.Entity("MlSuite.Domain.Pedido", b =>
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

                    b.Property<decimal?>("PackId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("SellerId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Uuid");

                    b.HasAlternateKey("Id");

                    b.HasIndex("EnvioUuid");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("MlSuite.Domain.PedidoDestinatário", b =>
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

                    b.Property<decimal?>("Id")
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

            modelBuilder.Entity("MlSuite.Domain.PedidoEnvio", b =>
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
                        .HasColumnType("text");

                    b.Property<int>("TipoEnvio")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal?>("ValorDeclarado")
                        .HasColumnType("numeric");

                    b.HasKey("Uuid");

                    b.HasAlternateKey("Id");

                    b.HasIndex("DestinatárioUuid");

                    b.ToTable("PedidoEnvio");
                });

            modelBuilder.Entity("MlSuite.Domain.PedidoItem", b =>
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

                    b.Property<Guid?>("SeparaçãoUuid")
                        .HasColumnType("uuid");

                    b.Property<string>("Sku")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Título")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Uuid");

                    b.HasIndex("ItemUuid");

                    b.HasIndex("ItemVariaçãoUuid");

                    b.HasIndex("PedidoUuid");

                    b.HasIndex("SeparaçãoUuid");

                    b.ToTable("PedidoItem");
                });

            modelBuilder.Entity("MlSuite.Domain.PedidoPagamento", b =>
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

                    b.HasAlternateKey("Id");

                    b.HasIndex("PedidoUuid");

                    b.ToTable("PedidoPagamento");
                });

            modelBuilder.Entity("MlSuite.Domain.PromolimitEntry", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Ativo")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Estoque")
                        .HasColumnType("integer");

                    b.Property<Guid>("ItemUuid")
                        .HasColumnType("uuid");

                    b.Property<int>("QuantidadeAVenda")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("VariaçãoUuid")
                        .HasColumnType("uuid");

                    b.HasKey("Uuid");

                    b.HasIndex("ItemUuid");

                    b.HasIndex("VariaçãoUuid");

                    b.ToTable("PromolimitEntries");
                });

            modelBuilder.Entity("MlSuite.Domain.Question", b =>
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

                    b.HasAlternateKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("MlSuite.Domain.RefreshToken", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatorIp")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ReasonRevoked")
                        .HasColumnType("text");

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("RevokerIp")
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("UserInfoUuid")
                        .HasColumnType("uuid");

                    b.HasKey("Uuid");

                    b.HasIndex("UserInfoUuid");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("MlSuite.Domain.Separação", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Etiqueta")
                        .HasColumnType("text");

                    b.Property<DateTime>("Fim")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Início")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("PedidoId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("UsuárioUuid")
                        .HasColumnType("uuid");

                    b.HasKey("Uuid");

                    b.HasIndex("PedidoId")
                        .IsUnique();

                    b.HasIndex("UsuárioUuid");

                    b.ToTable("Separações");
                });

            modelBuilder.Entity("MlSuite.Domain.SeparaçãoItem", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Separados")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Uuid");

                    b.ToTable("SeparaçãoItem");
                });

            modelBuilder.Entity("MlSuite.Domain.UserInfo", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("VerificationToken")
                        .HasColumnType("text");

                    b.HasKey("Uuid");

                    b.ToTable("Usuários");
                });

            modelBuilder.Entity("MlSuite.Domain.Item", b =>
                {
                    b.HasOne("MlSuite.Domain.MlUserAuthInfo", "Seller")
                        .WithMany()
                        .HasForeignKey("SellerUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("MlSuite.Domain.ItemVariação", b =>
                {
                    b.HasOne("MlSuite.Domain.Item", null)
                        .WithMany("Variações")
                        .HasForeignKey("ItemUuid");
                });

            modelBuilder.Entity("MlSuite.Domain.Pedido", b =>
                {
                    b.HasOne("MlSuite.Domain.PedidoEnvio", "Envio")
                        .WithMany()
                        .HasForeignKey("EnvioUuid");

                    b.Navigation("Envio");
                });

            modelBuilder.Entity("MlSuite.Domain.PedidoEnvio", b =>
                {
                    b.HasOne("MlSuite.Domain.PedidoDestinatário", "Destinatário")
                        .WithMany()
                        .HasForeignKey("DestinatárioUuid");

                    b.Navigation("Destinatário");
                });

            modelBuilder.Entity("MlSuite.Domain.PedidoItem", b =>
                {
                    b.HasOne("MlSuite.Domain.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemUuid");

                    b.HasOne("MlSuite.Domain.ItemVariação", "ItemVariação")
                        .WithMany()
                        .HasForeignKey("ItemVariaçãoUuid");

                    b.HasOne("MlSuite.Domain.Pedido", null)
                        .WithMany("Itens")
                        .HasForeignKey("PedidoUuid");

                    b.HasOne("MlSuite.Domain.SeparaçãoItem", "Separação")
                        .WithMany()
                        .HasForeignKey("SeparaçãoUuid");

                    b.Navigation("Item");

                    b.Navigation("ItemVariação");

                    b.Navigation("Separação");
                });

            modelBuilder.Entity("MlSuite.Domain.PedidoPagamento", b =>
                {
                    b.HasOne("MlSuite.Domain.Pedido", null)
                        .WithMany("Pagamentos")
                        .HasForeignKey("PedidoUuid");
                });

            modelBuilder.Entity("MlSuite.Domain.PromolimitEntry", b =>
                {
                    b.HasOne("MlSuite.Domain.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MlSuite.Domain.ItemVariação", "Variação")
                        .WithMany()
                        .HasForeignKey("VariaçãoUuid");

                    b.Navigation("Item");

                    b.Navigation("Variação");
                });

            modelBuilder.Entity("MlSuite.Domain.RefreshToken", b =>
                {
                    b.HasOne("MlSuite.Domain.UserInfo", "UserInfo")
                        .WithMany()
                        .HasForeignKey("UserInfoUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("MlSuite.Domain.Separação", b =>
                {
                    b.HasOne("MlSuite.Domain.Pedido", "Pedido")
                        .WithOne("Separação")
                        .HasForeignKey("MlSuite.Domain.Separação", "PedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MlSuite.Domain.UserInfo", "Usuário")
                        .WithMany()
                        .HasForeignKey("UsuárioUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");

                    b.Navigation("Usuário");
                });

            modelBuilder.Entity("MlSuite.Domain.Item", b =>
                {
                    b.Navigation("Variações");
                });

            modelBuilder.Entity("MlSuite.Domain.Pedido", b =>
                {
                    b.Navigation("Itens");

                    b.Navigation("Pagamentos");

                    b.Navigation("Separação");
                });
#pragma warning restore 612, 618
        }
    }
}
