using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cabflix.Models.ViewModel
{
    public class LoginViewModel
    {
        public string UrlRetorno { get; set; }

        [Required(ErrorMessage = "Informe o Login!")]
        [MaxLength(100, ErrorMessage = "O login dave ter até 100 caracteres!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Informe a Senha!")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres!")]
        [MaxLength(20, ErrorMessage = "A senha deve ter até 20 caracteres!")]
        public string Senha { get; set; }

        public Usuario Usuario { get; set; }

        public LoginViewModel()
        {
            Context data = new Context();
            Usuario = new Usuario();
        }
    }
}