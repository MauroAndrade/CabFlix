using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cabflix.Models.ViewModel
{
    public class AgendaViewModel
    {
        public int Id { get; set; } // ID (Primary key)

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Informe o Nome!")]
        [MaxLength(100, ErrorMessage = "O Nome dave ter até 100 caracteres!")]
        public string Nome { get; set; } // NOME (length: 100)

        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "Informe o Telefone!")]
        [MaxLength(50, ErrorMessage = "O Telefone dave ter até 50 caracteres!")]
        public string Telefone { get; set; } // TELEFONE (length: 50)

        public bool Individual { get; set; } // INDIVIDUAL

        public string Classificacao { get; set; } // FK_CLASSIFICACAO

        public bool Removed { get; set; }

    }
}