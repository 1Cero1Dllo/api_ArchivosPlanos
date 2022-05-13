using System.Data.SqlClient;
using Newtonsoft;
namespace project.Entities;
public class Connection
{
    string conexion = "Server=192.168.20.117;Database=ArchivosPlanos;User ID=apiUser;Password=1C3r012021*/-";

    public void connection(string query)
    {

        try
        {
            using (SqlConnection connection = new SqlConnection(conexion))
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
    

        using (SqlConnection connection = new SqlConnection(conexion))
        {
            connection.Open();
            var Exception = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
            string query = $"sp_InsertLogException '{Exception.ToString().Replace("\'", "\"")}'";
            SqlCommand comando = new SqlCommand(query, connection);
            comando.ExecuteNonQuery();
        }

    }

}
