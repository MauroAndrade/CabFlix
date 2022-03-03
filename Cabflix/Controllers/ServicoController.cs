using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class ServicoController : MasterController
    {
        private Context data = new Context();

        // GET: Servico
        public ActionResult Index()
        {
            CarregaViewbag();

            //BreadCrumb Config
            ViewBag.Controller = "Servico";
            ViewBag.Action = "Index";
            ViewBag.Tela = "Servico";

            var servico = data.Servicoes.OrderBy(x => x.Nome).ToList();
            return View(servico);
        }

        public void CarregaViewbag()
        {
            ViewBag.GrupoServico = new Context().GrupoDeServicoes.OrderBy(p => p.Nome);
            ViewBag.UnidadeMedida = new Context().UnidadeDeMedidas.OrderBy(e => e.Nome);
        }


    }
}