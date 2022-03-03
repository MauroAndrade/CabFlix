using Cabflix.Models.Database;
using Cabflix.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class EmpresaController : MasterController
    {

        private Context data = new Context();

        // GET: Usuario
        public ActionResult Index()
        {
            var usuarioLogado = GetUsuarioLogado();

            //BreadCrumb Config
            ViewBag.Controller = "Empresa";
            ViewBag.Action = "Index";
            ViewBag.Tela = "Empresa";

            if (usuarioLogado.FkEmpresa != 1)
            {
                var empresa = data.Empresas.Where(x => x.Id == usuarioLogado.FkEmpresa).ToList(); 
                return View(empresa);
            }
            else
            {
                var empresa = data.Empresas.OrderBy(x => x.Nome).ToList();
                return View(empresa); 
            }

        }

        // GET: Empresa/Create
        public ActionResult Create()
        {

            //BreadCrumb Config
            ViewBag.Controller = "Empresa";
            ViewBag.Action = "Create";
            ViewBag.Tela = "Cadastrar Empresa";

            CarregaViewbag();

            return View();
        }

        [HttpPost]
        public ActionResult Create(EmpresaViewModel viewmodel)
        {
            
            var usuarioLogado = GetUsuarioLogado();

            if (!ModelState.IsValid)
            {
                CarregaViewbag();
                return View(viewmodel);
            }

            if (data.Empresas.Count(u => u.Cnpj == viewmodel.CNPJ) > 0)
            {
                CarregaViewbag();
                ModelState.AddModelError("Cnpj", "Este CNPJ já está cadastrado!");
                return View(viewmodel);
            }

            try
            {
                Empresa novaEmpresa = new Empresa
                {
                    Cnpj = viewmodel.CNPJ,
                    Nome = viewmodel.Nome,
                    RazaoSocial = viewmodel.RazaoSocial,
                    InscricaoEstadual = viewmodel.InscricaoEstadual,
                    InscricaoMunicipal = viewmodel.InscricaoMunicipal,
                    Site = viewmodel.Site,
                    Email = viewmodel.Email,
                    AssinaturaEmail = viewmodel.AssinaturaEmail,
                    Cep = viewmodel.Cep,
                    Cidade = viewmodel.Cidade,
                    FkEstado = viewmodel.FKEstado,
                    Logradouro = viewmodel.Logradouro,
                    Numero = viewmodel.Numero,
                    Complemento = viewmodel.Complemento,
                    Bairro = viewmodel.Bairro,
                    Status = viewmodel.Status,
                    Telefone = viewmodel.Telefone
                };

                data.Empresas.Add(novaEmpresa);
                data.SaveChanges();

                return RedirectToAction("Index", "Empresa");

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            EmpresaViewModel viewModel = new EmpresaViewModel(); 

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            //BreadCrumb Config
            ViewBag.Controller = "Empresa";
            ViewBag.Action = "Edit";
            ViewBag.Tela = "Editar Empresa";

            CarregaViewbag();

            try
            {
                Empresa empresa = data.Empresas.FirstOrDefault(x => x.Id == id); 

                viewModel.Id = empresa.Id;
                viewModel.AssinaturaEmail = empresa.AssinaturaEmail;
                viewModel.Bairro = empresa.Bairro;
                viewModel.Cep = empresa.Cep;
                viewModel.Cidade = empresa.Cidade;
                viewModel.CNPJ = empresa.Cnpj;
                viewModel.Complemento = empresa.Complemento;
                viewModel.Email = empresa.Email;
                viewModel.FKEstado = empresa.FkEstado;
                viewModel.InscricaoEstadual = empresa.InscricaoEstadual;
                viewModel.InscricaoMunicipal = empresa.InscricaoMunicipal;
                viewModel.Logradouro = empresa.Logradouro;
                viewModel.Nome = empresa.Nome;
                viewModel.Numero = empresa.Numero;
                viewModel.RazaoSocial = empresa.RazaoSocial;
                viewModel.Site = empresa.Site;
                viewModel.Status = empresa.Status;
                viewModel.Telefone = empresa.Telefone;

                return View(viewModel);

            }
            catch (Exception e)
            {

                throw;
            }

        }

        [HttpPost]
        public ActionResult Edit(EmpresaViewModel viewModel) 
        {
            CarregaViewbag();
            var usuarioLogado = GetUsuarioLogado();

            //BreadCrumb Config
            ViewBag.Controller = "Empresa";
            ViewBag.Action = "Edit";
            ViewBag.Tela = "Editar Empresa";

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (data.Empresas.Count(u => u.Cnpj == viewModel.CNPJ && u.Id != viewModel.Id) > 0)
            {
                ModelState.AddModelError("Cnpj", "Este CNPJ já está em uso!");
                return View(viewModel);
            }

            try
            {
                Empresa empresa = data.Empresas.FirstOrDefault(u => u.Id == viewModel.Id);

                empresa.Id = viewModel.Id;
                empresa.AssinaturaEmail = viewModel.AssinaturaEmail;
                empresa.Bairro = viewModel.Bairro;
                empresa.Cep = viewModel.Cep;
                empresa.Cidade = viewModel.Cidade;
                empresa.Cnpj= viewModel.CNPJ;
                empresa.Complemento = viewModel.Complemento;
                empresa.Email = viewModel.Email;
                empresa.FkEstado = viewModel.FKEstado;
                empresa.InscricaoEstadual = viewModel.InscricaoEstadual;
                empresa.InscricaoMunicipal = viewModel.InscricaoMunicipal;
                empresa.Logradouro = viewModel.Logradouro;
                empresa.Nome = viewModel.Nome;
                empresa.Numero = viewModel.Numero;
                empresa.RazaoSocial = viewModel.RazaoSocial;
                empresa.Site = viewModel.Site;
                empresa.Status = viewModel.Status;
                empresa.Telefone = viewModel.Telefone;

                data.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        ////GET: Usuario/Delete
        //public ActionResult Delete(int id)
        //{

        //    UsuarioViewModel UsuarioViewmodel = new UsuarioViewModel();

        //    if (!ModelState.IsValid)
        //    {
        //        return View(UsuarioViewmodel);
        //    }

        //    //BreadCrumb Config
        //    ViewBag.Controller = "Usuario";
        //    ViewBag.Action = "Delete";
        //    ViewBag.Tela = "Deletar Usuário";

        //    CarregaViewbag();

        //    try
        //    {
        //        Usuario usuario = data.Usuarios.FirstOrDefault(u => u.Id == id);

        //        UsuarioViewmodel.Id = usuario.Id;
        //        UsuarioViewmodel.Nome = usuario.Nome;
        //        UsuarioViewmodel.Cpf = usuario.Cpf;
        //        UsuarioViewmodel.Email = usuario.Email;
        //        UsuarioViewmodel.FKEmpresa = usuario.FkEmpresa;
        //        UsuarioViewmodel.FKPerfil = usuario.FkPerfil;
        //        UsuarioViewmodel.Login = usuario.Login;
        //        UsuarioViewmodel.Status = usuario.Status;


        //        return View(UsuarioViewmodel);

        //    }
        //    catch (Exception e)
        //    {

        //        throw;
        //    }
        //}

        //[HttpPost]
        //public ActionResult Delete(UsuarioViewModel usuarioViewModel)
        //{
        //    CarregaViewbag();

        //    var usuarioLogado = GetUsuarioLogado();

        //    //BreadCrumb Config
        //    ViewBag.Controller = "Usuario";
        //    ViewBag.Action = "Delete";
        //    ViewBag.Tela = "Deletar Usuário";

        //    try
        //    {
        //        Usuario usuario = data.Usuarios.FirstOrDefault(u => u.Id == usuarioViewModel.Id);

        //        usuario.Removed = true;
        //        usuario.UpdateAt = DateTime.Now;
        //        usuario.UserUpdate = usuarioLogado.Id;

        //        data.SaveChanges();

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        return View();
        //    }
        //}

        public void CarregaViewbag()
        {
            //ViewBag.Perfil = new Context().Perfils.OrderBy(p => p.Nome);
            //ViewBag.Empresa = new Context().Empresas.OrderBy(e => e.Nome);
            ViewBag.Estado = new Context().Ufs.OrderBy(e => e.Nome);
        }

    }
}