using Cabflix.Models.Database;
using Cabflix.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class NivelCentroCustoController : MasterController
    {
        private Context data = new Context();

        // GET: NivelCentroCusto
        public ActionResult Index()
        {

            var usuarioLogado = GetUsuarioLogado();
            //BreadCrumb Config
            ViewBag.Controller = "NivelCentroCusto";
            ViewBag.Action = "Index";
            ViewBag.Tela = "Operadora";

            var nivel = data.CcNivels.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).OrderBy(x => x.Indice).ToList();
            return View(nivel); 
        }

        // GET: Nivel/Create
        public ActionResult Create()
        {

            //BreadCrumb Config
            ViewBag.Controller = "Usuario";
            ViewBag.Action = "Create";
            ViewBag.Tela = "Cadastrar Usuário";

            CarregaViewbag();

            return View();
        }

        [HttpPost]
        public ActionResult Create(CcNivel model) 
        {
            CarregaViewbag();
            var usuarioLogado = GetUsuarioLogado();

            var niveis = data.CcNivels.Count(x => x.FkEmpresa == usuarioLogado.FkEmpresa);

            var maxIndice = 0;
            if (niveis > 0)
            {
                maxIndice = data.CcNivels.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).Max(x => x.Indice);
                maxIndice += 1;
            }
            else
            {
                maxIndice = 1;
            }

            model.Indice = maxIndice;
            model.FkEmpresa = usuarioLogado.FkEmpresa;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            //if (data.CcNivels.Count(n => n.Nome.ToUpper() == model.Nome.ToUpper() && n.Empresa.Id == model.FkEmpresa) > 0)
            //{
            //    ModelState.AddModelError("Index", "´Já existe Nível Cadastrado com esse nome!");
            //    return View(model);
            //}

            try
            {
                data.CcNivels.Add(model);
                data.SaveChanges();

                return RedirectToAction("Index", "NivelCentroCusto");
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        //// GET: Usuario/Edit/5
        //public ActionResult Edit(string id)
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

        //    if (int.TryParse(id.Descriptografar(), out int cod)) { }

        //    try
        //    {
        //        Usuario usuario = data.Usuarios.FirstOrDefault(u => u.Id == cod);

        //        UsuarioViewmodel.Id = usuario.Id.ToString().Criptografar();
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

        //    if (int.TryParse(usuarioViewModel.Id.Descriptografar(), out int cod)) { }

        //    if (data.Usuarios.Count(u => u.Login == usuarioViewModel.Login && u.Id != cod) > 0)
        //    {
        //        ModelState.AddModelError("Login", "Este login já está em uso!");
        //        return View(usuarioViewModel);
        //    }

        //    try
        //    {
        //        Usuario usuario = data.Usuarios.FirstOrDefault(u => u.Id == cod);

        //        usuario.Id = cod;
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

        //        UsuarioViewmodel.Id = usuario.Id.ToString().Criptografar();
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

        //    if (int.TryParse(usuarioViewModel.Id.Descriptografar(), out int cod)) { }

        //    try
        //    {
        //        Usuario usuario = data.Usuarios.FirstOrDefault(u => u.Id == cod);

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

        public ActionResult MoveUp(string id)
        {
            var usuarioLogado = GetUsuarioLogado();
            if (int.TryParse(id.Descriptografar(), out int cod)) { }

            var NivelUp = data.CcNivels.Find(cod);

            if (NivelUp.Indice > 1)
            {
                var NivelDown = data.CcNivels.Where(x => x.Indice == NivelUp.Indice-1).FirstOrDefault();
                NivelDown.Indice = NivelDown.Indice + 1;
                NivelUp.Indice = NivelUp.Indice - 1;

                data.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult MoveDown(string id) 
        {
            var usuarioLogado = GetUsuarioLogado();
            if (int.TryParse(id.Descriptografar(), out int cod)) { }

            var NivelDown = data.CcNivels.Find(cod);
            var NivelUp = data.CcNivels.Where(x => x.Indice == NivelDown.Indice + 1).FirstOrDefault(); 

            var maxIndice = data.CcNivels.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).Max(x => x.Indice);

            if (NivelDown.Indice < maxIndice)
            {
                NivelDown.Indice = NivelDown.Indice + 1;
                
                NivelUp.Indice = NivelUp.Indice - 1;

                data.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public void CarregaViewbag()
        {
            ViewBag.Nivel = new Context().CcNivels.OrderBy(n => n.Indice);
            //ViewBag.Perfil = new Context().Perfils.OrderBy(p => p.Nome);
            //ViewBag.Empresa = new Context().Empresas.OrderBy(e => e.Nome);
        }
    }
}