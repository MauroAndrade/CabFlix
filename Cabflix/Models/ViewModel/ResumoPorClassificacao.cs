using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cabflix.Models.ViewModel
{
    public class ResumoPorClassificacao
    {
        public string Classificacao { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public decimal Porcentagem { get; set; } 

    }
}