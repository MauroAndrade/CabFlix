using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cabflix.Models.ViewModel
{
    public class NumeroLinhaViewmodel
    {
        public string Id { get; set; } // ID (Primary key)
        [Display(Name = "Número da Linha")]
        [Required(ErrorMessage = "Informe o Número da Linha!")]
        [MaxLength(100, ErrorMessage = "O Nome dave ter até 50 caracteres!")]
        public string Numero { get; set; } // NUMERO (length: 50)
        [Display(Name = "Contato")]
        [Required(ErrorMessage = "Informe o Contato!")]
        public int FkContato { get; set; } // FK_CONTATO
        [Display(Name = "Número da Conta")]
        [Required(ErrorMessage = "Informe o Número da Conta!")]
        public int FkConta { get; set; } // FK_CONTA
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Informe a Empresa!")]
        public int FkEmpresa { get; set; } // FK_EMPRESA
        [Display(Name = "Centro de Custo")]
        [Required(ErrorMessage = "Informe o Centro de Custo!")]
        public int FkCentroCusto { get; set; } // FK_CENTRO_CUSTO
        
    }
}