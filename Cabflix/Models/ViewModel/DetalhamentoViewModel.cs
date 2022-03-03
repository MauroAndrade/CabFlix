using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cabflix.Models.ViewModel
{
    public class DetalhamentoViewModel
    {
        string Operadora { get; set; }
	    string Tipo { get; set; }
	    string Conta { get; set; }
	    string Origem { get; set; }
	    string Destino { get; set; }
	    DateTime MesReferencia { get; set; }
	    string Servico { get; set; }
	    string UnidadeMedida { get; set; } 
        decimal Quantidade { get; set; }
	    DateTime? Horario { get; set; }
	    string UF { get; set; }
	    string Pais { get; set; }
	    decimal Valor { get; set; }
    }
}