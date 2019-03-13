using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSPagoServicio.Clases
{
    public class Operaciones
    {
        public string RealizarConsulta(DatosConsulta datos)
        {
            string trama = CrearTramaConsulta(datos);
            return trama;
        }
        private string CrearTramaConsulta(DatosConsulta datos)
        {
            OracleClass oracle = new OracleClass();
            Utilidad utilidad = new Utilidad();
            List<ParametrosConsulta> parametros = oracle.GetParametros(datos.BANCO,datos.TIP_OPER);
            string trama = string.Empty;

            int inicio_trama = oracle.InicioTrama(datos.BANCO);
            if (inicio_trama ==-1)
            {
                inicio_trama = 0;
            }
            for (int i = 0; i < inicio_trama; i++)
            {
                trama += "0";
            }
            for (int i = 0; i < parametros.Count; i++)
            {
                trama += utilidad.TipoDatoConsulta(parametros[i],datos);
            }

            return trama;
        }
        public DatosRespuestaConsulta InterpretarTramaConsulta(string trama, string banco, string operacion)
        {
            OracleClass oracle = new OracleClass();
            Utilidad utilidad = new Utilidad();
            int inicio_trama = oracle.InicioTrama(banco);
            if (inicio_trama == -1)
            {
                inicio_trama = 0;
            }
            else
            {
                trama = trama.Substring(inicio_trama);
            }
            List<ParametrosConsulta> parametros = oracle.GetParametros(banco, operacion);
            DatosRespuestaConsulta datos = utilidad.TipoRespuestaConsulta(parametros,trama);
            return datos;
        }



        public string RealizarPago(DatosPago datos)
        {
            string trama = CrearTramaPago(datos);
            RealizarReversion(datos);
            InterpretarTramaPago(trama, datos.MQ_BANCO, "553");
            return trama;
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