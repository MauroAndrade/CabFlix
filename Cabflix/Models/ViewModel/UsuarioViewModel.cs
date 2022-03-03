using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cabflix.Models.ViewModel
{
    public class UsuarioViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Informe seu Nome!")]
        [MaxLength(150, ErrorMessage = "O nome deve ter até 150 caracteres!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o Login!")]
        [MaxLength(100, ErrorMessage = "O login dave ter até 100 caracteres!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Informe o Email!")]
        [MaxLength(100, ErrorMessage = "O Email dave ter até 100 caracteres!")]
        public string Email { get; set; }

        [Display(Name ="CPF")]
        [Required(ErrorMessage = "Informe o Cpf!")]
        [MaxLength(100, ErrorMessage = "O Cpf dave ter até 11 caracteres!")]
        public string Cpf { get; set; }

        [Display(Name = "Perfil")]
        public int FKPerfil { get; set; }

        [Display(Name = "Empresa")]
        public int FKEmpresa { get; set; } 

        public bool Status { get; set; }

        public bool Removed { get; set; } 

        //[Required(ErrorMessage = "Informe a Senha Atual")]
        //[DataType(DataType.Password)]
        //[MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres!")]
        //[MaxLength(20, ErrorMessage = "A senha deve ter até 20 caracteres!")]
        //public string Senha { get; set; }

        //[Required(ErrorMessage = "Informe a Nova Senha")]
        //[DataType(DataType.Password)]
        //[MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres!")]
        //[MaxLength(20, ErrorMessage = "A senha deve ter até 20 caracteres!")]
        //public string NovaSenha { get; set; } 

        //[Required(ErrorMessage = "Informe a Nova Senha")]
        //[DataType(DataType.Password)]
        //[Display(Name = "Confirmar Senha")]
        //[MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
        //[MaxLength(20, ErrorMessage = "A senha deve ter até 20 caracteres!")]
        //[Compare(nameof(Senha), ErrorMessage = "A senha e a confirmação não são iguais")]
        //public string ConfirmacaoNovaSenha { get; set; } 

    }
}