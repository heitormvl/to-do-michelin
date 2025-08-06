using System;

namespace to_do_michelin.Models
{
    public class Tarefa
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Titulo { get; set; }
        public string? Descricao { get; set; }
        public bool Concluida { get; set; } = false;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        
        // Relacionamento com Identity
        public string? UsuarioId { get; set; }
        public virtual ApplicationUser? Usuario { get; set; }
    }
}
