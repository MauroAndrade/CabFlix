using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class AtivoController : MasterController
    {
        private Context data = new Context();

        // GET: Ativo
        public ActionResult Index()
        {
            var usuarioLogado = GetUsuarioLogado();

            //BreadCrumb Config
            ViewBag.Controller = "Ativo";
            ViewBag.Action = "Index";
            ViewBag.Tela = "Ativo";

            if (usuarioLogado.FkEmpresa != 1)
            {
                var ativo = data.Ativoes.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).ToList(); 
                return View(ativo);
            }
            else
            {
                var ativo = data.Ativoes.OrderBy(x => x.Identificador).ToList();
                return View(ativo);
            }
        }


    }
}