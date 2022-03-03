using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Threading.Tasks;


namespace Cabflix.Utils
{
    public static class Criptografia
    {
        private static byte[] key = { };
        private static byte[] iv = { 12, 34, 56, 78, 90, 102, 114, 126 };
        private static readonly string compl = "s&n@t_sp0pol()sc0s|";
        private static readonly string cryptoKey = "%2!b)f(g@3";

        /// <summary>
        /// Método para criptografar 
        /// </summary>
        /// <param name="value">Valor a ser criptografado</param>
        /// <param name="isMD5">Criptografa no MD5 ou normal, padrão normal </param>
        /// <returns></returns>
        public static string Criptografar(this string value, bool isMD5 = false)
        {
            value = compl + value;

            if (isMD5)
            {
                MD5 md5 = MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(value);
                byte[] hash = md5.ComputeHash(inputBytes);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                return sb.ToString();
            }
            else
            {
                DESCryptoServiceProvider des;
                MemoryStream ms;
                CryptoStream cs; byte[] input;

                des = new DESCryptoServiceProvider();
                ms = new MemoryStream();

                input = Encoding.UTF8.GetBytes(value); key = Encoding.UTF8.GetBytes(cryptoKey.Substring(0, 8));

                cs = new CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray())
                              .Replace("+", "_a0_")
                              .Replace("\\", "_b1_")
                              .Replace("/", "_c2_")
                              .Replace(":", "_d3_")
                              .Replace("*", "_e4_")
                              .Replace("?", "_f5_")
                              .Replace("\"", "_g6_")
                              .Replace("<", "_h7_")
                              .Replace(">", "_i8_")
                              .Replace("|", "_j9_");
            }
        }

        public static string Descriptografar(this string value)
        {
            value = value
                    .Replace("_a0_", "+")
                    .Replace("_b1_", "\\")
                    .Replace("_c2_", "/")
                    .Replace("_d3_", ":")
                    .Replace("_e4_", "*")
                    .Replace("_f5_", "?")
                    .Replace("_g6_", "\"")
                    .Replace("_h7_", "<")
                    .Replace("_i8_", ">")
                    .Replace("_j9_", "|");

            DESCryptoServiceProvider des;
            MemoryStream ms;
            CryptoStream cs; byte[] input;

            des = new DESCryptoServiceProvider();
            ms = new MemoryStream();

            input = new byte[value.Length];
            input = Convert.FromBase64String(value.Replace(" ", "+"));

            key = Encoding.UTF8.GetBytes(cryptoKey.Substring(0, 8));

            cs = new CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.FlushFinalBlock();

            return Encoding.UTF8.GetString(ms.ToArray()).Replace(compl, string.Empty);
        }

    }
}