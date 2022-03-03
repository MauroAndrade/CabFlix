using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class PermissaoController : MasterController
    {

        private Context data = new Context();

        // GET: Permissao
        public ActionResult Index()
        {
            var usuarioLogado = GetUsuarioLogado();

            //BreadCrumb Config
            ViewBag.Controller = "Permissao";
            ViewBag.Action = "Index";
            ViewBag.Tela = "Permissão";

            //if (usuarioLogado.FkEmpresa != 1)
            //{
            //    var usuarios = data.Usuarios.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).ToList();
            //    return View(usuarios);
            //}
            //else
            //{
            //    var usuarios = data.Usuarios.Where(x => x.Removed == false).OrderBy(x => x.Nome).ToList();
            //    return View(usuarios);
            //}

            return View();

        }

        //// GET: Permissao/Create
        //public ActionResult Create()
        //{

        //    //BreadCrumb Config
        //    ViewBag.Controller = "Usuario";
        //    ViewBag.Action = "Create";
        //    ViewBag.Tela = "Cadastrar Usuário";

        //    CarregaViewbag();

        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Create(UsuarioCreateViewModel viewmodel)
        //{
        //    CarregaViewbag();
        //    var usuarioLogado = GetUsuarioLogado();

        //    if (!ModelState.IsValid)
        //    {
        //        return View(viewmodel);
        //    }

        //    if (data.Usuarios.Count(u => u.Login == viewmodel.Login) > 0)
        //    {
        //        ModelState.AddModelError("Login", "Este login já está em uso!");
        //        return View(viewmodel);
        //    }

        //    try
        //    {
        //        Usuario novoUsuario = new Usuario
        //        {
        //            Nome = viewmodel.Nome,
        //            Login = viewmodel.Login,
        //            Email = viewmodel.Email,
        //            Cpf = viewmodel.Cpf,
        //            FkPerfil = viewmodel.FKPerfil,
        //            FkEmpresa = viewmodel.FKEmpresa,
        //            Status = viewmodel.Status,
        //            Senha = Hash.GerarHash(viewmodel.Senha),
        //            CreateAt = DateTime.Now,
        //            UserCreate = usuarioLogado.Id,
        //            Removed = false
        //        };

        //        data.Usuarios.Add(novoUsuario);
        //        data.SaveChanges();

        //        return RedirectToAction("Index", "Usuario");

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        //// GET: Usuario/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    UsuarioViewModel UsuarioViewmodel = new UsuarioViewModel();

        //    if (!ModelState.IsValid)
        //    {
        //        return View(UsuarioViewmodel);
        //    }

        //    //BreadCrumb Config
        //    ViewBag.Controller = "Usuario";
        //    ViewBag.Action = "Create";
        //    ViewBag.Tela = "Cadastrar Usuário";

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

        //// POST: Usuario/Edit/5
        //[HttpPost]
        //public ActionResult Edit(UsuarioViewModel usuarioViewModel)
        //{
        //    CarregaViewbag();
        //    var usuarioLogado = GetUsuarioLogado();

        //    //BreadCrumb Config
        //    ViewBag.Controller = "Usuario";
        //    ViewBag.Action = "Create";
        //    ViewBag.Tela = "Cadastrar Usuário";

        //    if (!ModelState.IsValid)
        //    {
        //        return View(usuarioViewModel);
        //    }

        //    if (data.Usuarios.Count(u => u.Login == usuarioViewModel.Login && u.Id != usuarioViewModel.Id) > 0)
        //    {
        //        ModelState.AddModelError("Login", "Este login já está em uso!");
        //        return View(usuarioViewModel);
        //    }

        //    try
        //    {
        //        Usuario usuario = data.Usuarios.FirstOrDefault(u => u.Id == usuarioViewModel.Id);

        //        usuario.Id = usuarioViewModel.Id;
        //        usuario.Nome = usuarioViewModel.Nome;
        //        usuario.Cpf = usuarioViewModel.Cpf;
        //        usuario.Email = usuarioViewModel.Email;
        //        usuario.FkEmpresa = usuarioViewModel.FKEmpresa;
        //        usuario.FkPerfil = usuarioViewModel.FKPerfil;
        //        usuario.Login = usuarioViewModel.Login;
        //        usuario.Status = usuarioViewModel.Status;
        //        usuario.UserUpdate = usuarioLogado.Id;
        //        usuario.UpdateAt = DateTime.Now;
        //        usuario.UserUpdate = 1;

        //        data.SaveChanges();

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        return View();
        //    }
        //}

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
            ViewBag.Perfil = new Context().Perfils.OrderBy(p => p.Nome);
            ViewBag.Empresa = new Context().Empresas.OrderBy(e => e.Nome);
        }

    }
}