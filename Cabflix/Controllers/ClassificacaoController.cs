using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class ClassificacaoController : MasterController
    {

        private Context data = new Context();

        // GET: Classificacao
        public ActionResult Index()
        {
            var usuarioLogado = GetUsuarioLogado();

            //BreadCrumb Config
            ViewBag.Controller = "Classificacao";
            ViewBag.Action = "Index";
            ViewBag.Tela = "Classificacao";

            if (usuarioLogado.FkEmpresa != 1)
            {
                var classificacao = data.ClassificacaoDeLigacaos.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).ToList(); 
                return View(classificacao);
            }
            else
            {
                var classificacao = data.ClassificacaoDeLigacaos.OrderBy(x => x.Nome).ToList();
                return View(classificacao); 
            }
            
        }

    }
}