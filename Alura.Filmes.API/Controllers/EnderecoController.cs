using Alura.Filmes.API.Data;
using Alura.Filmes.API.Data.Dtos.Endereco;
using Alura.Filmes.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alura.Filmes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnderecoController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public EnderecoController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AdicionarEndereco([FromBody] CreateEnderecoDto enderecoDto)
        {
            Endereco endereco = _mapper.Map<Endereco>(enderecoDto);
            _context.Enderecos.Add(endereco);
            _context.SaveChanges();

            return CreatedAtAction(nameof(RecuperarEnderecosPorId), new { Id = endereco.Id }, enderecoDto);
        }

        [HttpGet]
        public IEnumerable<ReadEnderecoDto> RecuperarEnderecos() => _mapper.Map<List<ReadEnderecoDto>>(_context.Enderecos);

        [HttpGet("{id}")]
        public IActionResult RecuperarEnderecosPorId(int id)
        {
            Endereco endereco = _context.Enderecos.FirstOrDefault(x => x.Id == id);

            if (endereco is null)
                return NotFound();

            ReadEnderecoDto enderecoDto = _mapper.Map<ReadEnderecoDto>(endereco);
            return Ok(enderecoDto);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarEndereco(int id, [FromBody] UpdateEnderecoDto enderecoDto)
        {
            Endereco endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
            if (endereco is null)
                return NotFound();

            _mapper.Map(enderecoDto, endereco);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarEndereco(int id)
        {
            Endereco endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
            if (endereco is null)
                return NotFound();

            _context.Remove(endereco);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
