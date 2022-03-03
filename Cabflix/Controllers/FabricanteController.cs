using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class FabricanteController : MasterController
    {
        private Context data = new Context();

        // GET: Fabricante
        public ActionResult Index()
        {
            var usuarioLogado = GetUsuarioLogado();

            //BreadCrumb Config
            ViewBag.Controller = "Fabricante";
            ViewBag.Action = "Index";
            ViewBag.Tela = "Fabricante";

            if (usuarioLogado.FkEmpresa != 1)
            {
                var fabricante = data.Fabricantes.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).ToList();
                return View(fabricante);
            }
            else
            {
                var fabricante = data.Fabricantes.OrderBy(x => x.Nome).ToList();
                return View(fabricante);
            }
        }


    }
}