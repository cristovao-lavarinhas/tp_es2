using System;
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
    
    public virtual DbSet<HistoricoAcao> HistoricoAcoes { get; set; }
    
    public virtual DbSet<TipoAcao> TipoAcoes { get; set; }
    
    public virtual DbSet<TaxaJuroMensal> TaxaJuroMensal { get; set; }
    
    public virtual DbSet<JurosCompostos> JurosCompostos { get; set; }




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
            entity.HasKey(e => e.AtivoId)
                .HasName("depositoprazo_pkey");

            entity.ToTable("depositoprazo");

            entity.HasIndex(e => e.NumConta, "depositoprazo_num_conta_key")
                .IsUnique();

            entity.Property(e => e.AtivoId)
                .HasColumnName("id");

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

            entity.HasOne(d => d.Ativo)
                .WithOne(a => a.Depositoprazo)
                .HasForeignKey<Depositoprazo>(d => d.AtivoId)
                .HasConstraintName("depositoprazo_id_fkey")
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Fundoinvestimento>(entity =>
        {
            entity.HasKey(e => e.AtivoId)
                .HasName("fundoinvestimento_pkey");

            entity.ToTable("fundoinvestimento");

            entity.Property(e => e.AtivoId)
                .HasColumnName("id");

            entity.Property(e => e.MontanteInvestido)
                .HasPrecision(15, 2)
                .HasColumnName("montante_investido");

            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");

            entity.Property(e => e.TaxaJuros)
                .HasPrecision(5, 2)
                .HasColumnName("taxa_juros");

            entity.HasOne(d => d.Ativo)
                .WithOne(a => a.Fundoinvestimento)
                .HasForeignKey<Fundoinvestimento>(d => d.AtivoId)
                .HasConstraintName("fundoinvestimento_ativo_id_fkey")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Imovelarrendado>(entity =>
        {
            entity.HasKey(e => e.AtivoId)
                .HasName("imovelarrendado_pkey");

            entity.ToTable("imovelarrendado");

            entity.Property(e => e.AtivoId)
                .HasColumnName("id");

            entity.Property(e => e.Designacao)
                .HasMaxLength(100)
                .HasColumnName("designacao");

            entity.Property(e => e.Localizacao)
                .HasMaxLength(255)
                .HasColumnName("localizacao");

            entity.Property(e => e.ValorImovel)
                .HasPrecision(15, 2)
                .HasColumnName("valor_imovel");

            entity.Property(e => e.ValorRenda)
                .HasPrecision(15, 2)
                .HasColumnName("valor_renda");

            entity.Property(e => e.ValorMensalCondominio)
                .HasPrecision(10, 2)
                .HasColumnName("valor_mensal_condominio");

            entity.Property(e => e.ValorAnualDespesas)
                .HasPrecision(10, 2)
                .HasColumnName("valor_anual_despesas");

            entity.HasOne(d => d.Ativo)
                .WithOne(a => a.Imovelarrendado)
                .HasForeignKey<Imovelarrendado>(d => d.AtivoId)
                .HasConstraintName("imovelarrendado_ativo_id_fkey")
                .OnDelete(DeleteBehavior.Cascade);
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

        modelBuilder.Entity<TipoAcao>(entity =>
        {
            entity.ToTable("tipoacao");
            
            entity.HasKey(e => e.Id).HasName("tipo_acao_pkey");

            entity.HasIndex(e => e.Nome)
                .IsUnique()
                .HasDatabaseName("tipo_acao_nome_key");

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<HistoricoAcao>(entity =>
        {
            entity.ToTable("historicoacao");

            entity.HasKey(e => e.Id).HasName("historicoacao_pkey");

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.DataAcao)
                .IsRequired()
                .HasColumnName("dataacao");

            entity.Property(e => e.AtivoId)
                .HasColumnName("ativofinanceiro_id");

            entity.Property(e => e.Ativo)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("ativo");

            entity.Property(e => e.TipoAcaoId)
                .HasColumnName("tipoacao_id");

            entity.HasOne(e => e.AtivoFinanceiro)
                .WithMany(a => a.HistoricoAcoes)
                .HasForeignKey(e => e.AtivoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("historicoacao_ativo_fkey");

            entity.HasOne(e => e.TipoAcao)
                .WithMany(t => t.Historicos)
                .HasForeignKey(e => e.TipoAcaoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("historicoacao_tipo_fkey");
        });
        
        modelBuilder.Entity<TaxaJuroMensal>(entity =>
        {
            entity.ToTable("taxa_juromensal"); // <- este é o nome da tabela real na base de dados

            entity.HasKey(e => e.Id).HasName("taxa_juromensal_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Mes).HasColumnName("mes");
            entity.Property(e => e.Ano).HasColumnName("ano");
            entity.Property(e => e.Taxa)
                .HasPrecision(5, 2)
                .HasColumnName("taxa");
            entity.Property(e => e.Descricao)
                .HasMaxLength(255)
                .HasColumnName("descricao");
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
