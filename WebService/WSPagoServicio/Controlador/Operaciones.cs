using System;
using System.Collections.Generic;
using System.Text;
using WSPagoServicio.DBConnection;
using WSPagoServicio.MQ;

namespace WSPagoServicio.Clases
{
    public class Operaciones
    {
        /// <summary>
        /// Permite Iniciar la consulta de usuario
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="datos">Valores con la info de uusario a consultar</param>
        /// <returns></returns>
        public DatosRespuestaConsulta RealizarConsulta(DatosConsulta datos)
        {
            DatosRespuestaConsulta respuesta_datos = new DatosRespuestaConsulta();
            OracleMethod oracle = new OracleMethod();
            //creacion trama
            string trama = CrearTramaConsulta(datos);
            //Ingreso trama
            Metodos MQ_metodos = new Metodos();
            string messageID = datos.FECHA + DateTime.Now.ToString("HHmmss") + datos.NIS;
            bool response = MQ_metodos.PutMessages(trama, messageID);
            
            if (response ==false)
            {
                respuesta_datos.TIP_OPER = Properties.Resources.CodErrorConexion;
                respuesta_datos.STATUS = "No fue posible realizar la Consulta, intente nuevamente";
                return respuesta_datos;
            }


            byte[] MessageId;
            string putID = string.Empty;
            //Validar crear messageId
            try
            {
                 MessageId = Encoding.ASCII.GetBytes(messageID);
                 putID = BitConverter.ToString(MessageId).Replace("-", string.Empty);
            }
            catch (Exception)
            {
                messageID = datos.FECHA + DateTime.Now.ToString("HHmm") + datos.NIS;
                MessageId = Encoding.ASCII.GetBytes(messageID);
                putID = BitConverter.ToString(MessageId).Replace("-", string.Empty);
            }

            if (putID.Length > 45)
            {
                putID = putID.Substring(0, putID.Length - 3);
            }

            //Buscar Trama
            string get_trama= oracle.ObtenerRespuesta(putID);
            
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
        /// <summary>
        /// Permite crear la trama para poder enviarla al MQ
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        private string CrearTramaConsulta(DatosConsulta datos)
        {
            OracleMethod oracle = new OracleMethod();
            Utilidad utilidad = new Utilidad();
            List<ParametrosMQConsulta> parametros = oracle.GetParametros(datos.BANCO,datos.TIP_OPER);
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
                    trama += utilidad.ValidarDatosParaConsulta(parametros[i], datos);
                }
            }

