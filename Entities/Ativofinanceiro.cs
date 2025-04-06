using System;
using System.Collections.Generic;

namespace esii.Entities
{
    public partial class Ativofinanceiro
    {
        public int Id { get; set; }

        public int UtilizadorId { get; set; }

        public DateOnly DataIni { get; set; }

        public int Duracao { get; set; }

        public decimal Imposto { get; set; }
        
        public virtual Imovelarrendado? Imovelarrendado { get; set; }
        
        public virtual Depositoprazo? Depositoprazo { get; set; }
        
        public virtual Fundoinvestimento? Fundoinvestimento { get; set; }
        public virtual Utilizador Utilizador { get; set; } = null!;
    }
}

