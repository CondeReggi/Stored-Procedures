namespace SQL_StoreProcedure
{
    using Microsoft.Data.SqlClient;
    using System.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Protocols;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;

    public class SQLHelpers
    {
        private IConfiguration Configuration { get; }
        public const int TIMEOUT_PREDETERMINADO = 30;
        public static SqlConnection conexion = null;

        public SQLHelpers(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public SqlConnection ObtenerConexion()
        {
            if (conexion == null || (conexion != null && conexion.State != ConnectionState.Open))
            {
                try
                {
                    conexion = null; //Para forzar un dispose en los casos en los que conexion != null

                    conexion = new SqlConnection();
                    conexion.ConnectionString = Configuration.GetConnectionString("Database");

                    conexion.Open();
                    while (conexion.State != ConnectionState.Open)
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
                catch (Exception excepcion)
                {
                    throw new Exception(excepcion.Message);
                }
            }
            return conexion;
        }

        public string getConnectionString()
        {
            return Configuration.GetConnectionString("Database");
        }

        public object EjecutarProcAlmacenado(string procAlmacenado)
        {
            SqlDataAdapter adaptador = null;
            SqlCommand comandoSql = null;
            object result = null;

            try
            {
                adaptador = new SqlDataAdapter();
                comandoSql = new SqlCommand();

                comandoSql.CommandText = procAlmacenado;
                comandoSql.CommandType = System.Data.CommandType.StoredProcedure;
                comandoSql.Connection = ObtenerConexion();

                adaptador.SelectCommand = comandoSql;

                comandoSql.Parameters.AddWithValue("@LastName", "");
                comandoSql.Parameters.AddWithValue("@FirstName", 1);

                comandoSql.ExecuteNonQuery();

                result = comandoSql.Parameters["@ReturnValue"].Value;
            }
            catch (Exception excepcion)
            {
                throw new Exception(excepcion.Message);
            }
            finally
            {
                if ((comandoSql != null))
                {
                    comandoSql.Dispose();
                    comandoSql = null;
                }
            }

           return result;
        }
    }
}





