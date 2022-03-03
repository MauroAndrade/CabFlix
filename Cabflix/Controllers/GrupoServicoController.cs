using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class GrupoServicoController : MasterController
    {
        private Context data = new Context();

        // GET: GrupoServico
        public ActionResult Index()
        {
            //BreadCrumb Config
            ViewBag.Controller = "GrupoServico";
            ViewBag.Action = "Index";
            ViewBag.Tela = "GrupoServico";

            var grupoServico = data.GrupoDeServicoes.OrderBy(x => x.Nome).ToList();
            return View(grupoServico);
        }

    }
}