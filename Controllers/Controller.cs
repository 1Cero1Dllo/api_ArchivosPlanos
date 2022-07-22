using API.Entities;
using System.Text.RegularExpressions;
using System.Collections;
using System.Data;
using System.Globalization;

namespace API.Controllers
{
    public class Controller
    {
        Connection connection = new Connection();
        string[]? data;
        string[]? row;
        ArrayList validated = new ArrayList();

        AttachFile archivo = new AttachFile();

        public void Insert(string query)
        {
            connection.connection(query, archivo.codigoMunicipio);
        }

        public void Delete(string nombreTable)
        {
            string query = $"DELETE {nombreTable}";
            Insert(query);
        }

        public string ReaderExperimental(string body, Municipio municipio, AttachFile files)
        {

            if (files.delete == true)
            {
                foreach (var item in municipio.nombreTable)
                {
                    Delete(item.ToString());
                }
            }


            archivo = files;
            var file = body.TrimEnd();
            data = file.Split("\n");

            ArrayList tabla1 = new();
            ArrayList tabla2 = new();
            ArrayList tabla3 = new();
            ArrayList tabla4 = new();
            ArrayList tabla5 = new();
            ArrayList tabla6 = new();

            foreach (var item in data)
            {
                if (item[0] == '1')
                {
                    tabla1.Add(item);
                }
                else if (item[0] == '4')
                {
                    tabla2.Add(item);
                }
                else if (item[0] == '2')
                {
                    tabla3.Add(item);
                }
                else if (item[0] == '3')
                {
                    tabla4.Add(item);
                }
                else if (item[0] == '6')
                {
                    tabla5.Add(item);
                }
                else if (item[0] == '7')
                {
                    tabla6.Add(item);
                }
            }

            //tabla1
            string[] Tabla1 = new string[tabla1.Count];

            for (int i = 0; i < Tabla1.Length; i++)
            {
                Tabla1[i] = tabla1[i].ToString().TrimEnd('|');
            }

            tabla1 = new ArrayList(Tabla1);

            string regreso1 = Insert(tabla1, municipio, municipio.nombreTable[0].ToString(), files, (int)municipio.NoColumnas[0]);

            //Tabla2
            string[] Tabla2 = new string[tabla2.Count];
            ArrayList Totales1 = new();
            ArrayList Fechas1 = new();
            ArrayList Totales2 = new();
            ArrayList Fechas2 = new();
            ArrayList Totales3 = new();
            ArrayList Fechas3 = new();


            for (int i = 0; i < tabla2.Count; i++)
            {
                string[] div = tabla2[i].ToString().Split('|');
                Totales1.Add(div[3].Substring(40, 10));
                Fechas1.Add(div[3].Substring(54, 8));

                Totales2.Add(div[5].Substring(40, 10));
                Fechas2.Add(div[5].Substring(54, 8));

                Totales3.Add(div[7].Substring(40, 10));
                Fechas3.Add(div[7].Substring(54, 8));

                var cultureInfo = new CultureInfo("co-CO");

                var fecha1 = DateTime.ParseExact(Fechas1[i].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                var fecha2 = DateTime.ParseExact(Fechas2[i].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                var fecha3 = DateTime.ParseExact(Fechas3[i].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);

                
                Tabla2[i] = div[2] + files.separator + Totales1[i] + files.separator + fecha1 + files.separator + Totales2[i] + files.separator + fecha2 + files.separator + Totales3[i] + files.separator + fecha3;
                ;
            }


            tabla2 = new ArrayList(Tabla2);
            string regreso2 = Insert(tabla2, municipio, municipio.nombreTable[1].ToString(), files, (int)municipio.NoColumnas[1]);


            //Tabla3
            string[] Tabla3 = new string[tabla3.Count];

            for (int i = 0; i < Tabla3.Length; i++)
            {
                Tabla3[i] = tabla3[i].ToString().TrimEnd('|');
            }

            tabla3 = new ArrayList(Tabla3);
            string regreso3 = Insert(tabla3, municipio, municipio.nombreTable[2].ToString(), files, (int)municipio.NoColumnas[2]);


            //Tabla4
            string[] Tabla4 = new string[tabla4.Count];

            for (int i = 0; i < Tabla4.Length; i++)
            {
                Tabla4[i] = tabla4[i].ToString().TrimEnd('|');
            }

            tabla4 = new ArrayList(Tabla4);
            string regreso4 = Insert(tabla4, municipio, municipio.nombreTable[3].ToString(), files, (int)municipio.NoColumnas[3]);



            //Tabla5
            string[] Tabla5 = new string[tabla5.Count];

            for (int i = 0; i < Tabla5.Length; i++)
            {
                Tabla5[i] = tabla5[i].ToString().TrimEnd('|');
            }

            tabla5 = new ArrayList(Tabla5);
            string regreso5 = Insert(tabla5, municipio, municipio.nombreTable[4].ToString(), files, (int)municipio.NoColumnas[4]);


            //Tabla6
            string[] Tabla6 = new string[tabla6.Count];

            for (int i = 0; i < Tabla6.Length; i++)
            {
                Tabla6[i] = tabla6[i].ToString().TrimEnd('|');
            }

            tabla6 = new ArrayList(Tabla6);
            string regreso6 = Insert(tabla6, municipio, municipio.nombreTable[5].ToString(), files, (int)municipio.NoColumnas[5]);


            return $" Tabla 1 \n {regreso1} \n Tabla 2 \n {regreso2} \n Tabla 3 \n {regreso3} \n Tabla 4 \n {regreso4} \n"
            + $" Tabla 5 \n {regreso5} \n Tabla 6 \n {regreso6} \n";

        }

        public string ReaderExperimentalAcuerdos(string body, Municipio municipio, AttachFile files)
        {

            archivo = files;
            var file = body.TrimEnd();
            data = file.Split("\n");

            ArrayList tabla1 = new();
            ArrayList tabla2 = new();

            foreach (var item in data)
            {
                if (item[1] == '1')
                {
                    tabla1.Add(item);
                }
                else if (item[1] == '4')
                {
                    tabla2.Add(item);
                }
            }

            if (files.delete == true)
            {
                Delete(municipio.nombreTable[0].ToString());
                Delete(municipio.nombreTable[1].ToString());
            }

            string respuesta = Insert(tabla1, municipio, municipio.nombreTable[0].ToString(), files, (int)municipio.NoColumnas[0]);
            respuesta += Insert(tabla2, municipio, municipio.nombreTable[1].ToString(), files, (int)municipio.NoColumnas[1]);

            return respuesta;

        }
        public string Insert(ArrayList tabla, Municipio municipio, string NombreTabla, AttachFile files, int NumeroColumnas)
        {

            archivo = files;
            var Res = connection.ConsultaTabla(municipio, NombreTabla);
            DataTable tbl = Res.Data;

            foreach (string line in tabla)
            {
                try
                {
                    var cols = line.Split(files.separator);
                    DataRow dr = tbl.NewRow();

                    for (int i = 0; i < NumeroColumnas; i++)
                    {
                        if (cols.Length > i)
                        {
                            dr[i] = cols[i].Trim();
                        }
                    }

                    tbl.Rows.Add(dr);
                }
                catch (System.Exception ex)
                {
                    connection.InsertException(ex, files.codigoMunicipio);
                    var cols = line.Split(files.separator);
                    return ex.Message;
                }

            }
            try
            {
                connection.BulkInsert(NombreTabla, tbl);
            }
            catch (System.Exception ex)
            {
                connection.InsertException(ex);
                return ex.Message;
            }


            return Res.Mensaje;

        }


        public string ReaderEmpopasto(string body, Municipio municipio, AttachFile files)
        {

            archivo = files;
            if (files.delete == true)
            {
                Delete(municipio.nombreTable[0].ToString());
            }


            var Res = connection.ConsultaTabla(municipio, municipio.nombreTable[0].ToString());
            DataTable tbl = Res.Data;


            foreach (string line in data)
            {
                try
                {
                    var cols = line.Split(files.separator);
                    DataRow dr = tbl.NewRow();

                    for (int i = 0; i < int.Parse(municipio.NoColumnas[0].ToString()); i++)
                    {
                        if (cols.Length > i)
                        {
                            dr[i] = cols[i].Trim();
                        }
                    }

                    tbl.Rows.Add(dr);
                }
                catch (System.Exception ex)
                {
                    connection.InsertException(ex, files.codigoMunicipio);
                    var cols = line.Split(files.separator);
                    return ex.Message;
                }

            }
            try
            {
                connection.BulkInsert(municipio.nombreTable[0].ToString(), tbl);
            }
            catch (System.Exception ex)
            {
                connection.InsertException(ex);
                return ex.Message;
            }


            return Res.Mensaje;
        }
    }


}