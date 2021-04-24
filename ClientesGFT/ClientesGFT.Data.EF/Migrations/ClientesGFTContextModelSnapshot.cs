﻿// <auto-generated />
using System;
using ClientesGFT.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ClientesGFT.Data.EF.Migrations
{
    [DbContext(typeof(ClientesGFTContext))]
    partial class ClientesGFTContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ClientesGFT.Domain.Entities.Adress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Cep")
                        .HasColumnType("varchar(8)")
                        .HasMaxLength(8)
                        .IsUnicode(false);

                    b.Property<int?>("CityId")
                        .HasColumnName("IdCidade")
                        .HasColumnType("int");

                    b.Property<string>("Complement")
                        .HasColumnName("Complemento")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnName("Bairro")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<int>("Number")
                        .HasColumnName("Numero")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnName("Rua")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Enderecos");
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.AdressEntities.City", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("Cidade")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int>("StateId")
                        .HasColumnName("IdEstado")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Description")
                        .IsUnique()
                        .HasName("UQ__Cidades__15F7A8DA6F9206A9");

                    b.HasIndex("StateId");

                    b.ToTable("Cidades");
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.AdressEntities.Country", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("Pais")
                        .HasColumnType("varchar(60)")
                        .HasMaxLength(60)
                        .IsUnicode(false);

                    b.Property<string>("Initials")
                        .IsRequired()
                        .HasColumnName("Sigla")
                        .HasColumnType("varchar(2)")
                        .HasMaxLength(2)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("Description")
                        .IsUnique()
                        .HasName("UQ__Paises__A15FFF7DE1A4457E");

                    b.HasIndex("Initials")
                        .IsUnique()
                        .HasName("UQ__Paises__3199C5ED121356C0");

                    b.ToTable("Paises");
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.AdressEntities.State", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("ContryId")
                        .HasColumnName("IdPais")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("Estado")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("ContryId");

                    b.HasIndex("Description")
                        .IsUnique()
                        .HasName("UQ__Estados__36DF552F9175DA7F");

                    b.ToTable("Estados");
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AdressId")
                        .HasColumnName("IdEndereco")
                        .HasColumnType("int");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnName("DataNasc")
                        .HasColumnType("datetime");

                    b.Property<string>("CPF")
                        .HasColumnName("CPF")
                        .HasColumnType("varchar(11)")
                        .HasMaxLength(11)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("DataCadastro")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<DateTime>("ModifiedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("DataAlteracao")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Nome")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("RG")
                        .HasColumnName("RG")
                        .HasColumnType("varchar(9)")
                        .HasMaxLength(9)
                        .IsUnicode(false);

                    b.Property<int?>("StatusFluxoId")
                        .HasColumnName("IdStatusAtual")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdressId");

                    b.HasIndex("CPF")
                        .IsUnique()
                        .HasName("UQ__Clientes__C1F89731CDBAAC7A")
                        .HasFilter("[CPF] IS NOT NULL");

                    b.HasIndex("StatusFluxoId");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.Fluxo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId")
                        .HasColumnName("IdCliente")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("DataCriacao")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("StatusId")
                        .HasColumnName("IdStatus")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnName("IdUsuario")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("StatusId");

                    b.HasIndex("UserId");

                    b.ToTable("Fluxo_Aprovacao");
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.Phone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClientId")
                        .HasColumnName("IdCliente")
                        .HasColumnType("int");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnName("Telefone")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Clientes_Telefones");
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Ativo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((1))");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("Nome")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Perfils");
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.Status", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("Descricao")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Status_Fluxo");
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Ativo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((1))");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("DataCadastro")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Nome")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("Senha")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique()
                        .HasName("UQ__Usuarios__5E55825BE0CCFE30");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnName("IdUsuario")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnName("IdPerfil")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId")
                        .HasName("PK__USUARIO_PERFIL");

                    b.HasIndex("RoleId");

                    b.ToTable("Usuario_Perfil");
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.Adress", b =>
                {
                    b.HasOne("ClientesGFT.Domain.Entities.AdressEntities.City", "City")
                        .WithMany("Adresses")
                        .HasForeignKey("CityId")
                        .HasConstraintName("FK__ENDERECOS__CIDADE");
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.AdressEntities.City", b =>
                {
                    b.HasOne("ClientesGFT.Domain.Entities.AdressEntities.State", "State")
                        .WithMany("Cities")
                        .HasForeignKey("StateId")
                        .HasConstraintName("FK__CIDADES__ESTADO")
                        .IsRequired();
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.AdressEntities.State", b =>
                {
                    b.HasOne("ClientesGFT.Domain.Entities.AdressEntities.Country", "Country")
                        .WithMany("States")
                        .HasForeignKey("ContryId")
                        .HasConstraintName("FK__ESTADOS__PAIS")
                        .IsRequired();
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.Client", b =>
                {
                    b.HasOne("ClientesGFT.Domain.Entities.Adress", "Adress")
                        .WithMany("Clients")
                        .HasForeignKey("AdressId")
                        .HasConstraintName("FK__CLIENTES__ENDERECO");

                    b.HasOne("ClientesGFT.Domain.Entities.Status", "CurrentStatus")
                        .WithMany("Clients")
                        .HasForeignKey("StatusFluxoId")
                        .HasConstraintName("FK__CLIENTES__STATUS_FLUXO");
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.Fluxo", b =>
                {
                    b.HasOne("ClientesGFT.Domain.Entities.Client", "Client")
                        .WithMany("Fluxos")
                        .HasForeignKey("ClientId")
                        .HasConstraintName("FK__FLUXO_APROVACAO__CLIENTE")
                        .IsRequired();

                    b.HasOne("ClientesGFT.Domain.Entities.Status", "Status")
                        .WithMany("Fluxos")
                        .HasForeignKey("StatusId")
                        .HasConstraintName("FK__FLUXO_APROVACAO__STATUS_ANTERIOR__STATUS_FLUXO")
                        .IsRequired();

                    b.HasOne("ClientesGFT.Domain.Entities.User", "User")
                        .WithMany("Fluxos")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__FLUXO_APROVACAO__USUARIO")
                        .IsRequired();
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.Phone", b =>
                {
                    b.HasOne("ClientesGFT.Domain.Entities.Client", "Client")
                        .WithMany("Phones")
                        .HasForeignKey("ClientId")
                        .HasConstraintName("FK__CLIENTES_TELEFONES__CLIENTE");
                });

            modelBuilder.Entity("ClientesGFT.Domain.Entities.UserRole", b =>
                {
                    b.HasOne("ClientesGFT.Domain.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK__USUARIO_PERFIL__PERFIL")
                        .IsRequired();

                    b.HasOne("ClientesGFT.Domain.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__USUARIO_PERFIL__USUARIO")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
