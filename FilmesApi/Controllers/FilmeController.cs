﻿using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;




[ApiController]
[Route("[controller]")]
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
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    /// 
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto )
    {
        // if (!string.IsNullOrEmpty(filme.Titulo) && filme.Titulo.Length <= 500 &&
        //      !string.IsNullOrEmpty(filme.Genero) &&
        //     filme.Duracao >= 70)

        Filme filme = _mapper.Map<Filme>( filmeDto );   

        _context.Filmes.Add(filme);
        _context.SaveChanges();
        //filme.Id = id++;
        //filmes.Add(filme);
        return CreatedAtAction(nameof(RecuperaFilmePorId), 
                        new { id = filme.Id },
                        filme);
    }



    [HttpGet]
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, int take = 50)
    //public IEnumerable<Filme> RecuperaFilmes()
    {
        //skip = quantos ele quer pular
        //take = quantos ele quer pegar
        //return filmes.Skip(skip).Take(take);
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
    }


    //A interrogação siginifica que ele é nulo
    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id)
    {
        //var filme =  filmes.FirstOrDefault(filme => filme.Id == id);
        var filme = _context.Filmes
                .FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        var filmeDto = _mapper.Map<ReadFilmeDto>( filme);
        return Ok(filmeDto);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id,
        [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);
        
        if(filme == null) return NotFound();
       
        _mapper.Map(filmeDto,filme );
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizaFilmeParcial(int id,
        JsonPatchDocument<UpdateFilmeDto> patchDocument)
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);

        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        patchDocument.ApplyTo(filmeParaAtualizar, ModelState);

        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }


        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();

        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(
            filme => filme.Id == id);

        if (filme == null) return NotFound();
        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }

}
