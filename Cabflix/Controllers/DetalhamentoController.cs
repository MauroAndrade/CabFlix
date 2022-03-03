using Cabflix.Models.Database;
using NReco.PdfGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Cabflix.Models.ViewModel;
using Rotativa.Options;
using Rotativa;

namespace Cabflix.Controllers
{
    public class DetalhamentoController : MasterController
    {
        Context data = new Context();

        // GET: Detalhamento
        public ActionResult Index(FormCollection filtro)
        {
            string numero = filtro["numero"];
            string conta = filtro["conta"];
            string referencia = filtro["referencia"];

            var periodo = referencia != null ? referencia.Split('-') : null;
            var dataInicio = referencia != null ? Convert.ToDateTime(periodo[0].ToString()) : DateTime.Now.AddMonths(-1);
            var dataFim = referencia != null ? Convert.ToDateTime(periodo[1].ToString()) : DateTime.Now;

            //if (referencia != null && referencia != "")
            //{
            //    MesReferencia = Convert.ToDateTime(referencia);
            //    MesReferencia.AddDays(-MesReferencia.Day);
            //    MesReferencia.AddDays(1);
            //}
            //else
            //{
            //    var data = DateTime.Now;
            //    MesReferencia = new DateTime(data.Year, data.Month, 1);
            //}

            ViewBag.ContaSelecionada = Convert.ToInt32(conta);
            ViewBag.NumeroSelecionada = Convert.ToInt32(numero);
            ViewBag.Referencia = dataInicio.ToShortDateString() + " - " + dataFim.ToShortDateString();

            var usuarioLogado = GetUsuarioLogado();

            ViewBag.Conta = data.Contas.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).ToList();
            ViewBag.Operadora = data.Operadoras.ToList();
            ViewBag.Numero = data.NumeroLinhas.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa);

