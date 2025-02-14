using Alura.Filmes.API.Data;
using Alura.Filmes.API.Data.Dtos.Filme;
using Alura.Filmes.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Alura.Filmes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Retorna todos os filmes incluidos
    /// </summary>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <param name="nomeCinema"></param>
    /// <returns>IEnumerable</returns>
    /// <response code="200"></response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public IEnumerable<ReadFilmeDto> RecurarFilmes(
            [FromQuery] int skip = 0, 
            [FromQuery] int take = 10, 
            [FromQuery] string? nomeCinema = null
        )
    {
        if (nomeCinema is null)
            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).ToList());

        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes
            .Skip(skip)
            .Take(take)
            .Where(filme => filme.Sessoes
            .Any(sessao => sessao.Cinema.Nome == nomeCinema))
            .ToList());
    }

    /// <summary>
    /// Retorna um filme passando seu Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="404"></response>
    [HttpGet("{id}")]
    public IActionResult RecurarFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme is null)
            return NotFound();

        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);

        return Ok(filme);
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto"> Objeto coms os campos necessários para a criação de um filme.</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso a inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionarFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);

        _context.Filmes.Add(filme);
        _context.SaveChanges();

        return CreatedAtAction(
                    nameof(RecurarFilmePorId),
                    new { id = filme.Id },
                    filme
               );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="filmeDto"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public IActionResult AtualizarFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(x => x.Id == id);

        if (filme is null) 
            return NotFound();

        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="patchFilme"></param>
    /// <returns></returns>
    [HttpPatch]
    public IActionResult AtualizarFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patchFilme)
    {
        var filme = _context.Filmes.FirstOrDefault(x => x.Id == id);

        if (filme is null)
            return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        patchFilme.ApplyTo(filmeParaAtualizar, ModelState);

        if (!TryValidateModel(filmeParaAtualizar))
            return ValidationProblem(ModelState);

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public IActionResult DeletarFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(x => x.Id == id);

        if (filme is null)
            return NotFound();

        _context.Remove(filme);
        _context.SaveChanges();

        return NoContent();
    }
}