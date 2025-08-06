using System.ComponentModel.DataAnnotations;

namespace to_do_michelin.DTOs
{
    public class TarefaCreateDTO
    {
        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Título deve ter entre 1 e 200 caracteres")]
        public required string Titulo { get; set; }

        [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
        public string? Descricao { get; set; }
    }
}
