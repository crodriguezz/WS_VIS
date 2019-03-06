using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSPagoServicio.Clases
{
    public class DatosConsulta
    {
        public string NIS { get; set; }
        public string BANCO { get; set; }
        public string OPERACION { get; set; }
        public string EMPRESA { get; set; }
    }

    public class DatosPago
    {

        public string NIS { get; set; }
        public string BANCO { get; set; }
        public string OPERACION { get; set; }
        public string EMPRESA { get; set; }
        public string TIPO_PAGO { get; set; }
        public string CODIGO_BANCO { get; set; }
        public string TOTAL { get; set; }

        public string TARJETA { get; set; }
        public string CVV2 { get; set; }
        public string FECHA_EXPIRACION { get; set; }
        public string MONTO { get; set; }

    }

    public class DatosReversion
    {

        public string OPERACION { get; set; }
        public string NIS { get; set; }
        public string EMPRESA { get; set; }
        public string TIPO_PAGO { get; set; }
        public string BANCO { get; set; }
        public string CODIGO_BANCO { get; set; }
        public string AGENCIA { get; set; }
        public string CAJERO { get; set; }
        public string EFECTIVO { get; set; }
        public string CHEQUES_BI { get; set; }
        public string NO_CHEQUE { get; set; }
        public string TOTAL { get; set; }
        public string FECHA_REV { get; set; }
        public string HORA_REV { get; set; }

        public string TARJETA { get; set; }
        public string CVV2 { get; set; }
        public string FECHA_EXPIRACION { get; set; }
        public string MONTO { get; set; }

    }

    public class ParametrosConsulta
    {
        public string OPERACION { get; set; }
        public string LONGITUD { get; set; }
    }
}