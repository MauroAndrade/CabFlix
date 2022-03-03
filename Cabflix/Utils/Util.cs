using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cabflix.Utils
{
    public class Util
    {

        public string GetIp()
        {
            //Pegar IP          
            string Meuip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(Meuip))
            {
                Meuip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return Meuip;

        }


    }
}