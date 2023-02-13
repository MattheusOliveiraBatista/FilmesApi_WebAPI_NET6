using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController: ControllerBase
{
    private static List<Filme> filmes = new List<Filme>();
    private static int id = 0;

    [HttpPost]
    public void AdicionaFilme([FromBody] Filme filme)
    {
       // if (!string.IsNullOrEmpty(filme.Titulo) && filme.Titulo.Length <= 500 &&
      //      !string.IsNullOrEmpty(filme.Genero) &&
       //     filme.Duracao >= 70)

       
            filme.Id = id++;

            filmes.Add(filme);
            Console.WriteLine(filme.Titulo);
            Console.WriteLine(filme.Duracao);


        

    }
    [HttpGet]
    public IEnumerable<Filme> RecuperaFilmes()
    {
        return filmes;
    }


    //A interrogação siginifica que ele é nulo
    [HttpGet("{id}")]
    public Filme? RecuperaFilmes(int id)
    {
        return filmes.FirstOrDefault(filme => filme.Id== id);
    }


}
