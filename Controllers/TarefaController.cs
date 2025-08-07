using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using to_do_michelin.Models;
using to_do_michelin.Services;
using to_do_michelin.DTOs;

namespace to_do_michelin.Controllers
{
    [ApiController]
    [Route("tarefas")]
    [Authorize]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaService _service;

        public TarefaController(TarefaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTarefas()
        {
            try
            {
                var tarefas = await _service.ListarAsync();
                return Ok(new { 
                    success = true, 
                    data = tarefas,
                    message = $"Encontradas {tarefas.Count} tarefas"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Erro ao listar tarefas",
                    error = ex.Message 
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterTarefa(Guid id)
        {
            try
            {
                var tarefa = await _service.BuscarPorIdAsync(id);
                if (tarefa == null) 
                    return NotFound(new { 
                        success = false, 
                        message = "Tarefa não encontrada" 
                    });
                    
                return Ok(new { 
                    success = true, 
                    data = tarefa 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Erro ao buscar tarefa",
                    error = ex.Message 
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarTarefa([FromBody] TarefaCreateDTO dto)
        {
            try
            {
                var tarefa = await _service.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterTarefa), new { id = tarefa.Id }, new { 
                    success = true, 
                    data = tarefa,
                    message = "Tarefa criada com sucesso"
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { 
                    success = false, 
                    message = "Usuário não autenticado" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Erro ao criar tarefa",
                    error = ex.Message 
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarTarefa([FromBody] TarefaUpdateDTO dto)
        {
            try
            {
                var sucesso = await _service.AtualizarAsync(dto);
                if (!sucesso) 
                    return NotFound(new { 
                        success = false, 
                        message = "Tarefa não encontrada ou não pertence ao usuário" 
                    });

                return Ok(new { 
                    success = true, 
                    message = "Tarefa atualizada com sucesso" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Erro ao atualizar tarefa",
                    error = ex.Message 
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ApagarTarefa(Guid id)
        {
            try
            {
                var sucesso = await _service.DeletarAsync(id);
                if (!sucesso) 
                    return NotFound(new { 
                        success = false, 
                        message = "Tarefa não encontrada ou não pertence ao usuário" 
                    });

                return Ok(new { 
                    success = true, 
                    message = "Tarefa excluída com sucesso" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Erro ao excluir tarefa",
                    error = ex.Message 
                });
            }
        }

        [HttpGet("estatisticas")]
        public async Task<IActionResult> ObterEstatisticas()
        {
            try
            {
                var estatisticas = await _service.ObterEstatisticasAsync();
                return Ok(new { 
                    success = true, 
                    data = estatisticas,
                    message = "Estatísticas obtidas com sucesso"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Erro ao obter estatísticas",
                    error = ex.Message 
                });
            }
        }

        [HttpPatch("{id}/alternar-conclusao")]
        public async Task<IActionResult> AlternarConclusaoTarefa(Guid id)
        {
            try
            {
                var tarefa = await _service.AlternarConclusaoAsync(id);
                if (tarefa == null) 
                    return NotFound(new { 
                        success = false, 
                        message = "Tarefa não encontrada ou não pertence ao usuário" 
                    });

                return Ok(new { 
                    success = true, 
                    data = tarefa,
                    message = tarefa.Concluida ? "Tarefa marcada como concluída" : "Tarefa marcada como pendente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Erro ao alternar status da tarefa",
                    error = ex.Message 
                });
            }
        }
    }
}
