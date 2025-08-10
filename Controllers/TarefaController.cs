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
    public class TarefaController : BaseController
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
                return Success(tarefas, $"Encontradas {tarefas.Count} tarefas");
            }
            catch (Exception ex)
            {
                return InternalServerError("Erro ao listar tarefas", new List<string> { ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterTarefa(Guid id)
        {
            try
            {
                var tarefa = await _service.BuscarPorIdAsync(id);
                if (tarefa == null) 
                    return NotFound("Tarefa não encontrada");
                    
                return Success(tarefa);
            }
            catch (Exception ex)
            {
                return InternalServerError("Erro ao buscar tarefa", new List<string> { ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarTarefa([FromBody] TarefaCreateDTO dto)
        {
            try
            {
                var tarefa = await _service.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterTarefa), new { id = tarefa.Id }, 
                    new RetornoModel<TarefaReadDTO>
                    {
                        Success = true,
                        Message = "Tarefa criada com sucesso",
                        Data = tarefa
                    });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Usuário não autenticado");
            }
            catch (Exception ex)
            {
                return InternalServerError("Erro ao criar tarefa", new List<string> { ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarTarefa([FromBody] TarefaUpdateDTO dto)
        {
            try
            {
                var sucesso = await _service.AtualizarAsync(dto);
                if (!sucesso) 
                    return NotFound("Tarefa não encontrada ou não pertence ao usuário");

                return Success("Tarefa atualizada com sucesso");
            }
            catch (Exception ex)
            {
                return InternalServerError("Erro ao atualizar tarefa", new List<string> { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ApagarTarefa(Guid id)
        {
            try
            {
                var sucesso = await _service.DeletarAsync(id);
                if (!sucesso) 
                    return NotFound("Tarefa não encontrada ou não pertence ao usuário");

                return Success("Tarefa excluída com sucesso");
            }
            catch (Exception ex)
            {
                return InternalServerError("Erro ao excluir tarefa", new List<string> { ex.Message });
            }
        }

        [HttpGet("estatisticas")]
        public async Task<IActionResult> ObterEstatisticas()
        {
            try
            {
                var estatisticas = await _service.ObterEstatisticasAsync();
                return Success(estatisticas, "Estatísticas obtidas com sucesso");
            }
            catch (Exception ex)
            {
                return InternalServerError("Erro ao obter estatísticas", new List<string> { ex.Message });
            }
        }

        [HttpPatch("{id}/alternar-conclusao")]
        public async Task<IActionResult> AlternarConclusaoTarefa(Guid id)
        {
            try
            {
                var tarefa = await _service.AlternarConclusaoAsync(id);
                if (tarefa == null) 
                    return NotFound("Tarefa não encontrada ou não pertence ao usuário");

                return Success(tarefa, tarefa.Concluida ? "Tarefa marcada como concluída" : "Tarefa marcada como pendente");
            }
            catch (Exception ex)
            {
                return InternalServerError("Erro ao alternar status da tarefa", new List<string> { ex.Message });
            }
        }
    }
}
