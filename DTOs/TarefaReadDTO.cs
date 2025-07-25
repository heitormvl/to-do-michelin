using System;

namespace to_do_michelin.DTOs
{
    public class TarefaReadDTO
    {
        public Guid Id { get; set; }
        public required string Titulo { get; set; }
        public required string Descricao { get; set; }
        public bool Concluida { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
