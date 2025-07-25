using System;

namespace to_do_michelin.Models
{
    public class Tarefa
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Titulo { get; set; }
        public required string Descricao { get; set; }
        public bool Concluida { get; set; } = false;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public required string UsuarioId { get; set; }  // para o EXTRA
    }
}
