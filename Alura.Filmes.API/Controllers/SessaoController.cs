using Alura.Filmes.API.Data;
using Alura.Filmes.API.Data.Dtos.Sessao;
using Alura.Filmes.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alura.Filmes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessaoController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public SessaoController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AdicionarSessao([FromBody] CreateSessaoDto sessaoDto)
    {
        Sessao sessao = _mapper.Map<Sessao>(sessaoDto);
        _context.Sessoes.Add(sessao);
        _context.SaveChanges();

        return CreatedAtAction(nameof(RecuperarSessoesPorId), new { sessao.FilmeId, sessao.CinemaId }, sessaoDto);
    }

    [HttpGet]
    public IEnumerable<ReadSessaoDto> RecuperarSessoes() => _mapper.Map<List<ReadSessaoDto>>(_context.Sessoes);

    [HttpGet("{filmeId}/{cinemaId}")]
    public IActionResult RecuperarSessoesPorId(int filmeId, int cinemaId)
    {
        Sessao? sessao = _context.Sessoes.FirstOrDefault(sessao => sessao.FilmeId == filmeId && sessao.CinemaId == cinemaId);

        if (sessao is null)
            return NotFound();

        ReadSessaoDto sessaoDto = _mapper.Map<ReadSessaoDto>(sessao);

        return Ok(sessaoDto);
    }

    [HttpDelete("{filmeId}/{cinemaId}")]
    public IActionResult DeletarCinema(int filmeId, int cinemaId)
    {
        Sessao? sessao = _context.Sessoes.FirstOrDefault(sessao => sessao.FilmeId == filmeId && sessao.CinemaId == cinemaId);
        if (sessao is null)
            return NotFound();

        _context.Remove(sessao);
        _context.SaveChanges();

        return NoContent();
    }
}
