using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using WSPagoServicio.Clases;
using MQLibrary;

namespace WSPagoServicio
{

    /// <summary> --- Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
    /// Descrión del Web Service Pago Servicio
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class PagoServicio : WebService
    {
        internal Operaciones operaciones;
        public SecureTokenWebService SoapHeader;

        [WebMethod]
        [System.Web.Services.Protocols.SoapHeader("SoapHeader")]
        public RespuestaAutenticacion AutenticarUsuario()
        {
            RespuestaAutenticacion autenticacion = new RespuestaAutenticacion
            {
                TIP_OPER = Properties.Resources.CodErrorAutorizacion,
                STATUS = "Ingrese Usuario y Contraseña"
            };

            if (SoapHeader == null)
                return autenticacion;
            if (string.IsNullOrEmpty(SoapHeader.Usuario) || string.IsNullOrEmpty(SoapHeader.Password))
                return autenticacion;


            autenticacion.STATUS = "Credenciales Invalidas";
            if (!SoapHeader.UsuarioCorrecto(SoapHeader.Usuario, SoapHeader.Password))
                return autenticacion;

            // Create and store the TokenAutenticacion before returning it.
            string token = Guid.NewGuid().ToString();
            HttpRuntime.Cache.Add(
              token,
              SoapHeader.Usuario,
              null,
              DateTime.Now.AddMinutes(5),//tiempo de expiracion de Item en Cache
              TimeSpan.Zero, 
              System.Web.Caching.CacheItemPriority.NotRemovable, 
              null);

            autenticacion.STATUS = string.Empty;
            autenticacion.TOKEN = token;
            autenticacion.TIP_OPER = Properties.Resources.CodAutorizacionExitosa;

            return autenticacion;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapHeader("SoapHeader")]
        public DatosRespuestaConsulta ConsultaSaldo(DatosConsulta datos)
        {
            DatosRespuestaConsulta respuesta_datos = new DatosRespuestaConsulta
            {
                TIP_OPER = Properties.Resources.CodErrorAutorizacion,
                STATUS = "Implementar metodo AutenticarUsuario de primero"
            };

            if (SoapHeader == null)
                return respuesta_datos;

            if (!SoapHeader.ValidarToken(SoapHeader))
                return respuesta_datos;

            operaciones = new Operaciones();
            respuesta_datos = operaciones.RealizarConsulta(datos);

            return respuesta_datos;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapHeader("SoapHeader")]
        public DatosRespuestaPago PagoEnLinea(DatosPago datos)
        {
            DatosRespuestaPago respuesta_datos = new DatosRespuestaPago
            {
                TIP_OPER = Properties.Resources.CodErrorAutorizacion,
                STATUS = "Implementar metodo AutenticarUsuario de primero"
            };

            if (SoapHeader == null)
                return respuesta_datos;

            if (!SoapHeader.ValidarToken(SoapHeader))
                return respuesta_datos;

            operaciones = new Operaciones();
            respuesta_datos = operaciones.PagarVisa(datos);
            return respuesta_datos;
        }        

    }
}
