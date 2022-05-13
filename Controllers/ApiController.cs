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
            resultado = Ebejico.Reader(body);
        }
        catch (System.Exception)
        {

            return NotFound();
        }


        return Ok(resultado);
    }
}