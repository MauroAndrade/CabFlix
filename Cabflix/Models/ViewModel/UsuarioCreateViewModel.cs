using Cabflix.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cabflix.Models.ViewModel
{
    public class UsuarioCreateViewModel
    {
        [Required(ErrorMessage = "Informe seu Nome!")]
        [MaxLength(150, ErrorMessage = "O nome deve ter até 150 caracteres!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o Login!")]
        [MaxLength(100, ErrorMessage = "O login dave ter até 100 caracteres!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Informe o Email!")]
        [MaxLength(100, ErrorMessage = "O Email dave ter até 100 caracteres!")]
        public string Email { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "Informe o Cpf!")]
        [MaxLength(100, ErrorMessage = "O Cpf dave ter até 11 caracteres!")]
        public string Cpf { get; set; }

        [Display(Name = "Perfil")]
        public int FKPerfil { get; set; }

        [Display(Name = "Empresa")]
        public int FKEmpresa { get; set; }

        public bool Status { get; set; }

        [Required(ErrorMessage = "Informe a Senha!")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres!")]
        [MaxLength(20, ErrorMessage = "A senha deve ter até 20 caracteres!")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Informe a Senha!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
        [MaxLength(20, ErrorMessage = "A senha deve ter até 20 caracteres!")]
        [Compare(nameof(Senha), ErrorMessage = "A senha e a confirmação não são iguais!")]
        public string ConfirmacaoSenha { get; set; }

        public Usuario Usuario { get; set; }

        public UsuarioCreateViewModel()
        {
            Context data = new Context();
            Usuario = new Usuario();
        }

        public UsuarioCreateViewModel(int idUsuario)
        {

            Context data = new Context();

            Usuario = data.Usuarios.Find(idUsuario);

            //Context data = new Context();
            //Processos = data.CADASTRO_PROCESSOS.Find(idProcesso);
            //PartesEnvolvidas = data.PROCESSO_PARTES_ENVOLVIDAS.Where(x => x.FK_ID_PROCESSO == idProcesso).ToList();
            //Fases = data.ACOMPANHAMENTO_FASES.Where(x => x.FK_ID_PROCESSO == idProcesso).ToList();
            //NovaParteEnvolvida = new PROCESSO_PARTES_ENVOLVIDAS();
            //NovaFase = new ACOMPANHAMENTO_FASES();
            //ParteEnvolvida = new PARTE_ENVOLVIDA();
            //ListaParteEnvolvida = data.PARTE_ENVOLVIDA.OrderBy(x => x.NM_PARTE_ENVOLVIDA).ToList().ToPagedList(1, 50);
            //ListaUsuario = data.USUARIO.OrderBy(x => x.DS_LOGIN_US).ToList().ToPagedList(1, 50);
            //NovaFaseResponsavel = new PROCESSO_FASE_RESPONSAVEL();
            //Usuarios = data.USUARIO.OrderBy(x => x.NM_USUARIO).ToList();
            //ListaHonorarios = data.CADASTRO_PROCESSOS.Where(x => x.ID_PROCESSO == idProcesso).ToList();
        }
    }
}