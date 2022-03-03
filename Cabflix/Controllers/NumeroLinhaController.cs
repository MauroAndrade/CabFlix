using Cabflix.Models.Database;
using Cabflix.Models.ViewModel;
using Cabflix.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class NumeroLinhaController : MasterController
    {

        private Context data = new Context();

        // GET: NumeroLinha
        public ActionResult Index()
        {
            var numeroLinha = data.NumeroLinhas.OrderBy(x => x.Numero).ToList();
            return View(numeroLinha);
        }

        // GET: NumeroLinha/Create
        public ActionResult Create()
        {
            CarregaViewbag();
            return View();
        }

        // POST: NumeroLinha/Create
        [HttpPost]
        public ActionResult Create(NumeroLinhaViewmodel viewmodel)
        {
            var usuarioLogado = GetUsuarioLogado();

            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }

            if (data.NumeroLinhas.Count(n => n.Numero == viewmodel.Numero) > 0)
            {
                ModelState.AddModelError("Index", "Número já Cadastrado!");
                return View(viewmodel);
            }

            try
            {
                var numeroLinha = new NumeroLinha
                {
                    Numero = viewmodel.Numero.Trim(),
                    FkContato = viewmodel.FkContato,
                    FkConta = viewmodel.FkConta,
                    FkEmpresa = viewmodel.FkEmpresa,
                    FkCentroCusto = viewmodel.FkCentroCusto
                };

                data.NumeroLinhas.Add(numeroLinha);
                data.SaveChanges();

                return RedirectToAction("Index", "NumeroLinha");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: NumeroLinha/Edit/5
        public ActionResult Edit(string id)
        {
            var usuarioLogado = GetUsuarioLogado();

            if (int.TryParse(id.Descriptografar(), out int cod)) { }

            var numeroLinha = data.NumeroLinhas.Find(cod);

            try
            {
                NumeroLinhaViewmodel viewmodel = new NumeroLinhaViewmodel();

                viewmodel.Id = numeroLinha.Id.ToString().Criptografar();
                viewmodel.Numero = numeroLinha.Numero;
                viewmodel.FkContato = numeroLinha.FkContato;
                viewmodel.FkConta = numeroLinha.FkConta;
                viewmodel.FkEmpresa = numeroLinha.FkEmpresa;
                viewmodel.FkCentroCusto = numeroLinha.FkCentroCusto;

                CarregaViewbag();

                return View(viewmodel);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        // POST: NumeroLinha/Edit/5
        [HttpPost]
        public ActionResult Edit(NumeroLinhaViewmodel viewmodel) 
        {
            var usuarioLogado = GetUsuarioLogado();

            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }

            if (int.TryParse(viewmodel.Id.Descriptografar(), out int cod)) { }

            var numeroLinha = data.NumeroLinhas.Find(cod);

            if (data.NumeroLinhas.Count(n => n.Numero == viewmodel.Numero && n.Id != cod) > 0)
            {
                CarregaViewbag();
                ModelState.AddModelError("Index", "Número já Cadastrado!");
                return View(viewmodel);
            }

            try
            {
                numeroLinha.Numero = viewmodel.Numero.Trim();
                numeroLinha.FkContato = viewmodel.FkContato;
                numeroLinha.FkConta = viewmodel.FkConta;
                numeroLinha.FkEmpresa = viewmodel.FkEmpresa;
                numeroLinha.FkCentroCusto = viewmodel.FkCentroCusto;

                data.SaveChanges();

                return RedirectToAction("Index", "NumeroLinha");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public void CarregaViewbag()
        {
            var usuarioLogado = GetUsuarioLogado();
            ViewBag.Nivel = new Context().CcNivels.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).OrderBy(n => n.Indice);
            ViewBag.CentroCusto = new Context().CentroCustoes.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa); 
            ViewBag.Empresa = new Context().Empresas.OrderBy(e => e.Nome);
            ViewBag.Conta = new Context().Contas.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa);
            ViewBag.Contato = new Context().Contatoes.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).OrderBy(x => x.Nome);
        }

        public JsonResult GetCentroCusto() 
        {
            var lista = data.CentroCustoes.OrderBy(x => x.Nome).ToList();
            var retorno = new List<object>();

            foreach (var item in lista)
            {
                //var hash = new Hashtable();
                //hash.Add("value", item.Id);
                //hash.Add("text", item.CcNivel.Nome + "  -->  " + item.Nome);
                //retorno.Add(hash);
            }

            return Json(retorno, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}