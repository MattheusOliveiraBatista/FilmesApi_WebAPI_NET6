using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;




[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private static List<Filme> filmes = new List<Filme>();
    private static int id = 0;

    [HttpPost]
    public IActionResult AdicionaFilme([FromBody] Filme filme )
    {
        // if (!string.IsNullOrEmpty(filme.Titulo) && filme.Titulo.Length <= 500 &&
        //      !string.IsNullOrEmpty(filme.Genero) &&
        //     filme.Duracao >= 70)


        filme.Id = id++;
        filmes.Add(filme);
        return CreatedAtAction(nameof(RecuperaFilmePorId), 
                        new { id = filme.Id },
                        filme);
    }



    [HttpGet]
    public IEnumerable<Filme> RecuperaFilmes([FromQuery] int skip = 0, int take = 50)
    //public IEnumerable<Filme> RecuperaFilmes()
    {
        //skip = quantos ele quer pular
        //take = quantos ele quer pegar
        return filmes.Skip(skip).Take(take);
    }


    //A interrogação siginifica que ele é nulo
    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id)
    {
        var filme =  filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        return Ok(filme);
    }

}
