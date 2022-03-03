using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using Cabflix.Models.ViewModel;
using Cabflix.Models.Database;
using Newtonsoft.Json;

namespace Cabflix.Controllers
{
    public class UploadController : MasterController
    {

        private Context data = new Context();

        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, string conta)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);

                    ReadCSV(_path, conta);

                }
                ViewBag.Message = "File Uploaded Successfully!!";




                return RedirectToAction("Index", "Fatura");
            }
            catch(Exception e)
            {
                ViewBag.Message = "File upload failed!!";
                return RedirectToAction("Index", "Fatura");
            }
        }

        public void ReadCSV(string filePath, string conta)
        {
            var usuarioLogado = GetUsuarioLogado();
            List<FaturaViewModel> RegistroIncorreto = new List<FaturaViewModel>();
            var dtMesReferencia = new DateTime();

            var sum = 0d;
            var count = 0;
            string line;

            // Open the stream and read it back.
            using (var fs = System.IO.File.OpenRead(filePath))
            using (var reader = new StreamReader(fs))


                while ((line = reader.ReadLine()) != null)
                {
                    if (count > 0)
                    {
                        var parts = line.Split(';');

                        var registro = new FaturaViewModel
                        {
                            operadora = parts[0],
                            tipoServico = parts[1],
                            numNf = parts[2],
                            numOrigem = parts[3],
                            numDestino = parts[4],
                            mes = parts[5],
                            data = parts[6],
                            codigo = parts[7],
                            grupo = parts[8],
                            codigo2 = parts[9],
                            servico = parts[10],
                            duracao = parts[11],
                            min = parts[12],
                            dados = parts[13],
                            horario = parts[14],
                            cidade = parts[15],
                            regiao = parts[16],
                            estado = parts[17],
                            pais = parts[18],
                            valor = parts[19],
                            valorCobrado = parts[20],
                            degrau = parts[21],
                            ufOrigem = parts[22]
                        };

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
                    count++;
                }
            LogImportacao log = new LogImportacao();

            //new DateTime(2020, 10, 1);

            log.QtdRegistros = count;
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






            //{
            //    byte[] b = new byte[1024];
            //    UTF8Encoding temp = new UTF8Encoding(true);

            //    while (fs.Read(b, 0, b.Length) > 0)
            //    {
            //        Console.WriteLine(temp.GetString(b));
            //    }
            //}

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

    }
}