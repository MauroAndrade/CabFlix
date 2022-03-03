using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Cabflix.Models.ViewModel;
using Cabflix.Models.Database;
using System.IO;
using System.Text;

namespace Cabflix.Controllers
{
    public class FaturaController : MasterController
    {

        private Context data = new Context();

        // GET: Fatura
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string UploadFatura(string[] list, string myjson, string operadora, string mes, string conta)
        {
            //Numero da Conta
            //Quantidade de linhas
            //Mes de referencia
            //Botao aprovar para a importacao e status de aprovado
            //Lista de Erros para exibir as validacoes dos campos
            //Data de Envio
            //Na hora do envio deve haver botao de reiniciar fatura para esta opcao a importacao deve remover a anterior e add a atual ou senao
            //apenas add novas linhas de codigo

            var usuarioLogado = GetUsuarioLogado();
            List<FaturaViewModel> RegistroIncorreto = new List<FaturaViewModel>();

            var dtMesReferencia = new DateTime();

            foreach (var item in list)
            {
                FaturaViewModel registro = JsonConvert.DeserializeObject<FaturaViewModel>(item);

                //validar tratamento de erro caso não encontre algum item abaixo
                var listaLinha = data.NumeroLinhas.Where(x => x.FkEmpresa == usuarioLogado.FkEmpresa).ToList();

                var linha = listaLinha.Where(x => x.Numero == registro.numOrigem).FirstOrDefault();

                ContaPadronizada contaPadronizada = new ContaPadronizada();

                if (linha != null)
                {
                    var servico = data.Servicoes.Where(x => x.Nome == registro.servico).FirstOrDefault();
                    var uf = data.Ufs.Where(x => x.Sigla.ToUpper() == registro.estado.ToUpper()).FirstOrDefault();

                    var t = registro.horario.Split(':');
                    var hora = "";
                    var minuto = "";
                    var segundo = "";
                    DateTime dt;
                    if (t.Length > 1)
                    {
                        hora = t[0];
                        minuto = t[1];
                        segundo = t[2];
                        dt = new DateTime(2020, 01, 01, Convert.ToInt32(hora), Convert.ToInt32(minuto), Convert.ToInt32(segundo));
                    }
                    else
                    {
                        dt = new DateTime(1900, 01, 01);
                    }

                    var faixaHorario = data.FaixaDeHorarios.Where(x => x.Inicio <= dt && x.Fim >= dt).FirstOrDefault();

                    dtMesReferencia = GetMesReferencia(registro.mes); 

                    
                    contaPadronizada.FkNumeroLinha = linha.Id;
                    contaPadronizada.FkServico = servico.Id;
                    contaPadronizada.MesReferencia = dtMesReferencia;
                    contaPadronizada.Data = Convert.ToDateTime(registro.data);
                    contaPadronizada.Valor = Convert.ToDecimal(registro.valor);
                    contaPadronizada.NumeroDestino = registro.numDestino;
                    //contaPadronizada.FkCcNivel = linha.CentroCusto.FkCcNivel;
                    contaPadronizada.FkCentroCusto = linha.FkCentroCusto;
                    contaPadronizada.FkContato = linha.FkContato;
                    contaPadronizada.FkEmpresa = linha.FkEmpresa;

                    if (faixaHorario != null)
                    {
                        contaPadronizada.FkFaixaDeHorario = faixaHorario.Id;
                    }
                    if (uf != null)
                    {
                        contaPadronizada.FkUf = uf.Id;
                    }
                    if (registro.horario != "")
                    {
                        contaPadronizada.Horario = registro.horario;
                    }
                    if (registro.min != "")
                    {
                        contaPadronizada.MinutosQtd = Convert.ToDecimal(registro.min);
                    }
                    if (registro.dados != "")
                    {
                        contaPadronizada.DadosQtd = Convert.ToDecimal(registro.dados);
                    }
                    if (registro.duracao != "")
                    {
                        contaPadronizada.DuracaoQtd = Convert.ToDecimal(registro.duracao);
                    }
                    if (registro.numDestino != "")
                    {
                        contaPadronizada.NumeroDestino = registro.numDestino;
                    }
                    data.ContaPadronizadas.Add(contaPadronizada);
                    data.SaveChanges();
                }
                else
                {
                    RegistroIncorreto.Add(registro);
                }
            }
            LogImportacao log = new LogImportacao();

            
            //new DateTime(2020, 10, 1);

            log.QtdRegistros = list.Count();
            log.DataImportacao = DateTime.Now;
            log.MesReferencia = new DateTime(2020, 10, 1);
            log.QtdErros = RegistroIncorreto.Count();
            log.Aprovado = false;
            log.FkConta = Convert.ToInt32(conta);

            data.LogImportacaos.Add(log);
            data.SaveChanges();

            

            foreach (var item in RegistroIncorreto)
            {
                RegistrosIncorreto incorretos = new RegistrosIncorreto();

                incorretos.Operadora = item.operadora;
                incorretos.TipoServico = item.tipoServico;
                incorretos.NumNf = item.numNf;
                incorretos.NumOrigem = item.numOrigem;
                incorretos.NumDestino = item.numDestino;
                incorretos.Mes = item.mes;
                incorretos.Data = item.data;
                incorretos.Codigo = item.codigo;
                incorretos.Grupo = item.grupo;
                incorretos.Codigo2 = item.codigo2;
                incorretos.Servico = item.servico;
                incorretos.Duracao = item.duracao;
                incorretos.Min = item.min;
                incorretos.Dados = item.dados;
                incorretos.Horario = item.horario;
                incorretos.Cidade = item.cidade;
                incorretos.Regiao = item.regiao;
                incorretos.Estado = item.estado;
                incorretos.Pais = item.pais;
                incorretos.Valor = item.valor;
                incorretos.ValorCobrado = item.valorCobrado;
                incorretos.Degrau = item.degrau;
                incorretos.UfOrigem = item.ufOrigem;
                incorretos.FkLogImportacao = log.Id;

                data.RegistrosIncorretoes.Add(incorretos);
            }
            data.SaveChanges();
            //GerarLog(RegistroIncorreto);
            ExportarCSV(RegistroIncorreto, conta);

            return "foi";
            //return Json(faturas);
            //return Json(fatura, JsonRequestBehavior.AllowGet);
        }

