using System.ComponentModel.DataAnnotations;

namespace to_do_michelin.DTOs
{
    public class TarefaUpdateDTO
    {
        [Required(ErrorMessage = "ID da tarefa é obrigatório")]
        public required Guid Id { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = "Título deve ter entre 1 e 200 caracteres")]
        public string? Titulo { get; set; }

        [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
        public string? Descricao { get; set; }

        public bool? Concluida { get; set; }
    }
}
