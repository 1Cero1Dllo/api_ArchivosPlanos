using System.Data.SqlClient;
using Newtonsoft;
using System.Data;
namespace API.Entities;
public class Connection
{
    private string url { get; set; }
    Municipio municipio = new();

    public Connection()
    {
        IConfigurationBuilder config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfiguration configuration = config.Build();
        this.url = configuration["DBConnection"];
    }

    public Respuesta Consulta(string Query)
    {
        Respuesta res = new();
        try
        {
            using (SqlConnection connection = new SqlConnection(this.url))
            {
                connection.Open();
                string query = $"{Query}";
                SqlCommand comando = new SqlCommand(query, connection);
                SqlDataReader reader = comando.ExecuteReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);
                res.Data = dataTable;
                connection.Close();
            }

        }
        catch (System.Exception ex)
        {

            res.Mensaje = ex.Message;
        }

        return res;
    }
    public Respuesta ConsultaTabla(Municipio municipio, string NombreTable)
    {
        Respuesta res = new();
        try
        {
            using (SqlConnection connection = new SqlConnection(this.url))
            {
                connection.Open();
                string query = $"SELECT TOP 0 * FROM {NombreTable}";
                SqlCommand comando = new SqlCommand(query, connection);
                SqlDataReader reader = comando.ExecuteReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);
                res.Data = dataTable;
                res.Mensaje = $"Se insertaron los datos correctamente en {municipio.nombre}";
                connection.Close();
                return res;
            }

        }
        catch (Exception ex)
        {
            InsertException(ex);
            res.Mensaje = ex.Message;
            return res;
        }
    }
    public List<string> ConsultaCodigo(Municipio municipio, string NombreTable)
    {
        Respuesta res = new();

        try
        {
            using (SqlConnection connection = new SqlConnection(this.url))
            {
                connection.Open();
                string query = $"SELECT Codigo FROM {NombreTable}";
                SqlCommand comando = new SqlCommand(query, connection);
                SqlDataReader reader = comando.ExecuteReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);


                List<string> list = dataTable.AsEnumerable().Select(r => r.Field<string>("Codigo")).ToList();



                connection.Close();

                return list;
            }

        }
        catch (Exception ex)
        {

            return new List<string>();
        }
    }




    public void BulkInsert(string NombreTabla, DataTable dataTable)
    {
        using (SqlConnection connection = new SqlConnection(this.url))
        {
            connection.Open();
            SqlBulkCopy bulkCopy = new SqlBulkCopy(connection);
            bulkCopy.DestinationTableName = NombreTabla;
            bulkCopy.WriteToServer(dataTable);
            connection.Close();
        }

    }

    public void BulkInsertTemp(string NombreTabla, DataTable dataTable)
    {
        using (SqlConnection connection = new SqlConnection(this.url))
        {
            connection.Open();
            SqlBulkCopy bulkCopy = new SqlBulkCopy(connection);
            bulkCopy.DestinationTableName = NombreTabla;
            bulkCopy.WriteToServer(dataTable);
            connection.Close();
        }

    }
    public Municipio ConsultaMunicipio(string codigoMunicipio, int tipo)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(this.url))
            {
                connection.Open();
                string query = $"SELECT T.NombreTable, M.Nombre, (SELECT COUNT(*) FROM information_schema.columns WHERE table_name = (T.NombreTable)) as Columnas FROM TblMunicipiosxTabla T INNER JOIN TblMunicipios M ON T.Municipio = M.CodigoMunicipio WHERE Municipio ='{codigoMunicipio}' AND TipoImpuesto = '{tipo}'";
                SqlCommand comando = new SqlCommand(query, connection);
                SqlDataReader reader = comando.ExecuteReader();



                while (reader.Read())
                {
                    municipio.nombreTable.Add(reader.GetString(0));
                    municipio.nombre = reader.GetString(1);
                    municipio.NoColumnas.Add(reader.GetInt32(2));
                }

                return municipio;
            }

        }
        catch (Exception ex)
        {
            InsertException(ex);
            municipio.Error = ex.Message;
            return municipio;
        }

    }
    public void connection(string query, string codigoMunicipio)
    {

        try
        {
            using (SqlConnection connection = new SqlConnection(this.url))
            {
                connection.Open();
                SqlCommand comando = new SqlCommand(query, connection);
                comando.ExecuteNonQuery();
                connection.Close();
            }


        }
        catch (Exception ex)
        {
            InsertException(ex, codigoMunicipio);
        }

    }
    public void connection( string Tabla, DataTable tbl,string codigoMunicipio)
    {

        try
        {
            using (SqlConnection connection = new SqlConnection(this.url))
            {
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    connection.Open();
                    command.CommandText = $"SELECT top 0 *INTO #temp{Tabla} FROM [ArchivosPlanos].[dbo].[{Tabla}]";
                    command.ExecuteNonQuery();
       
                  using (SqlBulkCopy bulkcopy = new SqlBulkCopy(connection))
                {
                    bulkcopy.BulkCopyTimeout = 660;
                    bulkcopy.DestinationTableName = $"#temp{Tabla}";
                    bulkcopy.WriteToServer(tbl);
                    bulkcopy.Close();
                }

                command.CommandTimeout = 300;
                command.CommandText = $"delete from  {Tabla} where Codigo in (select t.Codigo from  #temp{Tabla} t)" ;
                command.ExecuteNonQuery();
            command.CommandTimeout = 300;
                command.CommandText = $"drop table #temp{Tabla}" ;
                command.ExecuteNonQuery();
                }
              using (SqlBulkCopy bulkcopy = new SqlBulkCopy(connection))
                {
                    bulkcopy.BulkCopyTimeout = 660;
                    bulkcopy.DestinationTableName = Tabla;
                    bulkcopy.WriteToServer(tbl);
                    bulkcopy.Close();
                }

            }


        }
        catch (Exception ex)
        {
            InsertException(ex,codigoMunicipio);
        }

    }




    public void connection(List<string> query, string codigoMunicipio)
    {

        try
        {
            using (SqlConnection connection = new SqlConnection(this.url))
            {
                connection.Open();
                foreach (string item in query)
                {
                    SqlCommand comando = new SqlCommand(string.Join(" ", item.ToString()), connection);
                    comando.CommandTimeout = 100;
                    comando.ExecuteNonQuery();


                }



                connection.Close();


            }


        }
        catch (Exception ex)
        {
            InsertException(ex, codigoMunicipio);
        }

    }

    public void InsertException(Exception ex)
    {


        using (SqlConnection connection = new SqlConnection(this.url))
        {
            connection.Open();
            var Exception = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            string query = $"sp_InsertLogException '{Exception.ToString().Replace("\'", "\"")}'";
            SqlCommand comando = new SqlCommand(query, connection);
            comando.ExecuteNonQuery();
            connection.Close();
        }

    }

    public void InsertException(Exception ex, string codigoMunicipio)
    {


        using (SqlConnection connection = new SqlConnection(this.url))
        {
            connection.Open();
            var Exception = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            string query = $"sp_InsertLogException '{Exception.ToString().Replace("\'", "\"")}', {codigoMunicipio}";
            SqlCommand comando = new SqlCommand(query, connection);
            comando.ExecuteNonQuery();
            connection.Close();
        }

    }

    public void ejecutarProcedure(string NombreTablaTemp, DataTable dataTable)
    {

        using (SqlConnection connection = new SqlConnection(this.url))
        {
            connection.Open();

            SqlCommand comando = new SqlCommand("comprarar", connection);
            comando.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = comando.ExecuteReader();








            connection.Close();


        }

    }
}