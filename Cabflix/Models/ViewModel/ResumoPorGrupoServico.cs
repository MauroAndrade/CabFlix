using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cabflix.Models.ViewModel
{
    public class ResumoPorGrupoServico
    {
        public int ID_GRUPO_SERVICO { get; set; }
        public string GRUPO_SERVICO { get; set; }
        public Decimal? TOTAL_MINUTOS { get; set; }
        public Decimal? TOTAL_DADOS { get; set; }
        public Decimal TOTAL_VALOR { get; set; }
        public Decimal PORCENTAGEM { get; set; }
    }
}