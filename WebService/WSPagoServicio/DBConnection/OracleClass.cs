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
        private string connectionString = "";
        /// <summary>
        /// Permite crear una nueva conexion con la base de datos del MQ
        /// Creado por: Ludwing Ottoniel Cano fuentes - 05/03/2019
        ///</summary>
        /// <returns></returns>
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

    }
}