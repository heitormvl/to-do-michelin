using System;

public class TarefaReadDTO
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public bool Concluida { get; set; }
    public DateTime DataCriacao { get; set; }
}
