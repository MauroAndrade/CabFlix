using Cabflix.Models.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class OperadoraController : MasterController
    {
        private Context data = new Context();

        // GET: Operadora
        public ActionResult Index()
        {
            //BreadCrumb Config
            ViewBag.Controller = "Operadora";
            ViewBag.Action = "Index";
            ViewBag.Tela = "Operadora";

            var operadora = data.Operadoras.OrderBy(x => x.Nome).ToList();
            return View(operadora);
        }

        public JsonResult GetOperadora() 
        {
            var lista = data.Operadoras.OrderBy(x => x.Nome).ToList();
            var retorno = new List<object>();

            foreach (var item in lista)
            {
                var hash = new Hashtable();
                hash.Add("value", item.Id);
                hash.Add("text", item.Nome);
                retorno.Add(hash);
            }

            return Json(retorno, "application/json", JsonRequestBehavior.AllowGet);
        }

    }
}