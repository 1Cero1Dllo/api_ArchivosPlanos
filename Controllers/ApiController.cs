using Microsoft.AspNetCore.Mvc;
using API.Entities;
using System.Text;


namespace API.Controllers;

[ApiController]
[Route("[controller]")]

public class ApiController : ControllerBase
{

    Controller controller = new();


    [HttpGet]
    [Route("/")]
    public string index()
    {
        return $"API Archivos planos";
    }


    [HttpPost]
    [Route("/File")]
    public IActionResult ReadFile(AttachFile File)
    {

        string body;
        string resultado;


        Connection connection = new Connection();
        Municipio municipio = connection.ConsultaMunicipio(File.codigoMunicipio, File.tipo);

        if (!string.IsNullOrEmpty(municipio.nombreTable.ToString()))
        {
            try
            {
                var valueBytes = System.Convert.FromBase64String(File.base64);
                body = Encoding.UTF8.GetString(valueBytes);
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);
            }

            try
            {
                if (File.codigoMunicipio == "8001000588" || File.codigoMunicipio == "8907020342" )
                {
                    resultado = controller.ReaderExperimental(body, municipio, File);
                }
                else
                {
                    resultado = controller.Reader(body, municipio, File);
                }
            }
            catch (System.Exception ex)
            {

                return NotFound(ex.Message);
            }
        }
        else
        {
            return Problem("El tipo de tramite no esta habilitado para subir archivos planos");
        }


        return Ok(resultado);
    }

}




