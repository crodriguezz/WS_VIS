using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using WSPagoServicio.Clases;

namespace WSPagoServicio.DBConnection
{
    public class OracleMethod
    {
        private OracleClass oracle = new OracleClass();
        /// <summary>
        /// Permite obtener los parametros necesarios para la creacion de tramas para la cola MQ
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="banco">numero de banco</param>
        /// <param name="operacion">tipo de operacion, consulta, pago</param>
        /// <returns></returns>
        public List<ParametrosMQConsulta> GetParametros(string banco, string operacion)
        {
            List<ParametrosMQConsulta> parametros = new List<ParametrosMQConsulta>();

            var conexion = oracle.NuevaConexion();


            string sql = string.Format(Properties.Resources.query_banco_detalle, banco, operacion);
            try
            {

                OracleCommand comando = new OracleCommand(sql, conexion)
                {
                    CommandType = System.Data.CommandType.Text
                };

                //OracleDataReader reader = comando.ExecuteReader();
                OracleDataAdapter sqlDa = new OracleDataAdapter
                {
                    SelectCommand = comando
                };

                OracleDataReader reader;
                reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    ParametrosMQConsulta parametro = new ParametrosMQConsulta
                    {
                        OPERACION = reader["NOMBRE_CAMPO"].ToString(),
                        LONGITUD = reader["LONGITUD_CAMPO"].ToString()
                    };

                    parametros.Add(parametro);
                }

                // Always call Close when done reading.
                reader.Close();

            }
            catch (OracleException ex)
            {
                parametros = null;
                Console.WriteLine($"GetSQLError - {ex.Message}");
            }
            finally
            {
                conexion.Close();
                conexion.Dispose();
            }



            return parametros;
        }
        /// <summary>
        /// Permite determinar el inicio de la trama a enviar a la cola MQ
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="banco"> numero de banco al cual estamos consultando</param>
        /// <returns></returns>
        public int InicioTrama(string banco)
        {

            var conexion = oracle.NuevaConexion();
            int longitud = -1;
            string sql = string.Format(Properties.Resources.query_banco_ini, banco);

            try
            {
                OracleCommand comando = new OracleCommand(sql, conexion)
                {
                    CommandType = System.Data.CommandType.Text
                };

                OracleDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    longitud = Int32.Parse(reader[1].ToString()) - 1;
                }

            }
            catch (OracleException ex)
            {
                Console.WriteLine($"GetSQLError - {ex.Message}");
                longitud = -1;
            }
            finally
            {
                conexion.Close();
                conexion.Dispose();
            }

            return longitud;
        }
        /// <summary>
        /// Permite registrar el Evento de pago en el historial Visa
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="datos"> datos que contienen la informacion de pago</param>
        /// <param name="datos_visa">datos que indican la operacion de visa</param>
        /// <param name="operacion">tipo de operacion a realizar en este caso pago 552</param>
        /// <param name="messageID">mensaje unico el cual hace referencia al historial de MQ</param>
        /// <returns></returns>
        public bool RegistrarEvento(DatosPago datos, RespuestaVisa datos_visa, string operacion, string messageID)
        {

            var conexion = oracle.NuevaConexion();
            try
            {
                OracleCommand comando = new OracleCommand("MQPRO.NEW_VISA_HISTORY", conexion)
                {
                    CommandType = CommandType.StoredProcedure,
                    BindByName = true
                };
                DateTime date = System.DateTime.Now;

                string n_tarjeta = datos.TARJETA.Substring(datos.TARJETA.Length - 4);
                string tarjeta = "XXXXXXXXXXXX" + n_tarjeta;
                comando.Parameters.Add("p_fecha", OracleDbType.Date, date, ParameterDirection.Input);
                comando.Parameters.Add("p_hora", OracleDbType.Varchar2, date.ToString("HH:mm:ss"), ParameterDirection.Input);
                comando.Parameters.Add("p_nis", OracleDbType.Varchar2, datos.NIS_NIR, ParameterDirection.Input);
                comando.Parameters.Add("p_tarjeta", OracleDbType.Varchar2, tarjeta, ParameterDirection.Input);
                comando.Parameters.Add("p_monto", OracleDbType.Varchar2, datos.MONTO, ParameterDirection.Input);
                comando.Parameters.Add("p_f_exp", OracleDbType.Varchar2, datos.FECHA_EXPIRACION, ParameterDirection.Input);
                comando.Parameters.Add("p_operacion", OracleDbType.Varchar2, operacion, ParameterDirection.Input);
                comando.Parameters.Add("p_audit", OracleDbType.Varchar2, datos_visa.AuditNumber, ParameterDirection.Input);
                comando.Parameters.Add("p_reference", OracleDbType.Varchar2, datos_visa.ReferenceNumber, ParameterDirection.Input);
                comando.Parameters.Add("p_response", OracleDbType.Varchar2, datos_visa.ResponseCode, ParameterDirection.Input);
                comando.Parameters.Add("p_message", OracleDbType.Varchar2, datos_visa.MessageType, ParameterDirection.Input);
                comando.Parameters.Add("p_msg", OracleDbType.Varchar2, messageID, ParameterDirection.Input);

                comando.ExecuteNonQuery();
                return true;
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"GetSQLError - {ex.Message}");
                return false;
            }
            finally
            {
                conexion?.Close();
                conexion.Dispose();
            }
        }
        /// <summary>
        /// Permite obtener el mensaje de respuesta del MQ 
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <param name="messageID">variable con la cual se va a buscar al MQ la respuesta</param>
        /// <returns></returns>
        public string ObtenerRespuesta(string messageID)
        {
            string respuesta = string.Empty;

            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 100000; i++)
            {
                if (watch.Elapsed.TotalSeconds <= 10 && respuesta.Equals(string.Empty))
                {
                    var conexion = oracle.NuevaConexion();
                    try
                    {
                        //hacerlo hasta 10 segundos
                        OracleCommand comando = new OracleCommand("MQPRO.GET_VISA_RESPONSE", conexion)
                        {
                            CommandType = CommandType.StoredProcedure,
                            BindByName = true
                        };

                        comando.Parameters.Add("p_messageID", OracleDbType.Varchar2, messageID, ParameterDirection.Input);

                        OracleParameter op = new OracleParameter
                        {
                            OracleDbType = OracleDbType.RefCursor,
                            ParameterName = "cursor01",
                            Direction = ParameterDirection.Output
                        };
                        comando.Parameters.Add(op);

                        using (var reader = comando.ExecuteReader())
                        {
                            while (reader != null && reader.Read())
                            {
                                respuesta = reader.GetValue(0).ToString();
                                break;

                            }
                        }
                    }
                    catch (OracleException ex)
                    {
                        Console.WriteLine($"GetSQLError - {ex.Message}");
                        respuesta = string.Empty;
                    }
                    finally
                    {
                        conexion?.Close();
                        conexion.Dispose();
                    }
                }
                else
                {
                    break;
                }
            }
            watch.Stop();

            return respuesta;
        }
    }
}