using esii.Composite.Models;

using esii.Composite.Models;
using Microsoft.EntityFrameworkCore;

namespace esii.Composite.Data
{
    public class AtivosDbContext : DbContext
    {
        public AtivosDbContext(DbContextOptions<AtivosDbContext> options)
            : base(options) { }

        public DbSet<AtivoFinanceiro> AtivosFinanceiros { get; set; }
        public DbSet<DepositoPrazo> Depositos { get; set; }
        public DbSet<FundoInvestimento> Fundos { get; set; }
        public DbSet<ImovelArrendado> Imoveis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tabela base
            modelBuilder.Entity<AtivoFinanceiro>()
                .ToTable("ativofinanceiro")
                .HasKey(a => a.Id);

            // Depósito a Prazo
            modelBuilder.Entity<DepositoPrazo>()
                .ToTable("depositoprazo")
                .HasKey(d => d.Id);

            modelBuilder.Entity<DepositoPrazo>()
                .HasOne(d => d.AtivoFinanceiro)
                .WithOne(a => a.DepositoPrazo)
                .HasForeignKey<DepositoPrazo>(d => d.Id);

            // Fundo de Investimento
            modelBuilder.Entity<FundoInvestimento>()
                .ToTable("fundoinvestimento")
                .HasKey(f => f.Id);

            modelBuilder.Entity<FundoInvestimento>()
                .HasOne(f => f.AtivoFinanceiro)
                .WithOne(a => a.FundoInvestimento)
                .HasForeignKey<FundoInvestimento>(f => f.Id);

            // Imóvel Arrendado
            modelBuilder.Entity<ImovelArrendado>()
                .ToTable("imovelarrendado")
                .HasKey(i => i.Id);

            modelBuilder.Entity<ImovelArrendado>()
                .HasOne(i => i.AtivoFinanceiro)
                .WithOne(a => a.ImovelArrendado)
                .HasForeignKey<ImovelArrendado>(i => i.Id);
        }
    }
}
