using Alura.Filmes.API.Data.Dtos.Endereco;
using Alura.Filmes.API.Models;
using AutoMapper;

namespace Alura.Filmes.API.Profiles;

public class EnderecoProfile : Profile
{
    public EnderecoProfile()
    {
        CreateMap<CreateEnderecoDto, Endereco>();
        CreateMap<Endereco, ReadEnderecoDto>();
        CreateMap<UpdateEnderecoDto, Endereco>();
    }
}
