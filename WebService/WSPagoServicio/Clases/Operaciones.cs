using System;
using System.Collections.Generic;
using System.Linq;
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
            string messageID = datos.FECHA + datos.HORA + datos.NIS;
            bool response = MQ_metodos.PutMessages(trama, messageID);

            if (response ==false)
            {
                respuesta_datos.TIP_OPER = Properties.Resources.CodErrorConexion;
                return respuesta_datos;
            }

            //GET DE TRAMA
            //BuscarRespuesta(messageID);
            string get_trama = MQ_metodos.GetMessages();
            if (get_trama.Length==1)
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
            int inicio_trama = oracle.InicioTrama(banco);
            if (inicio_trama != -1)
            {
                trama = trama.Substring(inicio_trama);
            }
            List<ParametrosConsulta> parametros = oracle.GetParametros(banco, operacion);

            DatosRespuestaConsulta respuesta = new DatosRespuestaConsulta { TIP_OPER = Properties.Resources.CodErrorConexion};
            if (parametros != null)
            {
                respuesta = utilidad.TipoRespuestaConsulta(parametros, trama);
            }
            return respuesta;
        }

        public DatosRespuestaPago RealizarPago(DatosPago datos, RespuestaVisa datos_visa)
        {
            DatosRespuestaPago respuesta_datos = new DatosRespuestaPago();
            string trama = CrearTramaPago(datos);

            Metodos MQ_metodos = new Metodos();
            string messageID = datos.FECHA + datos.HORA + datos_visa.ReferenceNumber;
            bool response = MQ_metodos.PutMessages(trama, messageID);

            if (response == false)
            {
                //Reversion
                respuesta_datos = ReversionVisa(datos_visa);
                respuesta_datos.TIP_OPER = Properties.Resources.CodErrorConexion;
                respuesta_datos.STATUS = "No fue posible realizar el pago.";
                return respuesta_datos;
            }
            else
            {
                respuesta_datos.TIP_OPER = Properties.Resources.CodPagoExitoso;
                respuesta_datos.STATUS = "Su pago sera procesado en las proximas 24 hrs.";
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

            metodosWS.AuthorizationRequest1.posEntryMode = "012"; //012 - TARJETA  022- LECTOR DE BANDA

            metodosWS.AuthorizationRequest1.pan = datos.TARJETA; //NO TARJETA
            metodosWS.AuthorizationRequest1.expdate = datos.FECHA_EXPIRACION; // FECHA EXPIRACION YYMM
            metodosWS.AuthorizationRequest1.amount = datos.MONTO; //MONTO DE CONSUMO
            //OPCIONAL
            //metodosWS.AuthorizationRequest1.track2Data = ""; //022 SI ES LECTURA DE BANDA
            metodosWS.AuthorizationRequest1.cvv2 = datos.CVV2;   // CODIGO DE SEGURIDAD

            metodosWS.AuthorizationRequest1.paymentgwIP = "190.149.69.135";
            //metodosWS.AuthorizationRequest1.shopperIP = "192.168.100.4";
            //metodosWS.AuthorizationRequest1.merchantServerIP = "";

            metodosWS.AuthorizationRequest1.merchantUser = "76B925EF7BEC821780B4B21479CE6482EA415896CF43006050B1DAD101669921"; //USUARIO
            metodosWS.AuthorizationRequest1.merchantPasswd = "DD1791DB5B28DDE6FBC2B9951DFED4D97B82EFD622B411F1FC16B88B052232C7"; //CONTRASEÑA
            metodosWS.AuthorizationRequest1.terminalId = "77788881"; //ID TERMINAL
            metodosWS.AuthorizationRequest1.merchant = "00575123"; //AFILICION

            metodosWS.AuthorizationRequest1.messageType = "0200"; // 0200 VENTA 0400 REVERSA  0202 ANULACION
            metodosWS.AuthorizationRequest1.auditNumber = "090249"; //NO. TRANSACCION
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
                respuesta_datos = RealizarPago(datos, resp_visa);
                return respuesta_datos;
            }

            return respuesta_datos;
        }

        public DatosRespuestaPago ReversionVisa(RespuestaVisa datos)
        {
            WSVisa.PaymentGWServices services = new WSVisa.PaymentGWServices();
            WSVisa.AuthorizationRequest metodosWS = new WSVisa.AuthorizationRequest
            {
                AuthorizationRequest1 = new WSVisa.Request()
            };

            metodosWS.AuthorizationRequest1.posEntryMode = "012"; //012 - TARJETA  022- LECTOR DE BANDA
            metodosWS.AuthorizationRequest1.paymentgwIP = "190.149.69.135";
            //metodosWS.AuthorizationRequest1.shopperIP = "192.168.100.4";
            //metodosWS.AuthorizationRequest1.merchantServerIP = "";
            metodosWS.AuthorizationRequest1.merchantUser = "76B925EF7BEC821780B4B21479CE6482EA415896CF43006050B1DAD101669921"; //USUARIO
            metodosWS.AuthorizationRequest1.merchantPasswd = "DD1791DB5B28DDE6FBC2B9951DFED4D97B82EFD622B411F1FC16B88B052232C7"; //CONTRASEÑA
            metodosWS.AuthorizationRequest1.terminalId = "77788881"; //ID TERMINAL
            metodosWS.AuthorizationRequest1.merchant = "00575123"; //AFILICION
            metodosWS.AuthorizationRequest1.messageType = "0202"; // 0200 VENTA 0400 REVERSA  0202 ANULACION
            metodosWS.AuthorizationRequest1.auditNumber = datos.AuthorizationNumber; //NO. TRANSACCION


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
                //ENVIAR A UN LOG
            }

            return respuesta_datos;
        }





        private string CrearTramaPago(DatosPago datos)
        {
            OracleClass oracle = new OracleClass();
            Utilidad utilidad = new Utilidad();
            List<ParametrosConsulta> parametros = oracle.GetParametros(datos.MQ_BANCO, datos.TIP_OPER);
            string trama = string.Empty;

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

            return trama;
        }
        private void InterpretarTramaPago(string trama, string banco, string operacion)
        {
            OracleClass oracle = new OracleClass();
            Utilidad utilidad = new Utilidad();
            List<ParametrosConsulta> parametros = oracle.GetParametros(banco, operacion);
            DatosRespuestaPago datos = utilidad.TipoRespuestaPago(parametros, trama);


        }

        public string RealizarReversion(DatosPago datos)
        {
            DatosReversion rev = new DatosReversion
            {
                TIP_OPER = "554",
                MONTO = datos.MONTO,
                AGENCIA = datos.AGENCIA,
                MQ_BANCO = datos.MQ_BANCO,
                CAJERO = datos.CAJERO,
                CHEQUES_BI = datos.CHEQUES_BI,
                CODIGO_BANCO = datos.CODIGO_BANCO,
                CVV2 = datos.CVV2,
                EFECTIVO = datos.EFECTIVO,
                EMPRESA = datos.EMPRESA,
                FECHA_EXPIRACION = datos.FECHA_EXPIRACION,
                HORA_REV = DateTime.Now.ToString("HHmmss"),
                FECHA_REV = DateTime.Now.ToString("yyyyMMdd"),
                NIS_NIR = datos.NIS_NIR,
                NO_CHEQUE = datos.NO_CHEQUE,
                TARJETA = datos.TARJETA,
                TIPO_PAGO = datos.TIPO_PAGO,
                TOTAL_OPER = datos.TOTAL_OPER
            };
            string trama = CrearTramaReversion(rev);           
            return trama;
        }
        private string CrearTramaReversion(DatosReversion datos)
        {
            OracleClass oracle = new OracleClass();
            Utilidad utilidad = new Utilidad();
            List<ParametrosConsulta> parametros = oracle.GetParametros(datos.MQ_BANCO, datos.TIP_OPER);
            string trama = string.Empty;

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
                trama += utilidad.TipoDatoReversion(parametros[i], datos);
            }

            return trama;
        }
        private void InterpretarTramaReversion(string trama, string banco, string operacion)
        {
            OracleClass oracle = new OracleClass();
            Utilidad utilidad = new Utilidad();
            List<ParametrosConsulta> parametros = oracle.GetParametros(banco, operacion);
            DatosRespuestaReversion datos = utilidad.TipoRespuestaReversion(parametros, trama);


        }
    }
}