            var listaContaPadronizada = data.ContaPadronizadas.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa && x.MesReferencia >= dataInicio && x.MesReferencia <= dataFim).ToList();

            //var sql = @"SELECT 
	           //         OP.NOME AS Operadora,
	           //         TC.NOME AS Tipo,
	           //         C.CODIGO AS Conta,
	           //         NL.NUMERO AS Origem,
	           //         CP.NUMERO_DESTINO AS Destino,
	           //         CP.MES_REFERENCIA AS MesReferencia,
	           //         S.NOME AS Servico,
	           //         UM.SIMBOLO AS UnidadeMedida,
	           //         ISNULL(CP.MINUTOS_QTD, CP.DADOS_QTD) Quantidade,
	           //         CP.HORARIO AS Horario,
	           //         ISNULL(UF.SIGLA, '') AS UF,
	           //         ISNULL(PAIS.NOME, '') AS Pais,
	           //         CP.VALOR AS Valor
            //        FROM CONTA_PADRONIZADA CP
            //          INNER JOIN NUMERO_LINHA NL ON NL.ID = CP.FK_NUMERO_LINHA
            //          INNER JOIN CONTA C ON C.ID = NL.FK_CONTA
            //          INNER JOIN OPERADORA OP ON OP.ID = C.FK_OPERADORA
            //          INNER JOIN TIPO_CONTA TC ON TC.ID = C.FK_TIPO_CONTA
            //          INNER JOIN SERVICO S ON S.ID = CP.FK_SERVICO
            //          INNER JOIN UNIDADE_DE_MEDIDA UM ON UM.ID = S.FK_UNIDADE_DE_MEDIDA
            //          LEFT JOIN UF ON UF.ID = CP.FK_UF
            //          LEFT JOIN PAIS ON PAIS.ID = UF.ID";

            //var listaContaPadronizada1 = data.Database.SqlQuery<DetalhamentoViewModel>(sql).ToList();


            var ligacao = listaContaPadronizada.ToList();



            //Filtros
            var filtrado = false;
            if (numero != null && numero != "0")
            {
                ligacao = ligacao.Where(x => x.FkNumeroLinha == Convert.ToInt32(numero)).ToList();
                filtrado = true;
            }
            if (conta != null && conta != "0")
            {
                ligacao = ligacao.Where(x => x.NumeroLinha.FkConta == Convert.ToInt32(conta)).ToList();
                filtrado = true;
            }

            //Se houver filtro irá totalizar apenas do item filtrado
            if (filtrado)
            {
                int numeroId = Convert.ToInt32(numero);
                int contaId = Convert.ToInt32(conta);

                //Totalizadores pós filtro
                ViewBag.Total = ligacao.Sum(x => x.Valor);
                ViewBag.Dados = ligacao.Sum(x => x.DadosQtd);
                ViewBag.Registros = ligacao.Count();

                var NumeroLinhas = 0;
                if (numero != null && numero != "0")
                {
                    NumeroLinhas = data.NumeroLinhas.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa && x.Id == numeroId).Count();
                }
                if (conta != null && conta != "0")
                {
                    NumeroLinhas = data.NumeroLinhas.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa && x.FkConta == contaId).Count();
                }
                ViewBag.Numeros = NumeroLinhas;
            }
            else
            {
                //Totalizadores dos Cards
                ViewBag.Total = listaContaPadronizada.Sum(x => x.Valor);
                ViewBag.Dados = listaContaPadronizada.Sum(x => x.DadosQtd);
                ViewBag.Registros = listaContaPadronizada.Count();
                ViewBag.Numeros = data.NumeroLinhas.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).Count();
            }


            return View(ligacao);
        }
        // Relatorio or report bellow
        [HttpPost]
        public ActionResult RelatorioUsuarioDetalhado(string pData) 
        {
            var usuarioLogado = GetUsuarioLogado();

            var responsavel = data.Contatoes.Where(x => x.Id == 1).FirstOrDefault();

            var contato = data.Contatoes.FirstOrDefault(x => x.Id == 1);

            string sqlResumoPorGrupoServico = @"EXECUTE Proc_ResumoPorGrupoServico 'TESTE', 1;";
            var RelatorioResumoPorGrupoServico = data.Database.SqlQuery<ResumoPorGrupoServico>(sqlResumoPorGrupoServico).ToList(); 


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

            var relatorio = data.Database.SqlQuery<RelatorioUsuarioDetalhado>(sqlRelatorio).ToList();

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

        // Relatorio or report bellow
        [HttpGet]
        public ActionResult Relatorio()
        {
            var usuarioLogado = GetUsuarioLogado();

            var responsavel = data.Contatoes.Where(x => x.Id == 1).FirstOrDefault();

            var contato = data.Contatoes.FirstOrDefault(x => x.Id == 1);



            var relatorio = data.Database.SqlQuery<Models.ViewModel.RelatorioDetalhamentoUsuarioViewModel>("" +
                            "SELECT " +
                                "GDS.NOME AS Grupo, " +
                                "COUNT(GDS.ID) AS Qtd, " +
                                "SUM(CONTA.VALOR) AS Valor " +
                                "FROM[CabFlix].[dbo].[CONTA_PADRONIZADA] CONTA " +
                            "INNER JOIN CONTATO C ON C.ID = CONTA.FK_CONTATO  " +
                            "INNER JOIN SERVICO S ON S.ID = CONTA.FK_SERVICO " +
                            "INNER JOIN GRUPO_DE_SERVICO GDS ON GDS.ID = S.FK_GRUPO_DE_SERVICO " +
                            "WHERE CONTA.FK_CONTATO = 1 " +
                            "GROUP BY GDS.NOME " +
                            "ORDER BY GDS.NOME " +
                            "").ToList();


            if (usuarioLogado != null)
            {
                string grupos = "";

                foreach (var item in relatorio)
                {

                    grupos +=
                    "<tr>" +
                        "<td>" + item.Grupo + "</td> " +
                        "<td>" + item.Qtd + "</td> " +
                        "<td>  R$ " + string.Format("{0:N}", item.Valor) + "</td> " +
                    "</tr> ";
                }

                string tableGrupoServico = "<br><br><table class='content'> " +
                    "<tr>" +
                          "<th> Resumo por Grupo de Serviço </th> " +
                          "<th> Quantidade </th> " +
                          "<th> Valor </th> " +
                    "</tr>" +
                          $"{grupos}" +
                          //     "</tr> " +
                          "</tr></table>";

                string table = "<br><br><table class='content'><tr>" +
                          "<th> Company </th> " +
                          "<th> Contact </th> " +
                          "<th> Country </th> " +
                      "</tr><tr> " +
                          "<td> Alfreds Futterkiste </td> " +
                             "<td> Maria Anders </td> " +
                                "<td> Germany </td> " +
                            "</tr> " +
                            "<tr> " +
                                "<td> Centro comercial Moctezuma</td> " +
                                   "<td> Francisco Chang </td> " +
                                      "<td> Mexico </td> " +
                                  "</tr><tr> " +
                                      "<td> Ernst Handel </td> " +
                                         "<td> Roland Mendel </td> " +
                                            "<td> Austria </td> " +
                                        "</tr> " +
                                        "<tr> " +
                                            "<td> Island Trading </td> " +
                                               "<td> Helen Bennett </td> " +
                                                  "<td> UK </td> " +
                                              "</tr><tr> " +
                                                  "<td> Laughing Bacchus Winecellars</td> " +
                                                     "<td> Yoshi Tannamuri </td> " +
                                                        "<td> Canada </td> " +
                                                    "</tr><tr> " +
                                                        "<td> Magazzini Alimentari Riuniti</td>" +
                                                           "<td> Giovanni Rovelli </td>" +
                                                              "<td> Italy </td></tr></table>";




                string title = usuarioLogado.Empresa.RazaoSocial;
                string content =

                    //"<br><br><br>Kou Ichinomiya is the son of a wealthy businessman who holds firm belief in his elite status. As such, he is determined to avoid becoming indebted to anyone; but one day, after a run-in with some mischievous kids on Arakawa Bridge, he ends up falling into the river running underneath. Luckily for him, a passerby is there to save him—but now, he owes his life to this stranger! Angered by this, Kou insists on paying her back, but this may just be the worst deal the arrogant businessman has ever made. The stranger—a stoic, tracksuit-wearing homeless girl known only as Nino—lives in a cardboard box under the bridge and wants only one thing: to fall in love. Asking Kou to be her boyfriend, he has no choice but to accept, forcing him to move out of his comfortable home and start a new life under the bridge! [Written by MAL Rewrite] " +

                    "<br><br> <b>Periodo: </b> " +

                    $"{tableGrupoServico}" +
                    //  $"{table}" +
                    "<br><img src='https://mindfusion.eu/product_images/javascript/library/chart/free/js_combi_chart.png'>" +
                    "<br><br>Kou Ichinomiya is the son of a wealthy businessman who holds firm belief in his elite status. As such, he is determined to avoid becoming indebted to anyone; but one day, after a run-in with some mischievous kids on Arakawa Bridge, he ends up falling into the river running underneath. Luckily for him, a passerby is there to save him—but now, he owes his life to this stranger! Angered by this, Kou insists on paying her back, but this may just be the worst deal the arrogant businessman has ever made. The stranger—a stoic, tracksuit-wearing homeless girl known only as Nino—lives in a cardboard box under the bridge and wants only one thing: to fall in love. Asking Kou to be her boyfriend, he has no choice but to accept, forcing him to move out of his comfortable home and start a new life under the bridge! [Written by MAL Rewrite] " +
                    "<br><br>Kou Ichinomiya is the son of a wealthy businessman who holds firm belief in his elite status. As such, he is determined to avoid becoming indebted to anyone; but one day, after a run-in with some mischievous kids on Arakawa Bridge, he ends up falling into the river running underneath. Luckily for him, a passerby is there to save him—but now, he owes his life to this stranger! Angered by this, Kou insists on paying her back, but this may just be the worst deal the arrogant businessman has ever made. The stranger—a stoic, tracksuit-wearing homeless girl known only as Nino—lives in a cardboard box under the bridge and wants only one thing: to fall in love. Asking Kou to be her boyfriend, he has no choice but to accept, forcing him to move out of his comfortable home and start a new life under the bridge! [Written by MAL Rewrite] ";

                string html = System.IO.File.ReadAllText(Server.MapPath("~/Templates/relatorio.html"));

                Uri myuri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                string pathQuery = myuri.PathAndQuery;
                string hostName = myuri.ToString().Replace(pathQuery, "");

                string address = "Rua Pref. Faria Lima, 755";
                string city = "Londrina/";
                string state = "PR";

                html = html.Replace("/*URL_PATH*/", hostName);
                html = html.Replace("/*ADDRESS*/", address + " - " + city + "/" + state);
                html = html.Replace("/*TITLE*/", title);
                html = html.Replace("/*BODY*/", content);

                html = html.Replace("/*PERIODO*/", DateTime.Now.ToShortDateString());
                html = html.Replace("/*RESPONSAVEL*/", responsavel.Nome);
                html = html.Replace("/*TOTAL*/", relatorio.Sum(x => x.Valor).ToString());


                DownloadReportAsPDF(html);

            }

            return null;
        }

        private void DownloadReportAsPDF(string body)
        {
            try
            {
                HtmlToPdfConverter obj = new HtmlToPdfConverter();
                string respondentName = string.Empty;

                obj.PageHeaderHtml = getHeader();
                obj.PageFooterHtml = getFooter();

                var pdfBytes = obj.GeneratePdf(body);
                Response.Clear();
                MemoryStream ms = new MemoryStream(pdfBytes);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", $"attachment; filename={Guid.NewGuid()}.pdf");
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
            catch (Exception ex)
            {
                Response.Write($"Erro ao gerar o PDF : {ex}");
            }
        }

        public string DatePtBR()
        {
            DateTime date = DateTime.Now;
            string day = date.DayOfWeek.ToString();
            switch (day)
            {
                case "Sunday":
                    day = "Domingo";
                    break;
                case "Monday":
                    day = "Segunda-feira";
                    break;
                case "Tuesday":
                    day = "Terça-feira";
                    break;
                case "Wednesday":
                    day = "Quarta-feira";
                    break;
                case "Thursday":
                    day = "Quinta-feira";
                    break;
                case "Fryday":
                    day = "Sexta-feira";
                    break;
                case "Saturday":
                    day = "Sábado";
                    break;
                default:
                    day = "";
                    break;

            }
            List<string> month = new List<string>();

            month.Add("Janeiro");
            month.Add("Fevereiro");
            month.Add("Março");
            month.Add("Abril");
            month.Add("Maio");
            month.Add("Junho");
            month.Add("Julho");
            month.Add("Agosto");
            month.Add("Setembro");
            month.Add("Outubro");
            month.Add("Novembro");
            month.Add("Dezembro");


            return day + " " + date.Day.ToString() + " " + month.ElementAt(Convert.ToInt32(date.Month.ToString()) - 1) + " " + date.Year.ToString();
        }

        protected string getHeader()
        {
            return "<table  width='100%' ><tr><td>&nbsp;</td><td></td></tr></table>";
        }
        protected string getFooter()
        {
            return "<table width='100%' >" +
                "<tr>" +
                "<td>&nbsp;</td>" +
                "<td></td>" +
                "</tr>" +
                "<tr border='0' bordercolor='blue' bgcolor='#f1f1f1'>" +
                "<td style='font-family:sans-serif; font-size:12px; padding: 1px; letter-spacing: 1px; margin-left: 2px;'>" +
                "<p>&nbsp;" + DatePtBR() + "</p></td>" +
                "<td align='right' style='font-family:sans-serif; font-size:12px; padding: 2px; letter-spacing: 1px; margin-right: 2px;'>" +
                "<div>Página <span class='page'></span></strong>&nbsp;</span></div>" +
                "</td>" +
                "</tr>" +
                "</table>";
        }

    }
}