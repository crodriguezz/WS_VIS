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

            try
            {
                switch (tipo.OPERACION)
                {
                    case "TIP_OPER":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), consulta.TIP_OPER);
                    case "NIS":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), consulta.NIS);
                    case "USUARIO":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), consulta.USUARIO);
                    case "ESTACION":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), consulta.ESTACION);
                    case "FECHA":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), consulta.FECHA);
                    case "HORA":
                        return DateTime.Now.ToString("HHmmss");

                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
            
            return valor;
        }

        public DatosRespuestaConsulta TipoRespuestaConsulta(List<ParametrosConsulta> tipo, string trama)
        {
            DatosRespuestaConsulta dato = new DatosRespuestaConsulta();
            string valor = "";
            try
            {

                for (int i = 0; i < tipo.Count; i++)
                {
                    switch (tipo[i].OPERACION)
                    {
                        case "STATUS":
                            valor = trama.Substring(0, Int32.Parse(tipo[i].LONGITUD));
                            trama = trama.Substring(Int32.Parse(tipo[i].LONGITUD));
                            dato.STATUS = valor;
                            break;
                        case "NOM_TIT_CONT":
                            valor = trama.Substring(0, Int32.Parse(tipo[i].LONGITUD));
                            trama = trama.Substring(Int32.Parse(tipo[i].LONGITUD));
                            dato.NOM_TIT_CONT = valor;
                            break;
                        case "DIR_SUMINISTRO":
                            valor = trama.Substring(0, Int32.Parse(tipo[i].LONGITUD));
                            trama = trama.Substring(Int32.Parse(tipo[i].LONGITUD));
                            dato.DIR_SUMINISTRO = valor;
                            break;
                        case "USUARIO":
                            valor = trama.Substring(0, Int32.Parse(tipo[i].LONGITUD));
                            trama = trama.Substring(Int32.Parse(tipo[i].LONGITUD));
                            dato.USUARIO = valor;
                            break;
                        case "ESTACION":
                            valor = trama.Substring(0, Int32.Parse(tipo[i].LONGITUD));
                            trama = trama.Substring(Int32.Parse(tipo[i].LONGITUD));
                            dato.ESTACION = valor;
                            break;
                        case "DEUDA":
                            valor = trama.Substring(0, Int32.Parse(tipo[i].LONGITUD));
                            trama = trama.Substring(Int32.Parse(tipo[i].LONGITUD));
                            string decimales = valor.Substring(valor.Length - 2);
                            string entero = Int32.Parse(valor.Substring(0, valor.Length - 2)).ToString();
                            dato.DEUDA = entero + "." + decimales;
                            break;
                        case "NIS":
                            valor = trama.Substring(0, Int32.Parse(tipo[i].LONGITUD));
                            trama = trama.Substring(Int32.Parse(tipo[i].LONGITUD));
                            dato.NIS = valor;
                            break;
                        case "TIP_OPER":
                            valor = trama.Substring(0, Int32.Parse(tipo[i].LONGITUD));
                            trama = trama.Substring(Int32.Parse(tipo[i].LONGITUD));
                            dato.TIP_OPER = valor;
                            break;
                    }
                }

            }
            catch (Exception)
            {

                dato = new DatosRespuestaConsulta();
                dato.TIP_OPER = Properties.Resources.CodErrorConexion;
            }
            
            return dato;
        }

        public string TipoDatoPago(ParametrosConsulta tipo, DatosPago pago)
        {
            string valor = "";
            try
            {

                switch (tipo.OPERACION)
                {
                    case "FILLER":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), "");
                    case "TIP_OPER":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.TIP_OPER);
                    case "NIR/NIS":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.NIS_NIR);
                    case "EMPRESA":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.EMPRESA);
                    case "TIPO_PAGO":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.TIPO_PAGO);
                    case "CODIGO_BANCO":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.CODIGO_BANCO);
                    case "AGENCIA":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.AGENCIA);
                    case "CAJERO":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.CAJERO);
                    case "FECHA":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.FECHA);
                    case "HORA":
                        return DateTime.Now.ToString("HHmmss");
                    case "EFECTIVO":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.EFECTIVO);
                    case "CHEQUES_BI":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.CHEQUES_BI);
                    case "NO_CHEQUE":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.NO_CHEQUE);
                    case "TOTAL_OPER":
                        return ValidarTamaño(Int32.Parse(tipo.LONGITUD), pago.TOTAL_OPER);

                }
            }
            catch (Exception)
            {
                return string.Empty;
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
                    data += " ";
                }
                valor += data;
                return valor;
            }
        }

    }
}