﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using esii.Entities;

namespace esii.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ativofinanceiro> Ativofinanceiros { get; set; }

    public virtual DbSet<Depositoprazo> Depositoprazos { get; set; }

    public virtual DbSet<Fundoinvestimento> Fundoinvestimentos { get; set; }

    public virtual DbSet<Imovelarrendado> Imovelarrendados { get; set; }

    public virtual DbSet<Relatorio> Relatorios { get; set; }

    public virtual DbSet<Tiporelatorio> Tiporelatorios { get; set; }

    public virtual DbSet<Tipoutilizador> Tipoutilizadors { get; set; }

    public virtual DbSet<Utilizador> Utilizadors { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
      //  => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ativofinanceiro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ativofinanceiro_pkey");

            entity.ToTable("ativofinanceiro");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataIni).HasColumnName("data_ini");
            entity.Property(e => e.Duracao).HasColumnName("duracao");
            entity.Property(e => e.Imposto)
                .HasPrecision(10, 2)
                .HasColumnName("imposto");
            entity.Property(e => e.UtilizadorId).HasColumnName("utilizador_id");

            entity.HasOne(d => d.Utilizador).WithMany(p => p.Ativofinanceiros)
                .HasForeignKey(d => d.UtilizadorId)
                .HasConstraintName("ativofinanceiro_utilizador_id_fkey");
        });

        modelBuilder.Entity<Depositoprazo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("depositoprazo_pkey");

            entity.ToTable("depositoprazo");

            entity.HasIndex(e => e.NumConta, "depositoprazo_num_conta_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AtivoId).HasColumnName("ativo_id");
            entity.Property(e => e.Banco)
                .HasMaxLength(100)
                .HasColumnName("banco");
            entity.Property(e => e.NumConta)
                .HasMaxLength(50)
                .HasColumnName("num_conta");
            entity.Property(e => e.TaxaJurosAnual)
                .HasPrecision(5, 2)
                .HasColumnName("taxa_juros_anual");
            entity.Property(e => e.Titulares)
                .HasColumnType("character varying")
                .HasColumnName("titulares");
            entity.Property(e => e.Valor)
                .HasPrecision(15, 2)
                .HasColumnName("valor");

            entity.HasOne(d => d.Ativo).WithMany(p => p.Depositoprazos)
                .HasForeignKey(d => d.AtivoId)
                .HasConstraintName("depositoprazo_ativo_id_fkey");
        });

        modelBuilder.Entity<Fundoinvestimento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fundoinvestimento_pkey");

            entity.ToTable("fundoinvestimento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AtivoId).HasColumnName("ativo_id");
            entity.Property(e => e.MontanteInvestido)
                .HasPrecision(15, 2)
                .HasColumnName("montante_investido");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.TaxaJuros)
                .HasPrecision(5, 2)
                .HasColumnName("taxa_juros");

            entity.HasOne(d => d.Ativo).WithMany(p => p.Fundoinvestimentos)
                .HasForeignKey(d => d.AtivoId)
                .HasConstraintName("fundoinvestimento_ativo_id_fkey");
        });

        modelBuilder.Entity<Imovelarrendado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("imovelarrendado_pkey");

            entity.ToTable("imovelarrendado");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AtivoId).HasColumnName("ativo_id");
            entity.Property(e => e.Designacao)
                .HasMaxLength(100)
                .HasColumnName("designacao");
            entity.Property(e => e.Localizacao)
                .HasMaxLength(255)
                .HasColumnName("localizacao");
            entity.Property(e => e.ValorAnualDespesas)
                .HasPrecision(10, 2)
                .HasColumnName("valor_anual_despesas");
            entity.Property(e => e.ValorImovel)
                .HasPrecision(15, 2)
                .HasColumnName("valor_imovel");
            entity.Property(e => e.ValorMensalCondominio)
                .HasPrecision(10, 2)
                .HasColumnName("valor_mensal_condominio");
            entity.Property(e => e.ValorRenda)
                .HasPrecision(15, 2)
                .HasColumnName("valor_renda");

            entity.HasOne(d => d.Ativo).WithMany(p => p.Imovelarrendados)
                .HasForeignKey(d => d.AtivoId)
                .HasConstraintName("imovelarrendado_ativo_id_fkey");
        });

        modelBuilder.Entity<Relatorio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("relatorio_pkey");

            entity.ToTable("relatorio");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataFim).HasColumnName("data_fim");
            entity.Property(e => e.DataIni).HasColumnName("data_ini");
            entity.Property(e => e.TipoId).HasColumnName("tipo_id");
            entity.Property(e => e.UtilizadorId).HasColumnName("utilizador_id");

            entity.HasOne(d => d.Tipo).WithMany(p => p.Relatorios)
                .HasForeignKey(d => d.TipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("relatorio_tipo_id_fkey");

            entity.HasOne(d => d.Utilizador).WithMany(p => p.Relatorios)
                .HasForeignKey(d => d.UtilizadorId)
                .HasConstraintName("relatorio_utilizador_id_fkey");
        });

        modelBuilder.Entity<Tiporelatorio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tiporelatorio_pkey");

            entity.ToTable("tiporelatorio");

            entity.HasIndex(e => e.Descricao, "tiporelatorio_descricao_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<Tipoutilizador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tipoutilizador_pkey");

            entity.ToTable("tipoutilizador");

            entity.HasIndex(e => e.Descricao, "tipoutilizador_descricao_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<Utilizador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("utilizador_pkey");

            entity.ToTable("utilizador");

            entity.HasIndex(e => e.Email, "utilizador_email_key").IsUnique();

            entity.HasIndex(e => e.Nif, "utilizador_nif_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Imposto)
                .HasPrecision(10, 2)
                .HasColumnName("imposto");
            entity.Property(e => e.Nif)
                .HasMaxLength(20)
                .HasColumnName("nif");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.TipoId).HasColumnName("tipo_id");

            entity.HasOne(d => d.Tipo).WithMany(p => p.Utilizadors)
                .HasForeignKey(d => d.TipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("utilizador_tipo_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
