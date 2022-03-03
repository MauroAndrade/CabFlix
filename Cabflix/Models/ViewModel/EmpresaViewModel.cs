using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Cabflix.Models.ViewModel
{
    public class EmpresaViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o CNPJ!")]
        public string CNPJ { get; set; } 

        [Required(ErrorMessage = "Informe o Nome!")]
        [MaxLength(200, ErrorMessage = "O Nome deve ter até 200 caracteres!")]
        public string Nome { get; set; }

        [Display(Name = "Razão Social")]
        [Required(ErrorMessage = "Informe a Razão Social!")]
        [MaxLength(200, ErrorMessage = "A Razão Social deve ter até 200 caracteres!")]
        public string RazaoSocial { get; set; } 

        [Display(Name = "Inscrição Estadual")]
        [MaxLength(50, ErrorMessage = "A Inscrição Estadual deve ter até 50 caracteres!")]
        public string InscricaoEstadual { get; set; }

        [Display(Name = "Inscrição Municipal")]
        [MaxLength(50, ErrorMessage = "A Inscrição Municipal deve ter até 50 caracteres!")]
        public string InscricaoMunicipal { get; set; }

        [Display(Name = "Site")]
        [MaxLength(50, ErrorMessage = "O Site deve ter até 50 caracteres!")]
        public string Site { get; set; }

        [Display(Name = "Email")]
        [MaxLength(50, ErrorMessage = "O Email deve ter até 50 caracteres!")]
        public string Email { get; set; }

        [Display(Name = "Assinatura do Email")]
        [MaxLength(300, ErrorMessage = "O Assinatura do Email deve ter até 300 caracteres!")]
        [DataType(DataType.MultilineText)]
        public string AssinaturaEmail { get; set; }

        [Display(Name = "CEP")]
        [MaxLength(9, ErrorMessage = "O CEP deve ter até 9 caracteres!")]
        public string Cep { get; set; }

        [Display(Name = "Cidade")]
        [MaxLength(200, ErrorMessage = "A Cidade deve ter até 300 caracteres!")]
        public string Cidade { get; set; }

        [Display(Name = "Estado")]
        public int FKEstado { get; set; }

        [Display(Name = "Endereço")]
        [MaxLength(200, ErrorMessage = "O Endereço deve ter até 200 caracteres!")]
        public string Logradouro { get; set; }

        [Display(Name = "Número")]
        [MaxLength(10, ErrorMessage = "O Número deve ter até 10 caracteres!")]
        public string Numero { get; set; }

        [Display(Name = "Complemento")]
        [MaxLength(50, ErrorMessage = "O Complemento deve ter até 50 caracteres!")]
        public string Complemento { get; set; }

        [Display(Name = "Bairro")]
        [MaxLength(200, ErrorMessage = "O Bairro deve ter até 200 caracteres!")]
        public string Bairro { get; set; }

        public bool Status { get; set; }

        [Display(Name = "Telefone")]
        [MaxLength(50, ErrorMessage = "O Telefone deve ter até 11 caracteres!")]
        public string Telefone { get; set; }
    }
}