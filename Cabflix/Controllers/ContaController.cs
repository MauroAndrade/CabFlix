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
    public class ContaController : MasterController
    {
        private Context data = new Context();

        // GET: Conta
        public ActionResult Index()
        {
            var usuarioLogado = GetUsuarioLogado();
            var conta = data.Contas.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).ToList();
            return View(conta);
        }

        // GET: Conta/Create
        public ActionResult Create()
        {
            CarregaViewbag();
            return View();
        }

        [HttpPost]
        public ActionResult Create(ContaViewModel viewmodel) 
        {
            CarregaViewbag();
            var usuarioLogado = GetUsuarioLogado();

            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }

            if (data.Contas.Count(c => c.Codigo == viewmodel.Codigo) > 0)
            {
                ModelState.AddModelError("Codigo", "Conta já cadastrada!");
                return View(viewmodel);
            }

            try
            {
                Conta conta = new Conta
                {
                    Codigo = viewmodel.Codigo,
                    Cnpj = viewmodel.Cnpj,
                    RazaoSocial = viewmodel.RazaoSocial,
                    Vencimento = Convert.ToDateTime(viewmodel.Vencimento),
                    Prazo = viewmodel.Prazo,
                    InicioVigencia = Convert.ToDateTime(viewmodel.InicioVigencia),
                    FimVigencia = Convert.ToDateTime(viewmodel.FimVigencia),
                    PrimeiraFatura = Convert.ToDateTime(viewmodel.PrimeiraFatura),
                    UltimaFatura = Convert.ToDateTime(viewmodel.UltimaFatura),
                    Observacao = viewmodel.Observacao,
                    FkOperadora = viewmodel.FkOperadora,
                    FkEmpresa = usuarioLogado.FkEmpresa,
                    FkTipoConta = viewmodel.FkTipoConta
                };

                data.Contas.Add(conta);
                data.SaveChanges();

                return RedirectToAction("Index", "Conta");

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        // GET: Conta/Edit/5
        public ActionResult Edit(string id)
        {
            ContaViewModel viewModel = new ContaViewModel();

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            CarregaViewbag();

            if (int.TryParse(id.Descriptografar(), out int cod)) { }

            try
            {
                Conta conta = data.Contas.FirstOrDefault(u => u.Id == cod);

                viewModel.Id = conta.Id.ToString().Criptografar();
                viewModel.Codigo = conta.Codigo;
                viewModel.Cnpj = conta.Cnpj;
                viewModel.RazaoSocial = conta.RazaoSocial;
                viewModel.Vencimento = conta.Vencimento;
                viewModel.Prazo = conta.Prazo;
                viewModel.InicioVigencia = conta.InicioVigencia;
                viewModel.FimVigencia = conta.FimVigencia;
                viewModel.PrimeiraFatura = conta.PrimeiraFatura;
                viewModel.UltimaFatura = conta.UltimaFatura;
                viewModel.Observacao = conta.Observacao;
                viewModel.FkOperadora = conta.FkOperadora;
                viewModel.FkTipoConta = conta.FkTipoConta;

                return View(viewModel);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public void CarregaViewbag()
        {
            var usuarioLogado = GetUsuarioLogado();
            ViewBag.Operadora = new Context().Operadoras.OrderBy(p => p.Nome).ToList();
            ViewBag.TipoConta = new Context().TipoContas.OrderBy(e => e.Nome).ToList();
            ViewBag.Conta = new Context().Contas.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa);
        }

        public JsonResult GetConta()
        {
            var lista = data.Contas.OrderBy(x => x.Codigo).ToList();
            var retorno = new List<object>();

            foreach (var item in lista)
            {
                var hash = new Hashtable();
                hash.Add("value", item.Id);
                hash.Add("text", item.Codigo + " - " + item.Operadora.Nome );
                retorno.Add(hash);
            }

            return Json(retorno, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}