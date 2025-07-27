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
        public async Task<IActionResult> Get()
        {
            var tarefas = await _service.ListarAsync();
            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var tarefa = await _service.BuscarPorIdAsync(id);
            if (tarefa == null) return NotFound();
            return Ok(tarefa);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TarefaCreateDTO dto)
        {
            var tarefa = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] TarefaCreateDTO dto)
        {
            var sucesso = await _service.AtualizarAsync(id, dto);
            if (!sucesso) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var sucesso = await _service.DeletarAsync(id);
            if (!sucesso) return NotFound();

            return NoContent();
        }
    }
}