        public void ExportarCSV(List<FaturaViewModel> incorreto, string contaId)  
        {
            var id = Convert.ToInt32(contaId);
            var csv = new StringBuilder();

            var conta = data.Contas.FirstOrDefault(x => x.Id == id);

            csv.AppendLine("FAT_OPERADORA; FAT_TIPO_DE_SER; FAT_NUM_NF; FAT_NUM_ORIG; FAT_NUM_DEST; FAT_MES; FAT_DATA; FAT_CODIGO; FAT_GRUPO; " +
                           "FAT_CODIGO2; FAT_SERVICO; FAT_DURACAO; FAT_MIN; FAT_DADOS; FAT_HORARIO; FAT_CIDADE; FAT_REGIAO; FAT_ESTADO; FAT_PAIS; " +
                           "FAT_VALOR; FAT_VALOR_COBRADO; FAT_DEGRAU; FAT_UF_ORIGEM");

            foreach (var item in incorreto)
            {
                csv.AppendLine(item.operadora + ";" +
                               item.tipoServico + ";" +
                               item.numNf + ";" +
                               item.numOrigem + ";" +
                               item.numDestino + ";" +
                               item.mes + ";" +
                               item.data + ";" +
                               item.codigo + ";" +
                               item.grupo + ";" +
                               item.codigo2 + ";" +
                               item.servico + ";" +
                               item.duracao + ";" +
                               item.min + ";" +
                               item.dados + ";" +
                               item.horario + ";" +
                               item.cidade + ";" +
                               item.regiao + ";" +
                               item.estado + ";" +
                               item.pais + ";" +
                               item.valor + ";" +
                               item.valorCobrado + ";" +
                               item.degrau + ";" +
                               item.ufOrigem);

                //Response.Write(Environment.NewLine);
            }
        
            string path = @"C:\Teste\" + conta.Codigo + ".csv";

            System.IO.File.WriteAllText(path, csv.ToString());

            //Response.End();
        }

