using API.Entities;
using System.Text.RegularExpressions;
using System.Collections;
using System.Data;
namespace API.Controllers
{
    public class Controller
    {
        Connection connection = new Connection();
        string[]? data;
        string[]? row;
        ArrayList validated = new ArrayList();

        public void Insert(string query)
        {
            connection.connection(query);
        }

        public void Delete(string nombreTable)
        {
            string query = $"DELETE {nombreTable}";
            Insert(query);
        }

        public Content Validate(string[] row, int index, int NoColumnas)
        {
            string message = "";
            bool flag = true;


            if (row.Length != NoColumnas)
            {
                flag = false;
                message += $"El numero de columnas no coincide con la base de datos en la fila {index + 1} (Validar el separador) \n";
            }

            for (int i = 0; i < row.Length; i++)
            {
                if (!Regex.IsMatch(row[i], @"^[a-zA-ZñÑ°.=*?()&-_#'\s0-9,/:-]+$"))
                {
                    if (!string.IsNullOrEmpty(row[i]))
                    {
                        flag = false;
                        message += $"En la columna {i + 1} de la fila {index + 1} {row[i]} Contiene caracteres invalidos  ";
                    }

                }
            }


            Content ebejico = new Content(flag, message);

            return ebejico;
        }


        public string Reader(string body, Municipio municipio, AttachFile files)
        {
            var file = body.TrimEnd();
            data = file.Split("\n");

            string regreso = SetRows(municipio.nombreTable[0].ToString(), municipio, (int)municipio.NoColumnas[0], files, data);

            return regreso;

        }



        public string SetRows(string nombreTable, Municipio municipio, int NoColumnas, AttachFile files, string[] data)
        {
            //DateTime fecha = new DateTime();
            string query = $"INSERT INTO {nombreTable} VALUES";

            try
            {
                int iterador = 0;
                while (iterador < data.Length)
                {


                    row = data[iterador].TrimEnd().Split(files.separator);
                    var content = new Content();
                    content.flag = true;
                    content = Validate(row, iterador, NoColumnas);


                    if (content.flag == true)
                    {
                        int i = 0;
                        foreach (var item in row)
                        {
                            query = i == 0 ? query + $"('{row[i]}'" : query + $",'{row[i]}'";
                            i++;
                        }
                        query = query + "),";

                        if ((iterador % 999) == 0)
                        {
                            try
                            {
                                query = query.TrimEnd(',');
                                validated.Add(query);
                                query = $"INSERT INTO {nombreTable} VALUES";
                            }
                            catch (Exception ex)
                            {
                                return ex.Message;
                            }
                        }
                    }

                    else
                    {
                        return content.message;
                    }


                    iterador++;
                }


                try
                {
                    query = query.TrimEnd(',');
                    validated.Add(query);

                }
                catch (Exception ex)
                {
                    connection.InsertException(ex);
                    return ex.Message;
                }
            }
            catch (System.Exception ex)
            {
                connection.InsertException(ex);
                return ex.Message;
            }

            if (files.delete)
            {
                Delete(nombreTable);
            }

            foreach (string item in validated)
            {
                Insert(item);
            }


            return $"Se insertaron los datos correctamente en el {municipio.nombre}";
        }

        public string ReaderExperimental(string body, Municipio municipio, AttachFile files)
        {
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

            string regreso1 = SetRows(municipio.nombreTable[0].ToString(), municipio, (int)municipio.NoColumnas[0], files, Tabla1);

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


                Tabla2[i] = div[2] + files.separator + Totales1[i] + files.separator + Fechas1[i] + files.separator + Totales2[i] + files.separator + Fechas2[i] + files.separator + Totales3[i] + files.separator + Fechas3[i];
                ;
            }

            string regreso2 = SetRows(municipio.nombreTable[1].ToString(), municipio, (int)municipio.NoColumnas[1], files, Tabla2);


            //Tabla3
            string[] Tabla3 = new string[tabla3.Count];

            for (int i = 0; i < Tabla3.Length; i++)
            {
                Tabla3[i] = tabla3[i].ToString().TrimEnd('|');
            }

            string regreso3 = SetRows(municipio.nombreTable[2].ToString(), municipio, (int)municipio.NoColumnas[2], files, Tabla3);

            //Tabla4
            string[] Tabla4 = new string[tabla4.Count];

            for (int i = 0; i < Tabla4.Length; i++)
            {
                Tabla4[i] = tabla4[i].ToString().TrimEnd('|');
            }

            string regreso4 = SetRows(municipio.nombreTable[3].ToString(), municipio, (int)municipio.NoColumnas[3], files, Tabla4);


            //Tabla5
            string[] Tabla5 = new string[tabla5.Count];

            for (int i = 0; i < Tabla5.Length; i++)
            {
                Tabla5[i] = tabla5[i].ToString().TrimEnd('|');
            }

            string regreso5 = SetRows(municipio.nombreTable[4].ToString(), municipio, (int)municipio.NoColumnas[4], files, Tabla5);

            //Tabla6
            string[] Tabla6 = new string[tabla6.Count];

            for (int i = 0; i < Tabla6.Length; i++)
            {
                Tabla6[i] = tabla6[i].ToString().TrimEnd('|');
            }

            string regreso6 = SetRows(municipio.nombreTable[5].ToString(), municipio, (int)municipio.NoColumnas[5], files, Tabla6);

            return $" Tabla 1 \n {regreso1} \n Tabla 2 \n {regreso2} \n Tabla 3 \n {regreso3} \n Tabla 4 \n {regreso4} \n"
            + $" Tabla 5 \n {regreso5} \n Tabla 6 \n {regreso6} \n";
        }

        public string ReaderEmpopasto(string body, Municipio municipio, AttachFile files)
        {

            if (files.delete == true)
            {
                Delete(municipio.nombreTable[0].ToString());
            }
            var Res = connection.ConsultaTabla(municipio);
            DataTable tbl = Res.Data;

            var file = body.TrimEnd();
            data = file.Split("\n");
            var linea = 0;
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
                connection.BulkInsert(municipio, tbl);
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