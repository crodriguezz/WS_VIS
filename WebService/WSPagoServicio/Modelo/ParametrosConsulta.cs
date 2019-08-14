namespace WSPagoServicio.Clases
{
    /// <summary>
    /// Parametros para obtener la estructura de tramas.
    /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
    ///</summary>
    public class ParametrosMQConsulta
    {
        #region Public Constructors

        public ParametrosMQConsulta()
        {
            OPERACION = "";
            LONGITUD = "";
        }

        #endregion Public Constructors

        #region Public Properties

        public string OPERACION { get; set; }

        public string LONGITUD { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    /// Parametros de Consulta Web Service
    /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
    ///</summary>
    public class DatosConsulta
    {
        #region Public Constructors

        public DatosConsulta()
        {
            TIP_OPER = "";
            NIS = "";
            USUARIO = "";
            ESTACION = "";
            FECHA = "";
            HORA = "";
            BANCO = "";
        }

        #endregion Public Constructors

        #region Public Properties

        //MQ
        public string TIP_OPER { get; set; }

        public string NIS { get; set; }

        public string USUARIO { get; set; }

        public string ESTACION { get; set; }

        public string FECHA { get; set; }

        public string HORA { get; set; }

        //WS
        public string BANCO { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    /// Parametros de Respuesta para Metodo de Consulta
    /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
    ///</summary>
    public class DatosRespuestaConsulta
    {
        #region Public Properties

        //MQ
        public string TIP_OPER { get; set; }

        public string NIS { get; set; }

        public string NOM_TIT_CONT { get; set; }

        public string DIR_SUMINISTRO { get; set; }

        public string USUARIO { get; set; }

        public string ESTACION { get; set; }

        public string DEUDA { get; set; }

        public string STATUS { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    ///´Parametros para el metodo de Pago
    /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
    ///</summary>
    public class DatosPago
    {
        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Properties

        //MQ
        public string TIP_OPER { get; set; }

        public string NIS_NIR { get; set; }

        public string EMPRESA { get; set; }

        public string FECHA { get; set; }

        //WS
        public string MQ_BANCO { get; set; }

        public string TARJETA { get; set; }

        public string CVV2 { get; set; }

        public string FECHA_EXPIRACION { get; set; }

        public string MONTO { get; set; }

        //MQ
        public string FILLER { get; set; }

        public string TIPO_PAGO { get; set; }

        public string CODIGO_BANCO { get; set; }

        public string AGENCIA { get; set; }

        public string CAJERO { get; set; }

        public string HORA { get; set; }

        public string EFECTIVO { get; set; }

        public string CHEQUES_BI { get; set; }

        public string NO_CHEQUE { get; set; }

        public string TOTAL_OPER { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    /// Parametros de Respuesta para el Metodo de Pago
    /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
    ///</summary>
    public class DatosRespuestaPago
    {
        #region Public Properties

        public string TIP_OPER { get; set; }

        public string NOM_TIT_CONT { get; set; }

        public string STATUS { get; set; }

        public string ID_Terminal { get; set; }

        public string N_Autorizacion { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    /// Parametros para Reversión de Pago
    /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
    ///</summary>
    public class DatosReversion
    {
        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Properties

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

        #endregion Public Properties
    }

    /// <summary>
    /// Parametros de Respuesta de Visa
    /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
    ///</summary>
    public class RespuestaVisa
    {
        #region Public Properties

        public string AuditNumber { get; set; }

        public string ReferenceNumber { get; set; }

        public string AuthorizationNumber { get; set; }

        public string ResponseCode { get; set; }

        public string MessageType { get; set; }

        #endregion Public Properties
    }

    public class RespuestaAutenticacion
    {
        #region Public Properties

        public string TIP_OPER { get; set; }

        public string TOKEN { get; set; }

        public string STATUS { get; set; }

        #endregion Public Properties
    }
}
