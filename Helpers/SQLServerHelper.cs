using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ducsa.Extranet.Core.Helpers
{
    public class SQLServerHelper
    {
        private const int TIMEOUT_PREDETERMINADO = 30;

        private static SqlConnection conexion = null;

        private string _connectionString = "";

        public SQLServerHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection ObtenerConexion()
        {
            if (conexion == null || (conexion != null && conexion.State != ConnectionState.Open))
            {
                try
                {
                    conexion = null;
                    conexion = new SqlConnection();
                    conexion.ConnectionString = _connectionString;

                    conexion.Open();
                    while (conexion.State != ConnectionState.Open)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
                catch (Exception excepcion)
                {
                    throw new Exception(excepcion.Message, excepcion);
                }
            }
            return conexion;
        }

        public object EjecutarStoreProcedure(string proc, List<string> parametros)
        {
            SqlDataAdapter adaptador = null;
            SqlCommand comandoSql = null;
            SqlParameter valorRet = null;
            object retorno = null;

            try
            {
                adaptador = new SqlDataAdapter();
                comandoSql = new SqlCommand();

                comandoSql.CommandText = proc;
                comandoSql.CommandType = System.Data.CommandType.StoredProcedure;
                comandoSql.Connection = ObtenerConexion();

                if ((parametros != null))
                {
                    foreach(var elem in parametros)
                    {
                        comandoSql.Parameters.Add(elem, SqlDbType.VarChar); //ESTO ESTA MAL
                    }
                }

                valorRet = comandoSql.Parameters.Add("@ReturnValue", SqlDbType.Int);
                valorRet.Direction = ParameterDirection.ReturnValue;
                comandoSql.ExecuteNonQuery();

                retorno = comandoSql.Parameters["@ReturnValue"].Value;
            }
            catch (Exception excepcion)
            {
                throw new Exception(excepcion.Message, excepcion);
            }
            finally
            {
                if ((comandoSql != null))
                {
                    comandoSql.Dispose();
                    comandoSql = null;
                }
            }

            return retorno;
        }
    }
}
