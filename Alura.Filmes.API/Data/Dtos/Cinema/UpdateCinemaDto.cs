using System.ComponentModel.DataAnnotations;

namespace Alura.Filmes.API.Data.Dtos.Cinema;

public class UpdateCinemaDto
{
    [Required(ErrorMessage = "O campo nome é obrigatório.")]
    public string Nome { get; set; }
}
