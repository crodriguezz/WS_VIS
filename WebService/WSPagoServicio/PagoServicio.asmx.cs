using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using WSPagoServicio.Clases;

namespace WSPagoServicio
{   
    
    /// <summary>
    /// Descripción breve de PagoServicio
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
        public string AutenticarUsuario()
        {
            if (SoapHeader == null)
                return "Ingrese Usuario y Contraseña";
            if (string.IsNullOrEmpty(SoapHeader.Usuario) || string.IsNullOrEmpty(SoapHeader.Password))
                return "Ingrese Usuario y Contraseña";

            // Are the credentials valid?
            if (!SoapHeader.UsuarioCorrecto(SoapHeader.Usuario, SoapHeader.Password))
                return "Credenciales Invalidas";

            // Create and store the TokenAutenticacion before returning it.
            string token = Guid.NewGuid().ToString();
            HttpRuntime.Cache.Add(
              token,
              SoapHeader.Usuario,
              null,
              System.Web.Caching.Cache.NoAbsoluteExpiration,
              TimeSpan.FromMinutes(1),
              System.Web.Caching.CacheItemPriority.NotRemovable, 
              null);

            return token;
        }        
        

        [WebMethod]
        [System.Web.Services.Protocols.SoapHeader("SoapHeader")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]//Specify return format.
        public string PagoEnLinea(DatosPago datos)
        {
            if (SoapHeader == null)
                return "Implementar metodo AutenticarUsuario de primero";

            if (!SoapHeader.UsuarioCorrecto(SoapHeader))
                return "Implementar metodo AutenticarUsuario de primero";

            return "{ data:" + ConsultarVisa(datos) + "}";
        }
        internal string ConsultarVisa(DatosPago datos)
        {
            TestVisa.PaymentGWServices services = new TestVisa.PaymentGWServices();
            TestVisa.AuthorizationRequest test = new TestVisa.AuthorizationRequest
            {
                AuthorizationRequest1 = new TestVisa.Request()
            };

            test.AuthorizationRequest1.posEntryMode = "012"; //012 - TARJETA  022- LECTOR DE BANDA

            test.AuthorizationRequest1.pan = datos.TARJETA; //NO TARJETA
            test.AuthorizationRequest1.expdate = datos.FECHA_EXPIRACION; // FECHA EXPIRACION YYMM
            test.AuthorizationRequest1.amount = datos.MONTO; //MONTO DE CONSUMO
            //OPCIONAL
            //test.AuthorizationRequest1.track2Data = ""; //022 SI ES LECTURA DE BANDA
            test.AuthorizationRequest1.cvv2 = datos.CVV2;   // CODIGO DE SEGURIDAD

            test.AuthorizationRequest1.paymentgwIP = "190.149.69.135";
            //test.AuthorizationRequest1.shopperIP = "192.168.100.4";
            //test.AuthorizationRequest1.merchantServerIP = "";

            test.AuthorizationRequest1.merchantUser = "76B925EF7BEC821780B4B21479CE6482EA415896CF43006050B1DAD101669921"; //USUARIO
            test.AuthorizationRequest1.merchantPasswd = "DD1791DB5B28DDE6FBC2B9951DFED4D97B82EFD622B411F1FC16B88B052232C7"; //CONTRASEÑA
            test.AuthorizationRequest1.terminalId = "77788881"; //ID TERMINAL
            test.AuthorizationRequest1.merchant = "00575123"; //AFILICION

            test.AuthorizationRequest1.messageType = "0200"; // 0200 VENTA 0400 REVERSA  0202 ANULACION
            test.AuthorizationRequest1.auditNumber = "090249"; //NO. TRANSACCION
            //OPCIONAL
            test.AuthorizationRequest1.additionalData = "";

            TestVisa.AuthorizationResponse res = services.AuthorizationRequest(test);

            if (res.response.responseCode.Equals("00"))
            {
                operaciones = new Operaciones();       
                string resultado = operaciones.RealizarPago(datos);
                return resultado;
            }
            else{
                return res.response.responseCode;
            }
            

        }


        [WebMethod]
        [System.Web.Services.Protocols.SoapHeader("SoapHeader")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]//Specify return format.
        public string ConsultaSaldo(DatosConsulta datos)
        {

            if (SoapHeader == null)
                return "Implementar metodo AutenticarUsuario de primero";

            if (!SoapHeader.UsuarioCorrecto(SoapHeader))
                return "Implementar metodo AutenticarUsuario de primero";

            operaciones = new Operaciones();
            string data = operaciones.RealizarConsulta(datos);
            return "{ data:" + data + "}";
        }
        

    }
}
