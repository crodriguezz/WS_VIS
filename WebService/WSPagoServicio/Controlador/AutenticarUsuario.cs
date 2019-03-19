using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSPagoServicio
{
    /// <summary>
    /// PERMITE VALIDAR LA CONECTIVIDAD DEL USUARIO QUE DESEA CONECTARSE.
    /// DEVUELVE UN TOKEN DE CONECTIVIDAD
    /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
    ///</summary>
    public class SecureTokenWebService : System.Web.Services.Protocols.SoapHeader
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string TokenAutenticacion { get; set; }

        /// <summary>
        /// Permite identificar si el ususario es correcto
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="Usuario">Nombre usuario</param>
        /// <param name="Password">Contraseaña encriptada</param>
        /// <returns></returns>
        public bool UsuarioCorrecto(string Usuario, string Password)
        {
            if (Usuario.Equals(Properties.Resources.UserName) && Password.Equals(Properties.Resources.UserPassword))            
                return true;
            else
                return false;
        }
        /// <summary>
        /// Valida que todavia tenga un Token Valido de lo contrario devuelve error
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="SoapHeader"></param>
        /// <returns></returns>
        public bool ValidarToken(SecureTokenWebService SoapHeader)
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