        public DateTime GetMesReferencia(string d) 
        {
            var ArrayMesReferencia = d.Split('/');

            var mes = 0;
            switch (ArrayMesReferencia[0])
            {
                case "jan":
                    mes = 1;
                    break;
                case "fev":
                    mes = 2;
                    break;
                case "mar":
                    mes = 3;
                    break;
                case "abr":
                    mes = 4;
                    break;
                case "mai":
                    mes = 5;
                    break;
                case "jun":
                    mes = 6;
                    break;
                case "jul":
                    mes = 7;
                    break;
                case "ago":
                    mes = 8;
                    break;
                case "set":
                    mes = 9;
                    break;
                case "out":
                    mes = 10;
                    break;
                case "nov":
                    mes = 11;
                    break;
                case "dez":
                    mes = 12;
                    break;
                default:
                    break;
            }

            var ano = "20" + ArrayMesReferencia[1]; 


            var dtMesReferencia = new DateTime(Convert.ToInt32(ano), mes, 1);

            return dtMesReferencia;
        }

        //public void ExportarCSV(List<FaturaViewModel> incorreto)
        //{

        //    Response.Clear();
        //    Response.ContentType = "text/csv";
        //    Response.AppendHeader("Content-Disposition", "attachment; filename=Arquivo.csv");

        //    foreach (var item in incorreto)
        //    {
        //        Response.Write(item.operadora + "\t" + 
        //                       item.tipoServico + "\t" + 
        //                       item.numNf + "\t" + 
        //                       item.numOrigem + "\t" + 
        //                       item.numDestino + "\t" + 
        //                       item.mes + "\t" +
        //                       item.data + "\t" +
        //                       item.codigo + "\t" +
        //                       item.grupo + "\t" +
        //                       item.codigo2 + "\t" +
        //                       item.servico + "\t" +
        //                       item.duracao + "\t" +
        //                       item.min + "\t" +
        //                       item.dados + "\t" +
        //                       item.horario + "\t" +
        //                       item.cidade + "\t" +
        //                       item.regiao + "\t" +
        //                       item.estado + "\t" +
        //                       item.pais + "\t" +
        //                       item.valor + "\t" +
        //                       item.valorCobrado + "\t" +
        //                       item.degrau + "\t" +
        //                       item.ufOrigem + "\t");

        //        Response.Write(Environment.NewLine);
        //    }

        //    Response.End();
        //}

        //public void GerarLog(List<FaturaViewModel> incorreto)   
        //{
        //    ////declarando a variavel do tipo StreamWriter para 
        //    //abrir ou criar um arquivo para escrita 
        //    StreamWriter x;

        //    ////Colocando o caminho fisico e o nome do arquivo a ser criado
        //    //finalizando com .txt
        //    string CaminhoNome = "C:\\Users\\AntonioMarlon\\Desktop\\arq01.txt";

        //    //utilizando o metodo para criar um arquivo texto
        //    //e associando o caminho e nome ao metodo
        //    x = File.CreateText(CaminhoNome);

        //    //aqui, exemplo de escrever no arquivo texto
        //    //como se fossemos criar um recibo de pagamento

        //    //escrevendo o titulo
        //    x.WriteLine("Recibo de Pagamanto");
        //    //pulando linha sem escrita
        //    x.WriteLine();
        //    x.WriteLine();
        //    //escrevendo conteúdo do recibo
        //    x.WriteLine("Recebi do Sr: Nome do Pagador");
        //    x.WriteLine("a quantia de VALOR DO RECIBO (VALOR POR EXTENSO),");
        //    x.WriteLine("referente ao DESCRIÇÃO DO QUE FOI PAGO... ");
        //    x.WriteLine("CIDADE, DATA");
        //    x.WriteLine();
        //    x.WriteLine();
        //    x.WriteLine("__________________________________________________");
        //    x.WriteLine("Nome do pagador");
        //    x.WriteLine("CPF do pagador: CPF");
        //    x.WriteLine();

        //    //fechando o arquivo texto com o método .Close()
        //    x.Close();

            
        //}

    }
}