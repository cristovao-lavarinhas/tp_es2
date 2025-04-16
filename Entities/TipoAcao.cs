using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace esii.Entities
{
    public class TipoAcao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = null!;

        public virtual ICollection<HistoricoAcao> Historicos { get; set; } = new List<HistoricoAcao>();
    }
}