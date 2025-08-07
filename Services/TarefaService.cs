using to_do_michelin.Models;
using to_do_michelin.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            return _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        private string? ObterUsuarioNome()
        {
            return _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        }

        public async Task<List<TarefaReadDTO>> ListarAsync()
        {
            var usuarioId = ObterUsuarioId();
            var usuarioNome = ObterUsuarioNome();
            
            var tarefas = await _context.Tarefas
                .Where(t => t.UsuarioId == usuarioId)
                .OrderByDescending(t => t.DataCriacao)
                .Select(t => new TarefaReadDTO
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Descricao = t.Descricao,
                    Concluida = t.Concluida,
                    DataCriacao = t.DataCriacao,
                    UsuarioId = t.UsuarioId,
                    UsuarioNome = usuarioNome
                })
                .ToListAsync();
                
            return tarefas;
        }

        public async Task<TarefaReadDTO?> BuscarPorIdAsync(Guid id)
        {
            var usuarioId = ObterUsuarioId();
            var usuarioNome = ObterUsuarioNome();
            
            var tarefa = await _context.Tarefas
                .Where(t => t.Id == id && t.UsuarioId == usuarioId)
                .Select(t => new TarefaReadDTO
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Descricao = t.Descricao,
                    Concluida = t.Concluida,
                    DataCriacao = t.DataCriacao,
                    UsuarioId = t.UsuarioId,
                    UsuarioNome = usuarioNome
                })
                .FirstOrDefaultAsync();
                
            return tarefa;
        }

        public async Task<TarefaReadDTO> CriarAsync(TarefaCreateDTO dto)
        {
            var usuarioId = ObterUsuarioId();
            var usuarioNome = ObterUsuarioNome();
            
            if (string.IsNullOrEmpty(usuarioId))
                throw new UnauthorizedAccessException("Usuário não autenticado");

            var tarefa = new Tarefa
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                UsuarioId = usuarioId
            };
            
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
            
            return new TarefaReadDTO
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Concluida = tarefa.Concluida,
                DataCriacao = tarefa.DataCriacao,
                UsuarioId = tarefa.UsuarioId,
                UsuarioNome = usuarioNome
            };
        }

        public async Task<bool> AtualizarAsync(TarefaUpdateDTO dto)
        {
            var usuarioId = ObterUsuarioId();
            var tarefa = await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == dto.Id && t.UsuarioId == usuarioId);
            if (tarefa == null) return false;

            if (!string.IsNullOrEmpty(dto.Titulo))
                tarefa.Titulo = dto.Titulo;
                
            if (dto.Descricao != null)
                tarefa.Descricao = dto.Descricao;
                
            if (dto.Concluida.HasValue)
                tarefa.Concluida = dto.Concluida.Value;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletarAsync(Guid id)
        {
            var usuarioId = ObterUsuarioId();
            var tarefa = await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);
            if (tarefa == null) return false;

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> ObterEstatisticasAsync()
        {
            var usuarioId = ObterUsuarioId();
            
            var totalTarefas = await _context.Tarefas
                .Where(t => t.UsuarioId == usuarioId)
                .CountAsync();
                
            var tarefasConcluidas = await _context.Tarefas
                .Where(t => t.UsuarioId == usuarioId && t.Concluida)
                .CountAsync();
                
            var tarefasPendentes = totalTarefas - tarefasConcluidas;
            var percentualConcluido = totalTarefas > 0 ? (double)tarefasConcluidas / totalTarefas * 100 : 0;
            
            return new
            {
                TotalTarefas = totalTarefas,
                TarefasConcluidas = tarefasConcluidas,
                TarefasPendentes = tarefasPendentes,
                PercentualConcluido = Math.Round(percentualConcluido, 2)
            };
        }

        public async Task<TarefaReadDTO?> AlternarConclusaoAsync(Guid id)
        {
            var usuarioId = ObterUsuarioId();
            var usuarioNome = ObterUsuarioNome();
            
            var tarefa = await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);
            if (tarefa == null) return null;

            tarefa.Concluida = !tarefa.Concluida;
            await _context.SaveChangesAsync();
            
            return new TarefaReadDTO
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Concluida = tarefa.Concluida,
                DataCriacao = tarefa.DataCriacao,
                UsuarioId = tarefa.UsuarioId,
                UsuarioNome = usuarioNome
            };
        }
    }
}
