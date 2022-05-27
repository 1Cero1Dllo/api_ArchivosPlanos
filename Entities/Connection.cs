using System.Data.SqlClient;
using Newtonsoft;
namespace API.Entities;
public class Connection
{
    private string url { get; set; }

    public Connection()
    {
        IConfigurationBuilder config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfiguration configuration = config.Build();
        this.url = configuration["DBConnection"];
    }

    public string ConsultaTabla(string codigoMunicipio, int tipo)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(this.url))
            {
                connection.Open();
                string query = $"SELECT NombreTable FROM TblMunicipiosxTabla WHERE Municipio ='{codigoMunicipio}' AND TipoImpuesto = '{tipo}' ";
                SqlCommand comando = new SqlCommand(query, connection);
                SqlDataReader reader = comando.ExecuteReader();

                string nombreTable= "";
                while (reader.Read())
                {
                    nombreTable = reader.GetString(0);
                }

                return nombreTable;
            }


        }
        catch (Exception ex)
        {
            return ex.Message;
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

            Console.WriteLine(ex.Message);

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