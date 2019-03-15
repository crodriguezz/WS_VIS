using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSPagoServicio
{
    /// <summary>
    /// PERMITE VALIDAR LA CONECTIVIDAD DEL USUARIO QUE DESEA CONECTARSE.
    /// DEVUELVE UN TOKEN DE CONECTIVIDAD
    /// </summary>
    public class SecureTokenWebService : System.Web.Services.Protocols.SoapHeader
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string TokenAutenticacion { get; set; }
        

        public bool UsuarioCorrecto(string Usuario, string Password)
        {
            if (Usuario.Equals(Properties.Resources.UserName) && Password.Equals(Properties.Resources.UserPassword))            
                return true;
            else
                return false;
        }

        public bool UsuarioCorrecto(SecureTokenWebService SoapHeader)
        {
            if (SoapHeader == null)            
                return false;
            
            //validamos token
            if (!string.IsNullOrEmpty(SoapHeader.TokenAutenticacion))
                return (HttpRuntime.Cache[SoapHeader.TokenAutenticacion] != null);            

            return false;
        }
    }
}