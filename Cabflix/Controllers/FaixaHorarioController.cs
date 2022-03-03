using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class FaixaHorarioController : MasterController
    {

        private Context data = new Context();

        // GET: FaixaHorario
        public ActionResult Index()
        {
            var usuarioLogado = GetUsuarioLogado();

            //BreadCrumb Config
            ViewBag.Controller = "FaixaHorario";
            ViewBag.Action = "Index";
            ViewBag.Tela = "FaixaHorario";

            if (usuarioLogado.FkEmpresa != 1)
            {
                var faixaHorario = data.FaixaDeHorarios.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).ToList();
                return View(faixaHorario);
            }
            else
            {
                var faixaHorario = data.FaixaDeHorarios.OrderBy(x => x.Nome).ToList();
                return View(faixaHorario);
            }
        }
    }
}