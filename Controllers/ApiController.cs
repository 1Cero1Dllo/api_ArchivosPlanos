using Microsoft.AspNetCore.Mvc;
using API.Entities;
using System.Text;


namespace API.Controllers;

[ApiController]
[Route("[controller]")]

public class ApiController : ControllerBase
{

    EbejicoController Ebejico = new();


    [HttpGet]
    [Route("/")]
    public string index()
    {
        return $"Hola, te amo";
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
        string nombreTable = connection.ConsultaTabla(File.codigoMunicipio, File.tipo);

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
            resultado = Ebejico.Reader(body, nombreTable, File.separator, File.delete);
        }
        catch (System.Exception)
        {

            return NotFound();
        }


        return Ok(resultado);
    }


    
}

