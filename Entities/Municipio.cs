using System.Collections;

namespace API.Entities
{
    public class Municipio
    {
        public Municipio()
        {
            this.nombreTable = new ArrayList();
            this.NoColumnas = new ArrayList();
        }

        public string nombre { get; set; }
        public ArrayList NoColumnas { get; set; }

        public ArrayList nombreTable { get; set; }

        public string Error { get; set; }

    }
}
