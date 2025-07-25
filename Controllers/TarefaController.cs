using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using to_do_michelin.Models;
using to_do_michelin.Services;

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
        var novaTarefa = new Tarefa
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao
        };

        var tarefa = await _service.CriarAsync(novaTarefa);
        return CreatedAtAction(nameof(GetById), new { id = tarefa.Id }, tarefa);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] TarefaCreateDTO dto)
    {
        var atualizada = new Tarefa
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao
        };

        var sucesso = await _service.AtualizarAsync(id, atualizada);
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
