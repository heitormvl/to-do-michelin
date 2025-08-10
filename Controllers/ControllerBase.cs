using Microsoft.AspNetCore.Mvc;
using to_do_michelin.Models;

namespace to_do_michelin.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult Success<T>(T data, string message = "Operação realizada com sucesso")
        {
            return Ok(new RetornoModel<T>
            {
                Success = true,
                Message = message,
                Data = data
            });
        }

        protected IActionResult Success(string message = "Operação realizada com sucesso")
        {
            return Ok(new RetornoModel
            {
                Success = true,
                Message = message
            });
        }

        protected IActionResult Created<T>(T data, string message = "Recurso criado com sucesso")
        {
            return StatusCode(201, new RetornoModel<T>
            {
                Success = true,
                Message = message,
                Data = data
            });
        }

        protected IActionResult NoContent(string message = "Operação realizada com sucesso")
        {
            return StatusCode(204, new RetornoModel
            {
                Success = true,
                Message = message
            });
        }

        protected IActionResult BadRequest(string message, List<string>? errors = null)
        {
            return StatusCode(400, new RetornoModel
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            });
        }

        protected IActionResult Unauthorized(string message = "Usuário não autenticado")
        {
            return StatusCode(401, new RetornoModel
            {
                Success = false,
                Message = message
            });
        }

        protected IActionResult Forbidden(string message = "Acesso negado")
        {
            return StatusCode(403, new RetornoModel
            {
                Success = false,
                Message = message
            });
        }

        protected IActionResult NotFound(string message = "Recurso não encontrado")
        {
            return StatusCode(404, new RetornoModel
            {
                Success = false,
                Message = message
            });
        }

        protected IActionResult Conflict(string message = "Conflito de dados")
        {
            return StatusCode(409, new RetornoModel
            {
                Success = false,
                Message = message
            });
        }

        protected IActionResult UnprocessableEntity(string message, List<string>? errors = null)
        {
            return StatusCode(422, new RetornoModel
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            });
        }

        protected IActionResult InternalServerError(string message = "Erro interno do servidor", List<string>? errors = null)
        {
            return StatusCode(500, new RetornoModel
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            });
        }

        protected IActionResult Error(string message, int statusCode, List<string>? errors = null)
        {
            return StatusCode(statusCode, new RetornoModel
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            });
        }
    }
}
