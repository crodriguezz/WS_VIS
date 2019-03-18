using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using WSPagoServicio.Clases;

namespace WSPagoServicio
{
    public class OracleClass
    {
         string connectionString = "";


        public OracleConnection NuevaConexion()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MQConnection"].ConnectionString;

            try
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("GetConnectionError: No existe una cadena de conexión a la base de datos o es correcta.");
                }

                var connection = new OracleConnection(connectionString);
                connection.Open();
                return connection;

            }
            catch (Exception ex)
            {
                throw new Exception($"GetConnectionError: {ex.Message}");
            }
        }

        public List<ParametrosConsulta> GetParametros(string banco, string operacion)
        {
            List<ParametrosConsulta> parametros = new List<ParametrosConsulta>();

            var conexion = NuevaConexion();
            
                
                string sql = Properties.Resources.query_banco_detalle + banco + " AND CODIGO_MQ_FORMATO = '" + operacion + "' ORDER BY SECUENCIA ASC";
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
                    ParametrosConsulta parametro = new ParametrosConsulta
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
        /// Devuelve la posición donde inicia la trama
        /// </summary>
        /// <param name="conexion_"></param>
        /// <param name="banco"></param>
        /// <returns>valor de inicio</returns>
        public int InicioTrama(string banco)
        {

            var conexion = NuevaConexion();
            int longitud = -1;
            string sql = Properties.Resources.query_banco_ini + banco;            

            try
            {
                OracleCommand comando = new OracleCommand(sql, conexion)
                {
                    CommandType = System.Data.CommandType.Text
                };

                OracleDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    longitud = Int32.Parse(reader[1].ToString()) -1;
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

        public bool RegistrarEvento(DatosPago datos, RespuestaVisa datos_visa, string operacion, string messageID)
        {

            var conexion = NuevaConexion();
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
                comando.Parameters.Add("p_response", OracleDbType.Varchar2,datos_visa.ResponseCode, ParameterDirection.Input);
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

        public string ObtenerRespuesta(string messageID)
        {
            string respuesta = string.Empty;

            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 100000; i++)
            {
                if (watch.Elapsed.TotalSeconds <= 10 && respuesta.Equals(string.Empty))
                {
                    var conexion = NuevaConexion();
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