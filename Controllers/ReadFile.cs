
namespace project.controllers
{
    class ReadFile
    {

        public string readFile(string path)
        {
            string body = "";
            FileStream Archivo = File.Open(path, FileMode.Open);
            using (StreamReader reader = new StreamReader(Archivo))
            {
                body = reader.ReadToEnd();
            }
            return body;
        }
    }
}

