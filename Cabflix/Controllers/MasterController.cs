using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cabflix.Controllers
{
    public class MasterController : Controller
    {

        Context data = new Context();

        public Usuario GetUsuarioLogado()
        {
            var w = Request.GetOwinContext().Authentication.User;
            var UserId = w.Claims.Where(f => f.Type == "UserId").Select(f => f.Value).SingleOrDefault();
            var id = System.Convert.ToInt32(UserId);

            var usuario = data.Usuarios.FirstOrDefault(u => u.Id == id);

            return usuario;
        }

        public Contato GetContato(int id)
        {
            Contato contato = data.Contatoes.FirstOrDefault(x => x.Id == id);

            return contato;
        }

    }
}