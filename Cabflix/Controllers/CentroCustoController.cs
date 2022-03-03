using Cabflix.Models.Database;
using Cabflix.Models.ViewModel;
using Cabflix.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class CentroCustoController : MasterController
    {
        Context data = new Context();

        // GET: CentroCusto
        public ActionResult Index(bool status = false)
        {
            var usuarioLogado = GetUsuarioLogado();
            //BreadCrumb Config
            ViewBag.Controller = "CentroCusto";
            ViewBag.Action = "Index";
            ViewBag.Tela = "CentroCusto";
            ViewBag.Status = status == true ? false : true;

            //ViewBag.ListaNivel = data.CcNivels.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).OrderBy(x => x.Indice).ToList();

            var nivel = data.CentroCustoes.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa && x.Removed == status).OrderBy(x => x.Classificacao).ToList();
            return View(nivel);
        }

        public ActionResult Create()
        {

            //BreadCrumb Config
            ViewBag.Controller = "Usuario";
            ViewBag.Action = "Create";
            ViewBag.Tela = "Cadastrar Usuário";

            CarregaViewbag();
            return PartialView("_Create");
            //return View();
        }

        [HttpPost]
        public JsonResult Create(CentroCustoViewModel cc)
        {
            CarregaViewbag();
            var usuarioLogado = GetUsuarioLogado();

            if (!ModelState.IsValid)
            {
                return Json(new JsonReturn { Status = "Erro!", Message = "Model State Inválido!" }, "application/json", JsonRequestBehavior.AllowGet);
            }

            var validaCentroCusto = data.CentroCustoes.Where(n => (n.Nome.ToUpper() == cc.Nome.ToUpper() || n.Classificacao == cc.Classificacao) && n.Empresa.Id == usuarioLogado.FkEmpresa).FirstOrDefault(); 

            if (validaCentroCusto != null)
            {
                if (validaCentroCusto.Nome.ToUpper() == cc.Nome.ToUpper())
                {
                    return Json(new JsonReturn { Status = "Erro!", Message = "Centro de Custo já existe!" }, "application/json", JsonRequestBehavior.AllowGet);
                }
                if (validaCentroCusto.Classificacao == cc.Classificacao)
                {
                    return Json(new JsonReturn { Status = "Erro!", Message = "Classificação já existe!" }, "application/json", JsonRequestBehavior.AllowGet);
                }
            }

            try
            {
                CentroCusto centroCusto = new CentroCusto();
                centroCusto.FkEmpresa = usuarioLogado.FkEmpresa;
                centroCusto.Nome = cc.Nome.Trim();
                centroCusto.FkPai = cc.FkPai;
                centroCusto.Classificacao = cc.Classificacao.Trim();

                data.CentroCustoes.Add(centroCusto);
                data.SaveChanges();

                return Json(new JsonReturn { Status = "Sucesso!", Message = "Salvo com Sucesso!" }, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new JsonReturn { Status = "Erro!", Message = e.Message }, "application/json", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            try
            {
                CarregaViewbag();
                if (int.TryParse(id.Descriptografar(), out int cod)) { }

                var cc = data.CentroCustoes.Where(x => x.Id == cod).FirstOrDefault();

                CentroCustoEditViewModel ccViewModel = new CentroCustoEditViewModel();

                ccViewModel.Id = cc.Id.ToString().Criptografar();
                ccViewModel.Classificacao = cc.Classificacao;
                ccViewModel.FkPai = cc.FkPai;
                ccViewModel.FkEmpresa = cc.FkEmpresa;
                ccViewModel.Nome = cc.Nome;

                return PartialView("_Edit", ccViewModel);
            }
            catch (Exception e)
            {
                throw;
            }
            
        }

        [HttpPost]
        public JsonResult Edit(CentroCustoEditViewModel CentroCustoViewModel) 
        {
            try
            {
                if (int.TryParse(CentroCustoViewModel.Id.Descriptografar(), out int cod)) { }

                var CentroCusto = data.CentroCustoes.Find(cod);

                CentroCusto.Nome = CentroCustoViewModel.Nome.Trim();
                CentroCusto.FkPai = CentroCustoViewModel.FkPai;
                CentroCusto.Classificacao = CentroCustoViewModel.Classificacao.Trim();

                data.SaveChanges();

                return Json(new JsonReturn { Status = "Sucesso!", Message = "Salvo com Sucesso!" }, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new JsonReturn { Status = "Erro!", Message = e.Message }, "application/json", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ChangeStatus(string id)  
        {
            try
            {
                if (int.TryParse(id.Descriptografar(), out int cod)) { }

                var CentroCusto = data.CentroCustoes.Find(cod);

                CentroCusto.Removed = CentroCusto.Removed == true ? false : true;

                data.SaveChanges();

                return Json(new JsonReturn { Status = "Sucesso!", Message = "Salvo com Sucesso!" }, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new JsonReturn { Status = "Erro!", Message = e.Message }, "application/json", JsonRequestBehavior.AllowGet);
            }
        }

        public void CarregaViewbag()
        {
            var usuarioLogado = GetUsuarioLogado();
            ViewBag.CentroCusto = new Context().CentroCustoes.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).OrderBy(x => x.Nome);
            NivelCentroCustoViewModel nivelViewmodel = new NivelCentroCustoViewModel(); 
        }
    }
}