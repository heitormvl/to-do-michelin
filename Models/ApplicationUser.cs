using Microsoft.AspNetCore.Identity;

namespace to_do_michelin.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? NomeCompleto { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoLogin { get; set; }
        public bool Ativo { get; set; } = true;
        
        // Relacionamento com tarefas
        public virtual ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
    }
} 