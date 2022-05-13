using project.Entities;
using System.Text.RegularExpressions;
namespace project.controllers
{
    class EbejicoController
    {
        Connection connection = new Connection();
        string[]? data;
        string[]? row;

        public void Insert(string query)
        {
            connection.connection(query);
        }

        public void Delete()
        {
            string query = "DELETE Ebejico_P";
            Insert(query);
        }

        public Content Validate(string[] row, int index)
        {
            string message = "";
            bool flag = true;


            if (row.Length < 28)
            {
                flag = false;
                message += $"El numero de columnas no coincide con la base de datos en la fila {index +1} \n";
            }

            for (int i = 0; i < row.Length; i++)
            {
                if (!Regex.IsMatch(row[i], @"^[a-zA-ZñÑ°\s0-9,/:-]+$"))
                {
                    flag = false;
                    message += $"{row[i]} Contiene caracteres invalidos en la fila {index +1}";
                }
            }


            Content ebejico = new Content(flag, message);

            return ebejico;
        }

        public string Reader()
        {

            Delete();
            var reader = new ReadFile();
            string file = reader.readFile("./ArchivosPlanos/pepe.XLS").TrimEnd();
            data = file.Split("\r");
            DateTime fecha = new DateTime();
            string query = "INSERT INTO Ebejico_P VALUES";

            try
            {
                int iterador = 1;


                while (iterador < data.Length)
                {
                    row = data[iterador].TrimEnd().Split('\t');
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
                                query = "INSERT INTO Ebejico_P VALUES";
                            }
                            catch (Exception ex)
                            {
                                Delete();
                                connection.InsertException(ex);
                            }
                        }
                    }

                    else
                    {
                        Delete();
                        Console.WriteLine(content.message);
                        Console.ReadKey();
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

                    Delete();
                    connection.InsertException(ex);

                }
            }
            catch (System.Exception ex)
            {
                connection.InsertException(ex);
                Delete();
            }

            return "200 ok";
        }
    }
}