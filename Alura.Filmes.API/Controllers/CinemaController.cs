using Alura.Filmes.API.Data;
using Alura.Filmes.API.Data.Dtos.Cinema;
using Alura.Filmes.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alura.Filmes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CinemaController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public CinemaController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AdicionarCinema([FromBody] CreateCinemaDto cinemaDto)
    {
        Cinema cinema = _mapper.Map<Cinema>(cinemaDto);
        _context.Cinemas.Add(cinema);
        _context.SaveChanges();

        return CreatedAtAction(nameof(RecuperarCinemasPorId), new { Id = cinema.Id }, cinemaDto);
    }

    [HttpGet]
    public IEnumerable<ReadCinemaDto> RecuperarCinemas([FromQuery] int? enderecoId = null) 
    {
        if (enderecoId is null)
            return _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.ToList());

        // variavel que criei usando uma sintaxe mais adequada do EF
        var cinemaEntity = _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.AsNoTracking().Where(cinema => cinema.EnderecoId == enderecoId).Include(x => x.Endereco).ToList());

        // variavel de exemplo do curso
        // var cinemaCurso = _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.FromSqlRaw($"SELECT Id, Nome, EnderecoId FROM cinemas where cinemas.EnderecoId = {enderecoId}").ToList());

        // jeito correto para não haver SQL Injection
        var cinemaSQL = _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.FromSqlRaw("SELECT Id, Nome, EnderecoId FROM cinemas where cinemas.EnderecoId = {0}", enderecoId).ToList());

        return cinemaSQL;
    }

    [HttpGet("{id}")]
    public IActionResult RecuperarCinemasPorId(int id)
    {
        Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);

        if (cinema is null)
            return NotFound();

        ReadCinemaDto cinemaDto = _mapper.Map<ReadCinemaDto>(cinema);
        return Ok(cinemaDto);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizarCinema(int id, [FromBody] UpdateCinemaDto cinemaDto)
    {
        Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
        if (cinema is null)
            return NotFound();

        _mapper.Map(cinemaDto, cinema);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletarCinema(int id)
    {
        Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
        if (cinema is null)
            return NotFound();

        _context.Remove(cinema);
        _context.SaveChanges();
        return NoContent();
    }
}
