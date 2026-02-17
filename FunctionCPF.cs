using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using DIOFuncValidadorCPF.DTO;
using DIOFuncValidadorCPF.Services;

namespace DIOFuncValidadorCPF;

public class FunctionCPF
{
    #region Propriedades

    private readonly ILogger<FunctionCPF> logger;

    #endregion

    #region Construtor

    public FunctionCPF(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<FunctionCPF>();
    }

    #endregion

    #region Funções

    [Function("fnCPF")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        try
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            if (string.IsNullOrWhiteSpace(requestBody))
            {
                return new BadRequestObjectResult(new { Message = "Corpo da requisição está vazio" });
            }

            var jsonDocument = JsonDocument.Parse(requestBody);
            var cpfElement = jsonDocument.RootElement.GetProperty("cpf");
            string cpf = cpfElement.GetString() ?? "";

            if (string.IsNullOrEmpty(cpf))
            {
                return new BadRequestObjectResult(new { Message = "CPF não fornecido no corpo da requisição" });
            }

            bool isValid = ValidadorCPF.IsValid(cpf);

            return new OkObjectResult(new { CPF = cpf, IsValid = isValid });
        }
        catch (JsonException)
        {
            logger.LogError("Erro ao fazer parsing do JSON");
            return new BadRequestObjectResult(new { Message = "Corpo da requisição inválido - JSON mal formatado" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the request.");
            return new ObjectResult(new ResponseHttp { IsValid = false, Message = "An error occurred while processing the request." }) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }

    #endregion
}