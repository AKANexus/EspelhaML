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
    [Migration("20240217214453_ImpressaoTsCol")]
    partial class ImpressaoTsCol
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MlSuite.Domain.Embalagem", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Etiqueta")
                        .HasColumnType("text");

                    b.Property<decimal>("ReferenciaId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<Guid?>("SeparaçãoUuid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ShippingUuid")
                        .HasColumnType("uuid");

                    b.Property<int>("StatusEmbalagem")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("TimestampImpressao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("TipoVendaMl")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Uuid");

                    b.HasIndex("SeparaçãoUuid");

                    b.HasIndex("ShippingUuid")
                        .IsUnique();

                    b.HasIndex("ReferenciaId", "TipoVendaMl")
                        .IsUnique();

                    b.ToTable("Embalagem");
                });

            modelBuilder.Entity("MlSuite.Domain.EmbalagemItem", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Descrição")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("EmbalagemUuid")
                        .HasColumnType("uuid");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "url_imagem");

                    b.Property<int>("QuantidadeAEscanear")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "quantidade");

                    b.Property<int>("QuantidadeEscaneada")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "separados");

                    b.Property<string>("SKU")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Uuid");

                    b.HasIndex("EmbalagemUuid");

                    b.ToTable("EmbalagemItem");
                });

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

            modelBuilder.Entity("MlSuite.Domain.Order", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal?>("Frete")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<Guid?>("PackUuid")
                        .HasColumnType("uuid");

                    b.Property<decimal>("SellerId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<Guid?>("ShippingUuid")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Uuid");

                    b.HasAlternateKey("Id");

                    b.HasIndex("PackUuid");

                    b.HasIndex("ShippingUuid");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("MlSuite.Domain.OrderItem", b =>
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

                    b.Property<Guid?>("OrderUuid")
                        .HasColumnType("uuid");

                    b.Property<decimal>("PreçoUnitário")
                        .HasColumnType("numeric");

                    b.Property<int>("QuantidadeVendida")
                        .HasColumnType("integer");

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

                    b.HasIndex("OrderUuid");

                    b.ToTable("OrderItem");
                });

            modelBuilder.Entity("MlSuite.Domain.Pack", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("Id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<Guid>("ShippingUuid")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Uuid");

                    b.HasAlternateKey("Id");

                    b.HasIndex("ShippingUuid");

                    b.ToTable("Packs");
                });

            modelBuilder.Entity("MlSuite.Domain.Payment", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("Id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<Guid?>("OrderUuid")
                        .HasColumnType("uuid");

                    b.Property<int>("Parcelas")
                        .HasColumnType("integer");

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

                    b.HasIndex("OrderUuid");

                    b.ToTable("Payment");
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

                    b.Property<DateTime?>("Fim")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("Identificador")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Identificador"));

                    b.Property<DateTime?>("Início")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("SellerId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("UsuárioUuid")
                        .HasColumnType("uuid");

                    b.HasKey("Uuid");

                    b.HasIndex("UsuárioUuid");

                    b.ToTable("Separações");
                });

            modelBuilder.Entity("MlSuite.Domain.Shipping", b =>
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

                    b.Property<string>("Etiqueta")
                        .HasColumnType("text");

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

                    b.ToTable("Shipping");
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

            modelBuilder.Entity("MlSuite.Domain.WebHookInfo", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CallbackUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("WebHookTopic")
                        .HasColumnType("integer");

                    b.HasKey("Uuid");

                    b.ToTable("WebhookSubscribers");
                });

            modelBuilder.Entity("MlSuite.Domain.Embalagem", b =>
                {
                    b.HasOne("MlSuite.Domain.Separação", null)
                        .WithMany("Embalagens")
                        .HasForeignKey("SeparaçãoUuid");

                    b.HasOne("MlSuite.Domain.Shipping", "Shipping")
                        .WithOne("Embalagem")
                        .HasForeignKey("MlSuite.Domain.Embalagem", "ShippingUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shipping");
                });

            modelBuilder.Entity("MlSuite.Domain.EmbalagemItem", b =>
                {
                    b.HasOne("MlSuite.Domain.Embalagem", null)
                        .WithMany("EmbalagemItems")
                        .HasForeignKey("EmbalagemUuid");
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

            modelBuilder.Entity("MlSuite.Domain.Order", b =>
                {
                    b.HasOne("MlSuite.Domain.Pack", "Pack")
                        .WithMany("Pedidos")
                        .HasForeignKey("PackUuid");

                    b.HasOne("MlSuite.Domain.Shipping", "Shipping")
                        .WithMany()
                        .HasForeignKey("ShippingUuid");

                    b.Navigation("Pack");

                    b.Navigation("Shipping");
                });

            modelBuilder.Entity("MlSuite.Domain.OrderItem", b =>
                {
                    b.HasOne("MlSuite.Domain.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemUuid");

                    b.HasOne("MlSuite.Domain.ItemVariação", "ItemVariação")
                        .WithMany()
                        .HasForeignKey("ItemVariaçãoUuid");

                    b.HasOne("MlSuite.Domain.Order", null)
                        .WithMany("Itens")
                        .HasForeignKey("OrderUuid");

                    b.Navigation("Item");

                    b.Navigation("ItemVariação");
                });

            modelBuilder.Entity("MlSuite.Domain.Pack", b =>
                {
                    b.HasOne("MlSuite.Domain.Shipping", "Shipping")
                        .WithMany()
                        .HasForeignKey("ShippingUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shipping");
                });

            modelBuilder.Entity("MlSuite.Domain.Payment", b =>
                {
                    b.HasOne("MlSuite.Domain.Order", null)
                        .WithMany("Pagamentos")
                        .HasForeignKey("OrderUuid");
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
                    b.HasOne("MlSuite.Domain.UserInfo", "Usuário")
                        .WithMany()
                        .HasForeignKey("UsuárioUuid");

                    b.Navigation("Usuário");
                });

            modelBuilder.Entity("MlSuite.Domain.Shipping", b =>
                {
                    b.HasOne("MlSuite.Domain.PedidoDestinatário", "Destinatário")
                        .WithMany()
                        .HasForeignKey("DestinatárioUuid");

                    b.Navigation("Destinatário");
                });

            modelBuilder.Entity("MlSuite.Domain.Embalagem", b =>
                {
                    b.Navigation("EmbalagemItems");
                });

            modelBuilder.Entity("MlSuite.Domain.Item", b =>
                {
                    b.Navigation("Variações");
                });

            modelBuilder.Entity("MlSuite.Domain.Order", b =>
                {
                    b.Navigation("Itens");

                    b.Navigation("Pagamentos");
                });

            modelBuilder.Entity("MlSuite.Domain.Pack", b =>
                {
                    b.Navigation("Pedidos");
                });

            modelBuilder.Entity("MlSuite.Domain.Separação", b =>
                {
                    b.Navigation("Embalagens");
                });

            modelBuilder.Entity("MlSuite.Domain.Shipping", b =>
                {
                    b.Navigation("Embalagem");
                });
#pragma warning restore 612, 618
        }
    }
}