            return trama;
        }
        /// <summary>
        /// Permite interpretar la trama obteneida del MQ
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="trama"></param>
        /// <param name="banco"></param>
        /// <param name="operacion"></param>
        /// <returns></returns>
        public DatosRespuestaConsulta InterpretarTramaConsulta(string trama, string banco, string operacion)
        {
            OracleMethod oracle = new OracleMethod();
            Utilidad utilidad = new Utilidad();
            List<ParametrosMQConsulta> parametros = oracle.GetParametros(banco, operacion);

            DatosRespuestaConsulta respuesta = new DatosRespuestaConsulta { TIP_OPER = Properties.Resources.CodErrorConexion};
            if (parametros != null)
            {
                respuesta = utilidad.InterpretarRespuestaConsulta(parametros, trama);
            }
            return respuesta;
        }
        /// <summary>
        /// Inicia el Pago con Visa
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="datos"></param>
        /// <param name="datos_visa"></param>
        /// <param name="messageID"></param>
        /// <returns></returns>
        private DatosRespuestaPago RealizarPago(DatosPago datos, RespuestaVisa datos_visa, string messageID)
        {
            DatosRespuestaPago respuesta_datos = new DatosRespuestaPago();
            string trama = CrearTramaPago(datos);

            Metodos MQ_metodos = new Metodos();
            bool response = MQ_metodos.PutMessages(trama, messageID);

            OracleMethod oracle = new OracleMethod();
            if (response == false)
            {
                //Reversion
                respuesta_datos = ReversionVisa(datos);
                respuesta_datos.TIP_OPER = Properties.Resources.CodErrorConexion;
                respuesta_datos.STATUS = "No fue posible realizar el pago.";

                oracle.RegistrarEvento(datos, datos_visa, "REVERSION", "");
                return respuesta_datos;
            }
            else
            {
                respuesta_datos.TIP_OPER = Properties.Resources.CodPagoExitoso;
                respuesta_datos.STATUS = "Su pago sera procesado en las proximas 24 hrs.";

                oracle.RegistrarEvento(datos, datos_visa, "PAGO ENERGUATE", messageID);
                return respuesta_datos;
            }
        }
        /// <summary>
        /// Envia la información a Visa
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="datos"></param>
        /// <param name="tipoPago"></param>
        /// <returns></returns>
        public RespuestaVisa OperacionVisa(DatosPago datos, string tipoPago)
        {
            RespuestaVisa respuestaVisa = new RespuestaVisa();

            WSVisa.PaymentGWServices services = new WSVisa.PaymentGWServices();
            WSVisa.AuthorizationRequest metodosWS = new WSVisa.AuthorizationRequest
            {
                AuthorizationRequest1 = new WSVisa.Request()
            };

            metodosWS.AuthorizationRequest1.posEntryMode = Properties.VisaResource.VisaposEntryMode; //012 - TARJETA  022- LECTOR DE BANDA

            metodosWS.AuthorizationRequest1.pan = datos.TARJETA; //NO TARJETA
            metodosWS.AuthorizationRequest1.expdate = datos.FECHA_EXPIRACION; // FECHA EXPIRACION YYMM
            metodosWS.AuthorizationRequest1.amount = datos.MONTO; //MONTO DE CONSUMO
            //OPCIONAL
            //metodosWS.AuthorizationRequest1.track2Data = ""; //022 SI ES LECTURA DE BANDA
            metodosWS.AuthorizationRequest1.cvv2 = datos.CVV2;   // CODIGO DE SEGURIDAD

            metodosWS.AuthorizationRequest1.paymentgwIP = Properties.VisaResource.VisapaymentgwIP;
            //metodosWS.AuthorizationRequest1.shopperIP = "192.168.100.4";
            //metodosWS.AuthorizationRequest1.merchantServerIP = "";

            metodosWS.AuthorizationRequest1.merchantUser = Properties.VisaResource.UserVisa; //USUARIO
            metodosWS.AuthorizationRequest1.merchantPasswd = Properties.VisaResource.UserVisaPass; //CONTRASEÑA
            metodosWS.AuthorizationRequest1.terminalId = Properties.VisaResource.VisaTerminalID; //ID TERMINAL
            metodosWS.AuthorizationRequest1.merchant = Properties.VisaResource.VisaMerchant; //AFILICION

            metodosWS.AuthorizationRequest1.messageType = tipoPago; // 0200 VENTA 0400 REVERSA  0202 ANULACION
            metodosWS.AuthorizationRequest1.auditNumber = Properties.VisaResource.VisaAuditNumber; //NO. TRANSACCION
            //OPCIONAL
            metodosWS.AuthorizationRequest1.additionalData = "";

            WSVisa.AuthorizationResponse respuestaWS = services.AuthorizationRequest(metodosWS);

           
            respuestaVisa = new RespuestaVisa
            {
                MessageType = respuestaWS.response.messageType,
                AuditNumber = respuestaWS.response.auditNumber,
                AuthorizationNumber = respuestaWS.response.authorizationNumber,
                ResponseCode = respuestaWS.response.responseCode,
                ReferenceNumber = respuestaWS.response.referenceNumber
            };

            return respuestaVisa;
        }
        /// <summary>
        /// Realiza el pago con visa
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public DatosRespuestaPago PagarVisa(DatosPago datos)
        {
            RespuestaVisa resp_visa = OperacionVisa(datos, Properties.VisaResource.VisaMessageTypePago);

            DatosRespuestaPago respuesta_datos = new DatosRespuestaPago
            {
                TIP_OPER = Properties.Resources.CodErrorConexion,
                STATUS = "No se puedo efectuar el pago con Visa"
            };

            if (resp_visa!=null)
            {
                if (resp_visa.ResponseCode.Equals("00"))
                {                
                    string messageID = datos.FECHA + DateTime.Now.ToString("HHmm") + resp_visa.ReferenceNumber;
                    respuesta_datos = RealizarPago(datos, resp_visa,messageID);
                    return respuesta_datos;
                }
            }

            return respuesta_datos;
        }
        /// <summary>
        /// Realiza una reversion al momento de no tener una conexion con Visa
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        private DatosRespuestaPago ReversionVisa(DatosPago datos)
        {
            RespuestaVisa resp_visa = OperacionVisa(datos, Properties.VisaResource.VisaMessageTypeReversion);

            DatosRespuestaPago respuesta_datos = new DatosRespuestaPago
            {
                TIP_OPER = Properties.Resources.CodErrorConexion,
                STATUS = "No se puedo efectuar el pago con Visa"
            };

            if (resp_visa != null)
            {           
                if (resp_visa.ResponseCode.Equals("00"))
                {
                    //ENVIAR A LOG DE OPERACIONES
                    return respuesta_datos;
                }
                else
                {
                    OracleMethod oracle = new OracleMethod();
                    oracle.RegistrarEvento(datos, resp_visa, "ERROR AL REVERSION", "");
                }
            }

            return respuesta_datos;
        }
        /// <summary>
        /// Permite Crear la trama que sera enviada a MQ
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        private string CrearTramaPago(DatosPago datos)
        {
            OracleMethod oracle = new OracleMethod();
            Utilidad utilidad = new Utilidad();
            List<ParametrosMQConsulta> parametros = oracle.GetParametros(datos.MQ_BANCO, datos.TIP_OPER);
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
                    trama += utilidad.ValidarDatosParaPago(parametros[i], datos);
                }
            }
            return trama;
        }

    }
}