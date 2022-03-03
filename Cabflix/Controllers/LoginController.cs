using Cabflix.Models.Database;
using Cabflix.Models.ViewModel;
using Cabflix.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.Title = "Login";

            return View();
        }

        public ActionResult Login(string ReturnUrl)
        {

            ViewBag.Title = "Login";

            var viewmodel = new LoginViewModel
            {
                UrlRetorno = ReturnUrl
            };

            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("Login", "Login Incorreto!");
                return View(viewmodel);
            }

            Context data = new Context();
            var usuario = data.Usuarios.FirstOrDefault(u => u.Login == viewmodel.Login);

            if (usuario == null)
            {
                ModelState.AddModelError("Login", "Login Incorreto!");

                return View(viewmodel);
            }
            if (usuario.Senha != Hash.GerarHash(viewmodel.Senha))
            {
                ModelState.AddModelError("Senha", "Senha Incorreta!");

                return View(viewmodel);
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim("UserId", usuario.Id.ToString()),
                new Claim("Login", usuario.Login),
                new Claim("Empresa", usuario.Empresa.RazaoSocial),
                new Claim(ClaimTypes.Email, usuario.Email)
            }, "ApplicationCookie");

            //Recupera valor de um Claim customizado
            //var nome = identity.Claims.Where(f => f.Type == "Login").Select(f => f.Value).SingleOrDefault().ToString(); 

     

            Request.GetOwinContext().Authentication.SignIn(identity);

            HttpCookie c = new HttpCookie("Nome");
            c.Value = usuario.Nome;
            Response.Cookies.Add(c);

            HttpCookie e = new HttpCookie("Empresa");
            e.Value = usuario.Empresa.RazaoSocial;
            Response.Cookies.Add(e);

            HttpCookie user = new HttpCookie("User");
            user.Value = usuario.Id.ToString(); 
            Response.Cookies.Add(user); 

            if (!String.IsNullOrWhiteSpace(viewmodel.UrlRetorno) || Url.IsLocalUrl(viewmodel.UrlRetorno))
            {
                return Redirect(viewmodel.UrlRetorno);
            }
            else
            {
                return RedirectToAction("Index", "Painel");
            }

        }

        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");



            HttpCookie c = new HttpCookie("Nome");
            HttpCookie e = new HttpCookie("Empresa");
            HttpCookie user = new HttpCookie("User"); 
            
            c.Value = "";
            e.Value = "";
            user.Value = ""; 

            Response.Cookies.Add(c);
            Response.Cookies.Add(e);
            Response.Cookies.Add(user); 

            return RedirectToAction("Login", "Login");
        }

        public Usuario GetUsuarioLogado()
        {
            var w = Request.GetOwinContext().Authentication.User;
            var UserId = w.Claims.Where(f => f.Type == "UserId").Select(f => f.Value).SingleOrDefault();
            var id = System.Convert.ToInt32(UserId);

            Context data = new Context();
            var usuario = data.Usuarios.FirstOrDefault(u => u.Id == id);

            return usuario;
        }
        


    }
}
