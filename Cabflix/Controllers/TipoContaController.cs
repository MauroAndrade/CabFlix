using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class TipoContaController : MasterController
    {
        private Context data = new Context();

        // GET: TipoConta
        public ActionResult Index()
        {

            //BreadCrumb Config
            ViewBag.Controller = "TipoConta";
            ViewBag.Action = "Index";
            ViewBag.Tela = "TipoConta";

            var tipoConta = data.TipoContas.OrderBy(x => x.Nome).ToList();
            return View(tipoConta);
        }


    }
}