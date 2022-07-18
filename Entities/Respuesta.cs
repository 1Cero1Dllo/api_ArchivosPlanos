using System.Data;

namespace API.Entities
{
    public class Respuesta


    {
        public Respuesta()
        {

        }
        public Respuesta(DataTable data, string error)
        {
            this.Data = data;
            this.Mensaje = error;
        }

        public Respuesta(string error)
        {
            this.Mensaje = error;

        }
        public DataTable? Data { get; set; }
        public string Mensaje { get; set; }
    }
}