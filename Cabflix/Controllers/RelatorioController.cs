using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cabflix.Models.Database;
using Cabflix.Models.ViewModel;
using Newtonsoft.Json;
using Rotativa;
using Rotativa.Options;
using Cabflix.Utils;

namespace Cabflix.Controllers
{
    public class RelatorioController : MasterController
    {

        Context data = new Context();

        // Relatorio or report bellow
        [HttpPost]
        public ActionResult RelatorioUsuarioDetalhado(string pData)
        {

            //Email sendEmail = new Email(); 
            //var retorno = sendEmail.EnviarEmail();


            //Model
            RelatorioAcessoDetalhado relatorio = new RelatorioAcessoDetalhado();

            RelatorioUsuarioDetalhadoFiltro filtro = JsonConvert.DeserializeObject<RelatorioUsuarioDetalhadoFiltro>(pData);

            var mes = filtro.mes.Split('/');
            var mesReferencia = Convert.ToDateTime(mes[1] + "-" + mes[0] + "-01");

            relatorio.idContato = filtro.idContato;
            relatorio.Periodo = filtro.mes;

            var usuarioLogado = GetUsuarioLogado();

            var responsavel = data.Contatoes.Where(x => x.Id == 1).FirstOrDefault();

            var contato = data.Contatoes.FirstOrDefault(x => x.Id == 1);

            string sqlResumoPorGrupoServico = @"EXECUTE Proc_ResumoPorGrupoServico '" + mesReferencia.ToString("yyyy-MM-dd") + " 00:00:00'," + filtro.idContato + "; ";
            relatorio.ResumoPorGrupoServico = data.Database.SqlQuery<ResumoPorGrupoServico>(sqlResumoPorGrupoServico).ToList();

            string sqlResumoPorClassificacao = @"EXECUTE Proc_ResumoPorClassificacao '" + mesReferencia.ToString("yyyy-MM-dd") + " 00:00:00'," + filtro.idContato + "; ";
            var ResumoPorClassificacao = data.Database.SqlQuery<ResumoPorClassificacao>(sqlResumoPorClassificacao).ToList();

            var totalClassificacao = ResumoPorClassificacao.Count();

            var auxResumoPorClassificacao = ResumoPorClassificacao
                .GroupBy(x => x.Classificacao)
                .Select(c => new ResumoPorClassificacao
                {
                    Classificacao = c.Key,
                    Valor = c.Sum(x => x.Valor),
                    Quantidade = c.Count(x => x.Classificacao != null),
                    Porcentagem = ((c.Count(x => x.Classificacao != null) * 100) / totalClassificacao) //AJUSTAR DECIMAIS - NÃO ESTÁ APARECENDO
                }).ToList();
            relatorio.ResumoPorClassificacao = auxResumoPorClassificacao;

            relatorio.Total = relatorio.ResumoPorGrupoServico.Sum(x => x.TOTAL_VALOR);

            string sqlRelatorio = @"SELECT top (1000)
	                    CP.DATA,
						CP.HORARIO,
	                    CP.NUMERO_DESTINO AS DESTINO,
	                    UM.SIMBOLO AS UNIDADEMEDIDA,
	                    ISNULL(CP.MINUTOS_QTD, CP.DADOS_QTD) QUANTIDADE,
	                    CP.VALOR AS VALOR,
						S.ID AS ID_SERVICO,
						S.NOME AS SERVICO,
						GS.ID AS ID_GRUPO_SERVICO,
						GS.NOME AS GRUPO_SERVICO,
						CO.ID ID_CONTATO
                    FROM CONTA_PADRONIZADA CP
                      INNER JOIN NUMERO_LINHA NL ON NL.ID = CP.FK_NUMERO_LINHA
                      INNER JOIN CONTA C ON C.ID = NL.FK_CONTA
                      INNER JOIN OPERADORA OP ON OP.ID = C.FK_OPERADORA
                      INNER JOIN TIPO_CONTA TC ON TC.ID = C.FK_TIPO_CONTA
                      INNER JOIN SERVICO S ON S.ID = CP.FK_SERVICO
					  INNER JOIN GRUPO_DE_SERVICO GS ON GS.ID = S.FK_GRUPO_DE_SERVICO
                      INNER JOIN UNIDADE_DE_MEDIDA UM ON UM.ID = S.FK_UNIDADE_DE_MEDIDA
					  INNER JOIN CONTATO CO ON CO.ID = NL.FK_CONTATO
                      LEFT JOIN UF ON UF.ID = CP.FK_UF
                      LEFT JOIN PAIS ON PAIS.ID = UF.ID
		              WHERE CO.ID = 1
                      ORDER BY 1 DESC";

            relatorio.RelatorioUsuarioDetalhado = data.Database.SqlQuery<RelatorioUsuarioDetalhado>(sqlRelatorio).ToList();

            string titulo = "Detalhamento do Acesso";

            string empresa = usuarioLogado.Empresa.Nome;
            string endereco = usuarioLogado.Empresa.Logradouro;
            string email = usuarioLogado.Empresa.Email;
            string numero = usuarioLogado.Empresa.Numero;
            string periodo = DateTime.Now.ToShortDateString();

            var cabecalho = Request.Url.Authority + $"/Templates/Cabecalho.html?titulo=" + titulo + "&empresa=" + empresa + "&periodo=" + periodo;
            var rodape = Request.Url.Authority + "/Templates/Rodape.html";
            string customSwitches = string.Format("--header-html  \"{0}\" " +
                                              "--viewport-size 1280x1024 " +
                                              "--footer-spacing \"6\" " +
                                              "--header-spacing \"6\" " +
                                              "--footer-html \"{1}\" ", cabecalho, rodape);

            // Para download do arquivo com nome específico
            Response.AppendHeader("Content-Disposition", "inline; filename=Relatorio_" + DateTime.Now.ToShortTimeString() + ".pdf");

            return new ViewAsPdf()
            {
                ViewName = "RelatorioDetalhado",
                CustomSwitches = customSwitches,
                Model = relatorio,
                PageSize = Rotativa.Options.Size.A4,
                //PageMargins = new Margins(5, 5, 5, 5),
                //CustomSwitches = "--viewport-size 1280x1024",
                PageOrientation = Orientation.Portrait,
                //FileName = "Relatorio"
            };

        }

    }
}