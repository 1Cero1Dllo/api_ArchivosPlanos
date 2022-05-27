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


    [HttpGet]
    [Route("/saludame")]
    public string saludar(string name)
    {
        return $"Hola {name}, te amo";
    }



    [HttpPost]
    [Route("/File")]
    public IActionResult ReadFile(AttachFile File)
    {

        string body;
        string resultado;


        Connection connection = new Connection();
        Municipio municipio = connection.ConsultaMunicipio(File.codigoMunicipio, File.tipo);

        if (!string.IsNullOrEmpty(municipio.nombreTable))
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
                resultado = controller.Reader(body, municipio.nombreTable, File.separator, File.delete, municipio.NoColumnas, municipio.nombre);
            }
            catch (System.Exception)
            {

                return NotFound();
            }
        }
        else{
            return Problem("El tipo de tramite no esta habilitado para subir archivos planos");
        }


        return Ok(resultado);
    }



}

