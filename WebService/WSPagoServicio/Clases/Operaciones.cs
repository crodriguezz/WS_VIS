using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WSPagoServicio.MQ;

namespace WSPagoServicio.Clases
{
    public class Operaciones
    {
        public DatosRespuestaConsulta RealizarConsulta(DatosConsulta datos)
        {

            DatosRespuestaConsulta respuesta_datos = new DatosRespuestaConsulta();

            //creacion trama
            string trama = CrearTramaConsulta(datos);

            //Ingreso trama
            Metodos MQ_metodos = new Metodos();
            string messageID = datos.FECHA + DateTime.Now.ToString("HHmmss") + datos.NIS;
            bool response = MQ_metodos.PutMessages(trama, messageID);
            
            if (response ==false)
            {
                respuesta_datos.TIP_OPER = Properties.Resources.CodErrorConexion;
                return respuesta_datos;
            }
            byte[] MessageId = Encoding.ASCII.GetBytes(messageID);
            string putID = BitConverter.ToString(MessageId).Replace("-", string.Empty);
            if (putID.Length > 45)
            {
                putID = putID.Substring(0, putID.Length - 3);
            }

            //GET DE TRAMA
            string get_trama= BuscarRespuesta(putID);

            
            //string get_trama = MQ_metodos.GetMessages();
            if (get_trama.Equals(string.Empty))
            {
                respuesta_datos.TIP_OPER = Properties.Resources.CodErrorConexion;
                respuesta_datos.STATUS = "No fue posible realizar la consulta, intente nuevamente.";
                return respuesta_datos;
            }

            respuesta_datos = InterpretarTramaConsulta(get_trama, datos.BANCO, Properties.Resources.CodConsulta);

            if (respuesta_datos.TIP_OPER.Equals(Properties.Resources.CodErrorConexion))
            {
                respuesta_datos.STATUS = "No fue posible realizar la consulta, intente nuevamente.";
            }
            return respuesta_datos;
        }
        private string CrearTramaConsulta(DatosConsulta datos)
        {
            OracleClass oracle = new OracleClass();
            Utilidad utilidad = new Utilidad();
            List<ParametrosConsulta> parametros = oracle.GetParametros(datos.BANCO,datos.TIP_OPER);
            string trama = string.Empty;

            if (parametros != null)
            {
                int inicio_trama = oracle.InicioTrama(datos.BANCO);                
                for (int i = 0; i < inicio_trama; i++)
                {
                    trama += "0";
                }                
                
                for (int i = 0; i < parametros.Count; i++)
                {
                    trama += utilidad.TipoDatoConsulta(parametros[i], datos);
                }
            }

            return trama;
        }
        public DatosRespuestaConsulta InterpretarTramaConsulta(string trama, string banco, string operacion)
        {
            OracleClass oracle = new OracleClass();
            Utilidad utilidad = new Utilidad();
            //int inicio_trama = oracle.InicioTrama(banco);
            //if (inicio_trama != -1)
            //{
            //    trama = trama.Substring(inicio_trama);
            //}
            List<ParametrosConsulta> parametros = oracle.GetParametros(banco, operacion);

            DatosRespuestaConsulta respuesta = new DatosRespuestaConsulta { TIP_OPER = Properties.Resources.CodErrorConexion};
            if (parametros != null)
            {
                respuesta = utilidad.TipoRespuestaConsulta(parametros, trama);
            }
            return respuesta;
        }
        public string BuscarRespuesta(string mensajeID)
        {
            OracleClass oracle = new OracleClass();

            return oracle.ObtenerRespuesta(mensajeID);
        }



