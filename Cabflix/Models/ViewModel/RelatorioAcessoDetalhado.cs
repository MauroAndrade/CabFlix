using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cabflix.Models.ViewModel
{
    public class RelatorioAcessoDetalhado
    {
        public RelatorioAcessoDetalhado()
        {
            ResumoPorGrupoServico = new List<ResumoPorGrupoServico>();
            RelatorioUsuarioDetalhado = new List<RelatorioUsuarioDetalhado>();
            ResumoPorClassificacao = new List<ResumoPorClassificacao>();
        }

        public int idContato { get; set; }
        public string Periodo { get; set; }
        public decimal Total{ get; set; }
        public List<ResumoPorGrupoServico> ResumoPorGrupoServico { get; set; }
        public List<RelatorioUsuarioDetalhado> RelatorioUsuarioDetalhado { get; set; }
        public List<ResumoPorClassificacao> ResumoPorClassificacao { get; set; } 

    }
}
