using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cabflix.Models.ViewModel
{
    public class RelatorioUsuarioDetalhado
    {
        public DateTime DATA { get; set; }
        public string HORARIO { get; set; }
        public string DESTINO { get; set; }
        public string UNIDADEMEDIDA { get; set; }
        public int ID_CONTATO { get; set; } 
        public int ID_SERVICO { get; set; }
        public string SERVICO { get; set; }
        public int ID_GRUPO_SERVICO { get; set; }
        public string GRUPO_SERVICO { get; set; }
        public Decimal QUANTIDADE { get; set; }
        public Decimal VALOR { get; set; }
    }
}