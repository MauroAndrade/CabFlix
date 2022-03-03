using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cabflix.Models.ViewModel
{
    public class ContaViewModel
    {
        public string Id { get; set; } // ID (Primary key)
        [Display(Name = "Código")]
        [Required(ErrorMessage = "Informe o Código!")]
        public string Codigo { get; set; } // CODIGO (length: 50)
        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "Informe o CNPJ!")]
        public string Cnpj { get; set; } // CNPJ (length: 50)
        [Display(Name = "Razão Social")]
        [Required(ErrorMessage = "Informe a Razão Social!")]
        public string RazaoSocial { get; set; } // RAZAO_SOCIAL (length: 50)
        [Display(Name = "Vencimento")]
        [Required(ErrorMessage = "Informe o Vencimento!")]
        public DateTime Vencimento { get; set; } // VENCIMENTO
        [Display(Name = "Prazo")]
        [Required(ErrorMessage = "Informe o Prazo!")]
        public int Prazo { get; set; } // PRAZO
        [Display(Name = "Início da Vigência")]
        [Required(ErrorMessage = "Informe o Início da Vigência!")]
        public DateTime InicioVigencia { get; set; } // INICIO_VIGENCIA
        [Display(Name = "Fim da Vigência")]
        [Required(ErrorMessage = "Informe o Fim da Vigência!")]
        public DateTime? FimVigencia { get; set; } // FIM_VIGENCIA
        [Display(Name = "Primeira Fatura")]
        [Required(ErrorMessage = "Informe a data da primeira Fatura!")]
        public DateTime? PrimeiraFatura { get; set; } // PRIMEIRA_FATURA
        [Display(Name = "Última Fatura")]
        [Required(ErrorMessage = "Informe a data da última Fatura!")]
        public DateTime? UltimaFatura { get; set; } // ULTIMA_FATURA
        [Display(Name = "Observação")]
        public string Observacao { get; set; } // OBSERVACAO (length: 150)
        [Display(Name = "Operadora")]
        [Required(ErrorMessage = "Informe a Operadora!")]
        public int FkOperadora { get; set; } // FK_OPERADORA
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Informe o Tipo!")]
        public int FkTipoConta { get; set; } // FK_TIPO_CONTA

        // Reverse navigation

        /// <summary>
        /// Child NumeroLinhas where [NUMERO_LINHA].[FK_CONTA] point to this entity (FK_NUMERO_LINHA_CONTA)
        /// </summary>
        public virtual ICollection<NumeroLinha> NumeroLinhas { get; set; } // NUMERO_LINHA.FK_NUMERO_LINHA_CONTA

        // Foreign keys

        /// <summary>
        /// Parent Empresa pointed by [CONTA].([FkEmpresa]) (FK_CONTA_EMPRESA)
        /// </summary>
        public virtual Empresa Empresa { get; set; } // FK_CONTA_EMPRESA

        /// <summary>
        /// Parent Operadora pointed by [CONTA].([FkOperadora]) (FK_CONTA_OPERADORA)
        /// </summary>
        public virtual Operadora Operadora { get; set; } // FK_CONTA_OPERADORA

        /// <summary>
        /// Parent TipoConta pointed by [CONTA].([FkTipoConta]) (FK_CONTA_TIPO_CONTA)
        /// </summary>
        public virtual TipoConta TipoConta { get; set; } // FK_CONTA_TIPO_CONTA

        public ContaViewModel()
        {
            NumeroLinhas = new List<NumeroLinha>(); 
        }
    }
}