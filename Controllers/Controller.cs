using API.Entities;
using System.Text.RegularExpressions;
using System.Collections;
namespace API.Controllers
{
    public class EbejicoController
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


            if (row.Length != NoColumnas )
            {
                flag = false;
                message += $"El numero de columnas no coincide con la base de datos en la fila {index + 1} (Validar el separador) \n";
            }

            for (int i = 0; i < row.Length; i++)
            {
                if (!Regex.IsMatch(row[i], @"^[a-zA-ZñÑ°\s0-9,/:-]+$"))
                {
                    flag = false;
                    message += $"En la columna {i} de la fila {index + 1} {row[i]} Contiene caracteres invalidos  ";
                }
            }


            Content ebejico = new Content(flag, message);

            return ebejico;
        }

        public string Reader(string body, string nombreTable, string separator, bool delete, int NoColumnas,  string nombreMuni)
        {
            var file = body.TrimEnd();
            data = file.Split("\n");


            //DateTime fecha = new DateTime();
            string query = $"INSERT INTO {nombreTable} VALUES";

            try
            {
                int iterador = 0;
                while (iterador < data.Length)
                {
                    row = data[iterador].TrimEnd().Split(separator);
                    var content = new Content();
                    content = Validate(row, iterador, NoColumnas);

                    if (content.flag == true)
                    {
                        for (int i = 0; i < row.Length; i++)
                        {
                            // if (i == 7)
                            // {
                            //     fecha = DateTime.Parse(row[i]);
                            //     row[i] = fecha.ToString("yyyy-MM-dd"); //problema problema
                            // }
                            if (i == 0)
                            {
                                query = query + $"('{row[i]}'";
                            }
                            else
                            {
                                query = query + $",'{row[i]}'";
                            }

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
                    return ex.Message;
                }
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
            
            if (delete)
            {
                Delete(nombreTable);
            }

            foreach (string item in validated)
            {
                Insert(item);
            }

            return $"Se insertaron los datos correctamente en el {nombreMuni}";
        }
    }
}