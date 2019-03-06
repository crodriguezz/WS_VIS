using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSPagoServicio.Clases
{
    public class Operaciones
    {
        /// <summary>
        /// PERMITE REALIZAR CONSULTA
        /// </summary>
        /// <param name="nis"></param>
        /// <param name="empresa"></param>
        /// <param name="cod_banco"></param>
        /// <returns></returns>
        public string RealizarConsulta(DatosConsulta datos)
        {
            string trama = CrearTramaConsulta(datos);           
            return trama;
        }

        private string CrearTramaConsulta(DatosConsulta datos)
        {
            OracleClass oracle = new OracleClass();
            Utilidad utilidad = new Utilidad();
            List<ParametrosConsulta> parametros = oracle.GetParametros(datos.BANCO,datos.OPERACION);
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

        public string RealizarPago(DatosPago datos)
        {
            string trama = CrearTramaPago(datos);
            return trama;
        }
        private string CrearTramaPago(DatosPago datos)
        {
            OracleClass oracle = new OracleClass();
            Utilidad utilidad = new Utilidad();
            List<ParametrosConsulta> parametros = oracle.GetParametros(datos.BANCO, datos.OPERACION);
            string trama = string.Empty;

            int inicio_trama = oracle.InicioTrama(datos.BANCO);
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


        public string RealizarReversion(DatosPago datos)
        {
            string trama = CrearTramaReversion(datos);
            return trama;
        }
        private string CrearTramaReversion(DatosPago datos)
        {
            OracleClass oracle = new OracleClass();
            Utilidad utilidad = new Utilidad();
            List<ParametrosConsulta> parametros = oracle.GetParametros(datos.BANCO, datos.OPERACION);
            string trama = string.Empty;

            int inicio_trama = oracle.InicioTrama(datos.BANCO);
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
    }
}