        public DatosRespuestaPago RealizarPago(DatosPago datos, RespuestaVisa datos_visa, string messageID)
        {
            DatosRespuestaPago respuesta_datos = new DatosRespuestaPago();
            string trama = CrearTramaPago(datos);

            Metodos MQ_metodos = new Metodos();
            bool response = MQ_metodos.PutMessages(trama, messageID);

            if (response == false)
            {
                //Reversion
                respuesta_datos = ReversionVisa(datos);
                respuesta_datos.TIP_OPER = Properties.Resources.CodErrorConexion;
                respuesta_datos.STATUS = "No fue posible realizar el pago.";
                OracleClass oracle = new OracleClass();
                oracle.RegistrarEvento(datos, datos_visa, "REVERSION", "");
                return respuesta_datos;
            }
            else
            {
                respuesta_datos.TIP_OPER = Properties.Resources.CodPagoExitoso;
                respuesta_datos.STATUS = "Su pago sera procesado en las proximas 24 hrs.";
                OracleClass oracle = new OracleClass();
                oracle.RegistrarEvento(datos, datos_visa, "PAGO ENERGUATE", messageID);
                return respuesta_datos;
            }
        }
        /// <summary>
        /// Metodos Visa
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public DatosRespuestaPago PagarVisa(DatosPago datos)
        {
            WSVisa.PaymentGWServices services = new WSVisa.PaymentGWServices();
            WSVisa.AuthorizationRequest metodosWS = new WSVisa.AuthorizationRequest
            {
                AuthorizationRequest1 = new WSVisa.Request()
            };

            metodosWS.AuthorizationRequest1.posEntryMode = Properties.Resources.VisaposEntryMode; //012 - TARJETA  022- LECTOR DE BANDA

            metodosWS.AuthorizationRequest1.pan = datos.TARJETA; //NO TARJETA
            metodosWS.AuthorizationRequest1.expdate = datos.FECHA_EXPIRACION; // FECHA EXPIRACION YYMM
            metodosWS.AuthorizationRequest1.amount = datos.MONTO; //MONTO DE CONSUMO
            //OPCIONAL
            //metodosWS.AuthorizationRequest1.track2Data = ""; //022 SI ES LECTURA DE BANDA
            metodosWS.AuthorizationRequest1.cvv2 = datos.CVV2;   // CODIGO DE SEGURIDAD

            metodosWS.AuthorizationRequest1.paymentgwIP = Properties.Resources.VisapaymentgwIP;
            //metodosWS.AuthorizationRequest1.shopperIP = "192.168.100.4";
            //metodosWS.AuthorizationRequest1.merchantServerIP = "";

            metodosWS.AuthorizationRequest1.merchantUser = Properties.Resources.UserVisa; //USUARIO
            metodosWS.AuthorizationRequest1.merchantPasswd = Properties.Resources.UserVisaPass; //CONTRASEÑA
            metodosWS.AuthorizationRequest1.terminalId = Properties.Resources.VisaTerminalID; //ID TERMINAL
            metodosWS.AuthorizationRequest1.merchant = Properties.Resources.VisaMerchant; //AFILICION

            metodosWS.AuthorizationRequest1.messageType = Properties.Resources.VisaMessageTypePago; // 0200 VENTA 0400 REVERSA  0202 ANULACION
            metodosWS.AuthorizationRequest1.auditNumber = Properties.Resources.VisaAuditNumber; //NO. TRANSACCION
            //OPCIONAL
            metodosWS.AuthorizationRequest1.additionalData = "";

            WSVisa.AuthorizationResponse respuestaWS = services.AuthorizationRequest(metodosWS);


            DatosRespuestaPago respuesta_datos = new DatosRespuestaPago
            {
                TIP_OPER = Properties.Resources.CodErrorConexion,
                STATUS = "No se puedo efectuar el pago con Visa"
            };

            if (respuestaWS.response.responseCode.Equals("00"))
            {
                RespuestaVisa resp_visa = new RespuestaVisa
                {
                    MessageType = respuestaWS.response.messageType,
                    AuditNumber = respuestaWS.response.auditNumber,
                    AuthorizationNumber = respuestaWS.response.authorizationNumber,
                    ResponseCode = respuestaWS.response.responseCode,
                    ReferenceNumber = respuestaWS.response.referenceNumber
                };
                string messageID = datos.FECHA + DateTime.Now.ToString("HHmm") + resp_visa.ReferenceNumber;
                respuesta_datos = RealizarPago(datos, resp_visa,messageID);


                return respuesta_datos;
            }

            return respuesta_datos;
        }

