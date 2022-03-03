using Cabflix.Models.Database;
using Cabflix.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class PainelController : MasterController
    {

        Context data = new Context();

        // GET: Painel
        public ActionResult Index(FormCollection filtro)
        {
            //BreadCrumb Config
            ViewBag.Controller = "Painel";
            ViewBag.Action = "Index";
            ViewBag.Tela = "Bem Vindo";

            var usuarioLogado = GetUsuarioLogado();


            string numero = filtro["numero"];
            string conta = filtro["conta"];
            string referencia = filtro["referencia"];

            var periodo = referencia != null ? referencia.Split('-') : null;
            var dataInicio = referencia != null ? Convert.ToDateTime(periodo[0].ToString()) : DateTime.Now.AddMonths(-1);
            var dataFim = referencia != null ? Convert.ToDateTime(periodo[1].ToString()) : DateTime.Now;

            ViewBag.ContaSelecionada = Convert.ToInt32(conta);
            ViewBag.NumeroSelecionada = Convert.ToInt32(numero);
            ViewBag.Referencia = dataInicio.ToShortDateString() + " - " + dataFim.ToShortDateString();

            ViewBag.Conta = data.Contas.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).ToList();
            ViewBag.Operadora = data.Operadoras.ToList();
            ViewBag.Numero = data.NumeroLinhas.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa);

            var listaContaPadronizada = data.ContaPadronizadas.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa && x.MesReferencia >= dataInicio && x.MesReferencia <= dataFim).ToList();
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

            //var resumoGrupo = data.ContaPadronizadas.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa)
            //    .GroupBy(x.)
            //    .ToList(); 

            var sqlGrupos = @"SELECT
                              GS.ID AS IdGrupo,
                              GS.NOME AS Grupo,
                              S.nome AS Servico,
                              (SELECT
                                COUNT(S.ID))
                              AS Quantidade,
                              (SELECT
                                SUM(CP.VALOR))
                              AS Valor,
                              (SELECT
                                SUM(CP.VALOR) / COUNT(S.ID))
                              AS Media,
                              (CASE
                                WHEN SUM(CP.VALOR) > 0 THEN ((SUM(CP.VALOR) * 100) / (SELECT
                                    SUM(CP2.VALOR)
                                  FROM CONTA_PADRONIZADA CP2)
                                  )
                                WHEN SUM(CP.VALOR) < 0 THEN ((SUM(CP.VALOR) * 100) / (SELECT
                                    SUM(CP2.VALOR)
                                  FROM CONTA_PADRONIZADA CP2)
                                  )
                                ELSE 0
                              END) AS Porcentagem
                            FROM CONTA_PADRONIZADA CP
                            INNER JOIN SERVICO S
                              ON S.ID = CP.FK_SERVICO
                            INNER JOIN GRUPO_DE_SERVICO GS
                              ON GS.ID = S.FK_GRUPO_DE_SERVICO
                            GROUP BY GS.ID, GS.NOME, S.NOME";

            List<PainelGruposViewModel> grupos = data.Database.SqlQuery<PainelGruposViewModel>(sqlGrupos).ToList();

            ViewBag.Grupos = grupos;

            ViewBag.ListaGruposServico = grupos.Select(x => x.Grupo).Distinct().ToList();

            string sqlResumoContato = @"SELECT 
                                        c.ID,
                                        c.NOME,
                                        n.NUMERO,
                                        SUM(cp.VALOR) as VALOR
                                        FROM CONTA_PADRONIZADA cp
                                        INNER JOIN NUMERO_LINHA n
                                        ON cp.FK_NUMERO_LINHA = n.ID
                                        INNER JOIN CONTATO c
                                        ON cp.FK_CONTATO = c.ID
                                        GROUP BY n.NUMERO, c.NOME, c.ID ";

            List<PainelResumoContatoViewModel> resumoContato = data.Database.SqlQuery<PainelResumoContatoViewModel>(sqlResumoContato).ToList();

            ViewBag.ResumoContato = resumoContato;


            return View();
        }
    }
}