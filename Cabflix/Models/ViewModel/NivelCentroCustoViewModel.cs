using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cabflix.Models.ViewModel
{
    public class NivelCentroCustoViewModel
    {
        public NivelCentroCustoViewModel()
        {
            
        }

        [Key]
        public string Id { get; set; }

        public int Indice { get; set; }

        [Required(ErrorMessage = "Informe o Nivel!")]
        [MaxLength(150, ErrorMessage = "O nivel deve ter até 100 caracteres!")]
        public string Nome { get; set; }

        [Display(Name = "Nivel")]
        public int FK_CC_NIVEL { get; set; }

        [Display(Name = "Empresa")]
        public int Fk_Empresa { get; set; } // FK_EMPRESA
    }
}