        public DatosRespuestaPago ReversionVisa(DatosPago datos)
        {
            WSVisa.PaymentGWServices services = new WSVisa.PaymentGWServices();
            WSVisa.AuthorizationRequest metodosWS = new WSVisa.AuthorizationRequest
            {
                AuthorizationRequest1 = new WSVisa.Request()
            };

            metodosWS.AuthorizationRequest1.posEntryMode = Properties.Resources.VisaposEntryMode; //012 - TARJETA  022- LECTOR DE BANDA

            metodosWS.AuthorizationRequest1.pan = datos.TARJETA; //NO TARJETA
            metodosWS.AuthorizationRequest1.expdate = datos.FECHA_EXPIRACION; // FECHA EXPIRACION YYMM
            metodosWS.AuthorizationRequest1.amount = datos.MONTO; //MONTO DE CONSUMO
            //OPCIONAL
            //metodosWS.AuthorizationRequest1.track2Data = ""; //022 SI ES LECTURA DE BANDA
            metodosWS.AuthorizationRequest1.cvv2 = datos.CVV2;   // CODIGO DE SEGURIDAD

            metodosWS.AuthorizationRequest1.paymentgwIP = Properties.Resources.VisapaymentgwIP;
            //metodosWS.AuthorizationRequest1.shopperIP = "192.168.100.4";
            //metodosWS.AuthorizationRequest1.merchantServerIP = "";

            metodosWS.AuthorizationRequest1.merchantUser = Properties.Resources.UserVisa; //USUARIO
            metodosWS.AuthorizationRequest1.merchantPasswd = Properties.Resources.UserVisaPass; //CONTRASEÑA
            metodosWS.AuthorizationRequest1.terminalId = Properties.Resources.VisaTerminalID; //ID TERMINAL
            metodosWS.AuthorizationRequest1.merchant = Properties.Resources.VisaMerchant; //AFILICION

            metodosWS.AuthorizationRequest1.messageType = Properties.Resources.VisaMessageTypeReversion; // 0200 VENTA 0400 REVERSA  0202 ANULACION
            metodosWS.AuthorizationRequest1.auditNumber = Properties.Resources.VisaAuditNumber; //NO. TRANSACCION
            //OPCIONAL
            metodosWS.AuthorizationRequest1.additionalData = "";


            WSVisa.AuthorizationResponse respuestaWS = services.AuthorizationRequest(metodosWS);
            DatosRespuestaPago respuesta_datos = new DatosRespuestaPago
            {
                TIP_OPER = Properties.Resources.CodErrorConexion,
                STATUS = "No se puedo efectuar el pago con Visa"
            };

            if (respuestaWS.response.responseCode.Equals("00"))
            {
                //ENVIAR A LOG DE OPERACIONES
                return respuesta_datos;
            }
            else
            {
                //
            }

            return respuesta_datos;
        }

        private string CrearTramaPago(DatosPago datos)
        {
            OracleClass oracle = new OracleClass();
            Utilidad utilidad = new Utilidad();
            List<ParametrosConsulta> parametros = oracle.GetParametros(datos.MQ_BANCO, datos.TIP_OPER);
            string trama = string.Empty;
            if (parametros != null)
            {
                int inicio_trama = oracle.InicioTrama(datos.MQ_BANCO);
                if (inicio_trama == -1)
                {
                    inicio_trama = 0;
                }
                for (int i = 0; i < inicio_trama; i++)
                {
                    trama += "0";
                }
                for (int i = 0; i < parametros.Count; i++)
                {
                    trama += utilidad.TipoDatoPago(parametros[i], datos);
                }
            }
            return trama;
        }

    }
}