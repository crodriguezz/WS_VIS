using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSPagoServicio.Clases
{
    public class ParametrosConsulta
    {
        public string OPERACION { get; set; }
        public string LONGITUD { get; set; }

        public ParametrosConsulta()
        {
            OPERACION = "";
            LONGITUD = "";
        }
    }

    public class DatosConsulta
    {

        //MQ
        public string TIP_OPER { get; set; }
        public string NIS { get; set; }
        public string USUARIO { get; set; }
        public string ESTACION { get; set; }
        public string FECHA { get; set; }
        public string HORA { get; set; }
        //WS
        public string BANCO { get; set; }
        public string EMPRESA { get; set; }

        public DatosConsulta()
        {
            TIP_OPER = "";
            NIS = "";
            USUARIO = "";
            ESTACION = "";
            FECHA = "";
            HORA = "";
            BANCO = "";
            EMPRESA = "";
        }
    }
    public class DatosRespuestaConsulta
    {
        //MQ
        public string TIP_OPER  { get; set; }
        public string NIS { get; set; }
        public string NOM_TIT_CONT { get; set; }
        public string DIR_SUMINISTRO { get; set; }
        public string USUARIO { get; set; }
        public string ESTACION { get; set; }
        public string DEUDA { get; set; }
        public string STATUS { get; set; }
    }

    public class DatosPago
    {
        //MQ
        public string FILLER { get; set; }
        public string TIP_OPER { get; set; }
        public string NIS_NIR { get; set; }
        public string EMPRESA { get; set; }
        public string TIPO_PAGO { get; set; }
        public string CODIGO_BANCO { get; set; }
        public string AGENCIA { get; set; }
        public string CAJERO { get; set; }
        public string FECHA { get; set; }
        public string HORA { get; set; }
        public string EFECTIVO { get; set; }
        public string CHEQUES_BI { get; set; }
        public string NO_CHEQUE { get; set; }
        public string TOTAL_OPER { get; set; }

        //WS
        public string MQ_BANCO { get; set; }
        public string TARJETA { get; set; }
        public string CVV2 { get; set; }
        public string FECHA_EXPIRACION { get; set; }
        public string MONTO { get; set; }

        public DatosPago()
        {
            FILLER = "";
            TIP_OPER = "";
            NIS_NIR = "";
            EMPRESA = "";
            TIPO_PAGO = "";
            CODIGO_BANCO = "";
            AGENCIA = "";
            CAJERO = "";
            FECHA = "";
            HORA = "";
            EFECTIVO = "";
            CHEQUES_BI = "";
            NO_CHEQUE = "";
            TOTAL_OPER = "";
            MQ_BANCO = "";
            TARJETA = "";
            CVV2 = "";
            FECHA_EXPIRACION = "";
            MONTO = "";
        }
    }
    public class DatosRespuestaPago
    {

        public string TIP_OPER { get; set; }
        public string NOM_TIT_CONT { get; set; }
        public string STATUS { get; set; }

    }

    public class DatosReversion
    {

        public string FILLER { get; set; }
        public string TIP_OPER { get; set; }
        public string NIS_NIR { get; set; }
        public string EMPRESA { get; set; }
        public string TIPO_PAGO { get; set; }
        public string CODIGO_BANCO { get; set; }
        public string AGENCIA { get; set; }
        public string CAJERO { get; set; }
        public string FECHA { get; set; }
        public string HORA { get; set; }
        public string EFECTIVO { get; set; }
        public string CHEQUES_BI { get; set; }
        public string NO_CHEQUE { get; set; }
        public string TOTAL_OPER { get; set; }
        public string FECHA_REV { get; set; }
        public string HORA_REV { get; set; }

        public string MQ_BANCO { get; set; }
        public string TARJETA { get; set; }
        public string CVV2 { get; set; }
        public string FECHA_EXPIRACION { get; set; }
        public string MONTO { get; set; }

        public DatosReversion()
        {
            FILLER = "";
            TIP_OPER = "";
            NIS_NIR = "";
            EMPRESA = "";
            TIPO_PAGO = "";
            CODIGO_BANCO = "";
            AGENCIA = "";
            CAJERO = "";
            FECHA = "";
            HORA = "";
            EFECTIVO = "";
            CHEQUES_BI = "";
            NO_CHEQUE = "";
            TOTAL_OPER = "";
            FECHA_REV = "";
            HORA_REV = "";
            MQ_BANCO = "";
            TARJETA = "";
            CVV2 = "";
            FECHA_EXPIRACION = "";
            MONTO = "";
        }
    }
    public class DatosRespuestaReversion
    {

        public string TIP_OPER { get; set; }
        public string STATUS { get; set; }

    }

}