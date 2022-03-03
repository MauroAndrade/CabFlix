using Cabflix.Models.Database;
using Cabflix.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class AgendaController : MasterController
    {
        private Context data = new Context();

        // GET: Agenda
        public ActionResult Index(bool status = false)
        {
            var usuarioLogado = GetUsuarioLogado();

            //BreadCrumb Config
            ViewBag.Controller = "Agenda";
            ViewBag.Action = "Index";
            ViewBag.Tela = "Agenda";

            ViewBag.Status = status == true ? false : true;

            if (usuarioLogado.FkEmpresa != 1)
            {

                var sql = @"SELECT A.ID, 
                                   A.NOME, 
                                   A.TELEFONE, 
                                   A.INDIVIDUAL, 
                                   A.REMOVED, 
                                   CL.NOME AS CLASSIFICACAO
                            FROM AGENDA A
                            INNER JOIN CLASSIFICACAO_DE_LIGACAO CL
                                ON CL.ID = A.FK_CLASSIFICACAO
                            WHERE FK_EMPRESA = " + usuarioLogado.FkEmpresa +
                            " AND A.REMOVED = " + (status ? 1 : 0);

                var listaAgenda = data.Database.SqlQuery<AgendaViewModel>(sql).ToList();
                return View(listaAgenda);
            }
            else
            {
                var sql = @"SELECT A.ID, 
                                   A.NOME, 
                                   A.TELEFONE, 
                                   A.INDIVIDUAL, 
                                   A.REMOVED,     
                                   CL.NOME AS CLASSIFICACAO
                            FROM AGENDA A
                            INNER JOIN CLASSIFICACAO_DE_LIGACAO CL
                                ON CL.ID = A.FK_CLASSIFICACAO
	                        WHERE A.REMOVED =" + (status ? 1 : 0) +
                            " ORDER BY A.NOME";

                var listaAgenda = data.Database.SqlQuery<AgendaViewModel>(sql).ToList();
                return View(listaAgenda);
            }
        }

        public ActionResult Create()
        {
            //BreadCrumb Config
            ViewBag.Controller = "Agenda";
            ViewBag.Action = "Create";
            ViewBag.Tela = "Cadastrar Agenda";

            var usuarioLogado = GetUsuarioLogado();
            ViewBag.ClassificacaoLigacao = getClassificacaoDeLigacao(usuarioLogado.FkEmpresa);

            return PartialView("_Create");
        }

        [HttpPost]
        public JsonResult Create(AgendaEditViewModel a)
        {
            //CarregaViewbag();
            var usuarioLogado = GetUsuarioLogado();

            if (!ModelState.IsValid)
            {
                return Json(new JsonReturn { Status = "Erro!", Message = "Model State Inválido!" }, "application/json", JsonRequestBehavior.AllowGet);
            }

            var validaAgenda = data.Agenda.Where(x => (x.Nome.ToUpper() == a.Nome.ToUpper() || x.Telefone == a.Telefone) && x.Empresa.Id == usuarioLogado.FkEmpresa).FirstOrDefault();

            if (validaAgenda != null)
            {
                if (validaAgenda.Nome.ToUpper() == a.Nome.ToUpper())
                {
                    return Json(new JsonReturn { Status = "Erro!", Message = "Agenda já existe!" }, "application/json", JsonRequestBehavior.AllowGet);
                }
                if (validaAgenda.Telefone == a.Telefone)
                {
                    return Json(new JsonReturn { Status = "Erro!", Message = "Telefone já existe!" }, "application/json", JsonRequestBehavior.AllowGet);
                }
            }

            try
            {
                Agendum agenda = new Agendum();
                agenda.FkEmpresa = usuarioLogado.FkEmpresa;
                agenda.Nome = a.Nome.Trim();
                agenda.Telefone= a.Telefone.Trim();
                agenda.FkClassificacao = a.FkClassificacao;
                agenda.Individual = a.Individual;
                agenda.Removed = false;

                data.Agenda.Add(agenda);
                data.SaveChanges();

                return Json(new JsonReturn { Status = "Sucesso!", Message = "Salvo com Sucesso!" }, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new JsonReturn { Status = "Erro!", Message = e.Message }, "application/json", JsonRequestBehavior.AllowGet);
            }
        }

        public IEnumerable<ClassificacaoDeLigacao> getClassificacaoDeLigacao(int idEmpresa)
        {
            var lista = data.ClassificacaoDeLigacaos.Where(x => x.FkEmpresa == idEmpresa && x.Removed == false).AsEnumerable();
            return lista;
        }

    }
}