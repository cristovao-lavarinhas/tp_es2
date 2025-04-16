using System;

namespace esii.Models
{
    public class HistoricoAcaoViewModel
    {
        public string TipoAcao { get; set; } = null!;
        public DateTime DataHora { get; set; }
        public int AtivoId { get; set; }
        public string Ativo { get; set; } = null!;
    }
}
