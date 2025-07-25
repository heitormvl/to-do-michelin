using to_do_michelin.Models;
using Microsoft.EntityFrameworkCore;

namespace to_do_michelin.Services
{
    public class TarefaService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public TarefaService(AppDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        private string? ObterUsuarioId()
        {
            return _httpContext.HttpContext?.User?.Identity?.Name;
        }

        public async Task<List<Tarefa>> ListarAsync()
        {
            var usuarioId = ObterUsuarioId();
            return await _context.Tarefas
                .Where(t => t.UsuarioId == usuarioId)
                .OrderByDescending(t => t.DataCriacao)
                .ToListAsync();
        }

        public async Task<Tarefa?> BuscarPorIdAsync(Guid id)
        {
            var usuarioId = ObterUsuarioId();
            return await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);
        }

        public async Task<Tarefa> CriarAsync(Tarefa tarefa)
        {
            var usuarioId = ObterUsuarioId() ?? "anonymous";
            tarefa.UsuarioId = usuarioId;
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
            return tarefa;
        }

        public async Task<bool> AtualizarAsync(Guid id, Tarefa tarefaAtualizada)
        {
            var tarefa = await BuscarPorIdAsync(id);
            if (tarefa == null) return false;

            tarefa.Titulo = tarefaAtualizada.Titulo;
            tarefa.Descricao = tarefaAtualizada.Descricao;
            tarefa.Concluida = tarefaAtualizada.Concluida;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletarAsync(Guid id)
        {
            var tarefa = await BuscarPorIdAsync(id);
            if (tarefa == null) return false;

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
