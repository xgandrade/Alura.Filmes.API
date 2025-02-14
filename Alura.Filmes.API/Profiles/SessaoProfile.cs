using Alura.Filmes.API.Data.Dtos.Sessao;
using Alura.Filmes.API.Models;
using AutoMapper;

namespace Alura.Filmes.API.Profiles;

public class SessaoProfile : Profile
{
    public SessaoProfile()
    {
        CreateMap<CreateSessaoDto, Sessao>();
        CreateMap<Sessao, ReadSessaoDto>();
    }
}
