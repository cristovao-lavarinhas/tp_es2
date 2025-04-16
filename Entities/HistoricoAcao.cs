using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esii.Entities
{
    public class HistoricoAcao
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AtivoFinanceiro")]
        public int AtivoId { get; set; }
        public string Ativo { get; set; }  
        public virtual Ativofinanceiro AtivoFinanceiro { get; set; } = null!;

        [ForeignKey("TipoAcao")]
        public int TipoAcaoId { get; set; }
        public virtual TipoAcao TipoAcao { get; set; } = null!;
        
        public DateTime DataAcao { get; set; }
    }
}
