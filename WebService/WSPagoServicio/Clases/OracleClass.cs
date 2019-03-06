using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
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

    }
}