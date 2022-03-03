using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cabflix.Models.ViewModel
{
    public class CentroCustoEditViewModel
    {
        public string Id { get; set; } // ID (Primary key)

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Informe o Nome!")]
        [MaxLength(100, ErrorMessage = "O Nome dave ter até 50 caracteres!")]
        public string Nome { get; set; } // NOME (length: 50)

        [Display(Name = "Classificação")]
        [Required(ErrorMessage = "Informe a Classificação!")]
        [MaxLength(100, ErrorMessage = "A Classificação dave ter até 50 caracteres!")]
        public string Classificacao { get; set; } // CLASSIFICACAO (length: 50)

        public int FkCcNivel { get; set; } // FK_CC_NIVEL
        public int FkEmpresa { get; set; } // FK_EMPRESA

        [Display(Name = "Centro de Custo")]
        public int? FkPai { get; set; } // FK_PAI
    }
}