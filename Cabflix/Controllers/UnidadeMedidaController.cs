using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class UnidadeMedidaController : MasterController
    {
        private Context data = new Context();

        // GET: UnidadeMedida
        public ActionResult Index()
        {
            //BreadCrumb Config
            ViewBag.Controller = "UnidadeMedida";
            ViewBag.Action = "Index";
            ViewBag.Tela = "UnidadeMedida";

            var unidadeMedida = data.UnidadeDeMedidas.OrderBy(x => x.Nome).ToList(); 
            return View(unidadeMedida);
        }


    }
}