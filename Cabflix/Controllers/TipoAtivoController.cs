using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class TipoAtivoController : MasterController
    {
        private Context data = new Context();

        // GET: TipoAtivo
        public ActionResult Index()
        {
            var usuarioLogado = GetUsuarioLogado();
            //BreadCrumb Config
            ViewBag.Controller = "TipoAtivo";
            ViewBag.Action = "Index";
            ViewBag.Tela = "TipoAtivo";

            if (usuarioLogado.FkEmpresa != 1)
            {
                var tipoAtivo = data.TipoAtivoes.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).ToList(); 
                return View(tipoAtivo);
            }
            else
            {
                var tipoAtivo = data.TipoAtivoes.OrderBy(x => x.Nome).ToList();
                return View(tipoAtivo);
            }
        }

    }
}