using Alura.Filmes.API.Data.Dtos.Endereco;
using Alura.Filmes.API.Data.Dtos.Sessao;

namespace Alura.Filmes.API.Data.Dtos.Cinema;

public class ReadCinemaDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public ReadEnderecoDto Endereco { get; set; }
    public ICollection<ReadSessaoDto> Sessoes { get; set; }
}
