using API.Entities;
using System.Text.RegularExpressions;
namespace API.Controllers
{
    public class EbejicoController
    {
        Connection connection = new Connection();
        string[]? data;
        string[]? row;

        public void Insert(string query)
        {
            connection.connection(query);
        }

        public void Delete(string nombreTable)
        {
            string query = $"DELETE {nombreTable}";
            Insert(query);
        }

        public Content Validate(string[] row, int index)
        {
            string message = "";
            bool flag = true;


            if (row.Length < 28)
            {
                flag = false;
                message += $"El numero de columnas no coincide con la base de datos en la fila {index + 1} \n";
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

        public string Reader(string body, string nombreTable, string separator, bool delete)
        {


            if (delete)
            {
                Delete(nombreTable);
            }

            var file = body.TrimEnd();
            data = file.Split("\n");
            DateTime fecha = new DateTime();
            string query = $"INSERT INTO {nombreTable} VALUES";

            try
            {
                int iterador = 1;


                while (iterador < data.Length)
                {
                    row = data[iterador].TrimEnd().Split(separator);
                    var content = new Content();
                    content = Validate(row, iterador);

                    if (content.flag == true)
                    {
                        for (int i = 0; i < row.Length; i++)
                        {
                            if (i == 7)
                            {
                                fecha = DateTime.Parse(row[i]);
                                row[i] = fecha.ToString("yyyy-MM-dd");
                            }
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
                                Insert(query);
                                query = $"INSERT INTO {nombreTable} VALUES";
                            }
                            catch (Exception ex)
                            {
                                Delete(nombreTable);
                                connection.InsertException(ex);
                                return ex.Message;
                            }
                        }
                    }

                    else
                    {
                        Delete(nombreTable);
                        // Console.WriteLine(content.message);
                        // Console.ReadKey();
                        return content.message;
                    }


                    iterador++;
                }


                try
                {
                    query = query.TrimEnd(',');
                    Insert(query);

                }
                catch (Exception ex)
                {

                    Delete(nombreTable);
                    connection.InsertException(ex);
                    return ex.Message;

                }
            }
            catch (System.Exception ex)
            {
                connection.InsertException(ex);
                Delete(nombreTable);
                return ex.Message;
            }

            return "200 ok";
        }
    }
}