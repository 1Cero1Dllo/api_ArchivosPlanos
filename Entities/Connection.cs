using System.Data.SqlClient;
using Newtonsoft;
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
            municipio.Error = ex.Message;
            return municipio;
        }

    }
    public void connection(string query)
    {

        try
        {
            using (SqlConnection connection = new SqlConnection(this.url))
            {
                connection.Open();
                SqlCommand comando = new SqlCommand(query, connection);
                comando.ExecuteNonQuery();
            }


        }
        catch (Exception ex)
        {
            InsertException(ex);
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
        }

    }
}