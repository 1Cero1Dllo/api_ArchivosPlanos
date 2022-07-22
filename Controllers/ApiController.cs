using Microsoft.AspNetCore.Mvc;
using API.Entities;
using Newtonsoft.Json;
using System.Text;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;

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
    [Route("/GetImpuestosxMunicipio")]
    public IActionResult GetImpuestos(string codigoMunicipio)
    {
        Connection connection = new();
        string query = $@"EXEC sp_GetImpuestoxMunicipio '{codigoMunicipio}'";
        var res = connection.Consulta(query);
        string json = JsonConvert.SerializeObject(res);
        return Ok(json);
    }


    [HttpPost]
    [Route("/File")]
    public IActionResult ReadFile(AttachFile File)
    {

        string body;
        string resultado;
        byte[] valueBytes;


        Connection connection = new Connection();
        Municipio municipio = connection.ConsultaMunicipio(File.codigoMunicipio, File.tipo);

        if (!string.IsNullOrEmpty(municipio.nombreTable.ToString()))
        {
            try
            {
                valueBytes = System.Convert.FromBase64String(File.base64);
                body = Encoding.UTF8.GetString(valueBytes);
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);
            }

            try
            {
                if (File.codigoMunicipio == "8001000588" || File.codigoMunicipio == "8907020342")
                {
                    if (File.tipo == 1)
                    {
                        resultado = controller.ReaderExperimental(body, municipio, File);
                    }
                    else
                    {
                        resultado = controller.ReaderExperimentalAcuerdos(body, municipio, File);
                    }

                }
                else if (File.codigoMunicipio == "8912006863")
                {
                    resultado = controller.ReaderEmpopasto(body, municipio, File);
                }
                else
                {
                    resultado = controller.ReaderEmpopasto(body, municipio, File);
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




