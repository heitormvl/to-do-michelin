using System;

namespace to_do_michelin.DTOs
{
    public class TarefaUpdateDTO
    {
        public Guid Id { get; set; }
        public required string Titulo { get; set; }
        public required string Descricao { get; set; }
        public bool Concluida { get; set; }
    }
}
