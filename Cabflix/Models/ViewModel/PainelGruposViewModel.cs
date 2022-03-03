using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cabflix.Models.ViewModel
{
    public class PainelGruposViewModel
    {
        public int IdGrupo { get; set; }
        public string Grupo { get; set; } 
        public string Servico { get; set; }
        public int Quantidade { get; set; }
        public Decimal Valor { get; set; }
        public Decimal Media { get; set; }
        public Decimal Porcentagem { get; set; }

    }
}