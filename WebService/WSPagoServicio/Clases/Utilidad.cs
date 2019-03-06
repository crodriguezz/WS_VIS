using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSPagoServicio.Clases
{
    public class Utilidad
    {
        public string TipoDatoConsulta(ParametrosConsulta tipo, DatosConsulta consulta)
        {
            string valor = "";

            switch (tipo.OPERACION)
            {
                case "FECHA":
                    return DateTime.Now.ToString("MMddyyyy");
                case "HORA":
                    return DateTime.Now.ToString("HHmmss");
                case "NIS":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), consulta.NIS);
                case "TIP_OPER":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), consulta.OPERACION);

            }
            return valor;
        }

        public string TipoDatoPago(ParametrosConsulta tipo, DatosPago pago)
        {
            string valor = "";

            switch (tipo.OPERACION)
            {
                case "FECHA":
                    return DateTime.Now.ToString("MMddyyyy");
                case "HORA":
                    return DateTime.Now.ToString("HHmmss");
                case "FILLER":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), "");
                case "TIP_OPER":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.OPERACION);
                case "NIR/NIS":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.NIS);
                case "EMPRESA":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.EMPRESA);
                case "TIPO_PAGO":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.TIPO_PAGO);
                case "CODIGO_BANCO":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.CODIGO_BANCO);
                case "TOTAL_OPER":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.TOTAL);

            }
            return valor;
        }


        public string TipoDatoReversion(ParametrosConsulta tipo, DatosReversion reversion)
        {
            string valor = "";

            switch (tipo.OPERACION)
            {
                case "FILLER":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), "");
                case "TIP_OPER":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), reversion.OPERACION);
                case "NIR/NIS":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), reversion.NIS);
                case "EMPRESA":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), reversion.EMPRESA);
                case "TIPO_PAGO":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), reversion.TIPO_PAGO);
                case "CODIGO_BANCO":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), reversion.CODIGO_BANCO);
                case "AGENCIA":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), reversion.AGENCIA);
                case "CAJERO":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), reversion.CAJERO);
                case "EFECTIVO":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), reversion.EFECTIVO);
                case "CHEQUES_BI":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), reversion.CHEQUES_BI);
                case "NO_CHEQUE":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), reversion.NO_CHEQUE);
                case "TOTAL_OPER":
                    return ValidarTamaño(Int32.Parse(tipo.LONGITUD), reversion.TOTAL);
                case "FECHA":
                    return DateTime.Now.ToString("MMddyyyy");
                case "HORA":
                    return DateTime.Now.ToString("HHmmss");
                case "FECHA_REV":
                    return reversion.FECHA_REV;
                case "HORA_REV":
                    return reversion.HORA_REV;

            }
            return valor;
        }

        public string ValidarTamaño(int tamaño, string valor)
        {
            if (valor.Length == tamaño)
            {
                return valor;
            }
            else
            {
                string data = "";
                for (int i = valor.Length; i < tamaño; i++)
                {
                    data += "0";
                }
                data += valor;
                return data;
            }
        }
    }
}