using Api.Data.Collections;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using api.Models;
using System;
namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;


/*Injeção de dependencia no construtor*/
        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            /*Cria o obj para add ao banco*/
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);
            
            return StatusCode(201, "Infectado adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            
            return Ok(infectados);
        }

         [HttpPut]
        public ActionResult EditarInfectado([FromBody] InfectadoDto dto)
        {
            /*Cria o obj para add ao banco*/
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(_=>_.DataNascimento == dto.DataNascimento),Builders<Infectado>.Update.Set("sexo",dto.Sexo));
            
            return StatusCode(201, "Infectado Atualizado com sucesso");
        }


         [HttpDelete]
        public ActionResult DeletarInfectado( DateTime id)
        {
            /*Cria o obj para add ao banco*/
           _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(_=>_.DataNascimento ==id));
            
            return StatusCode(201, "Infectado Atualizado com sucesso");
        }
    }
}
