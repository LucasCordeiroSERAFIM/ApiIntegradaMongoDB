using System;
using Api.Data.Collections;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Api.Data
{
    /*classe de Conexão*/
    public class MongoDB
    {
        public IMongoDatabase DB { get; }

        public MongoDB(IConfiguration configuration)
        {
            try
            {
                var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration["ConnectionString"]));/*ConnectionString* do appsetings.json*/
                var client = new MongoClient(settings);
                DB = client.GetDatabase(configuration["NomeBanco"]);//nome dop appsetings.json
                MapClasses();/*mapear as entidades para o banco*/
            }
            catch (Exception ex)
            {
                throw new MongoException("It was not possible to connect to MongoDB", ex);
            }
        }

        private void MapClasses()
        {
            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            if (!BsonClassMap.IsClassMapRegistered(typeof(Infectado)))/*mapa pertencente a classe infectado se nao tiver mapeado mapeie*/
            {
                BsonClassMap.RegisterClassMap<Infectado>(i =>
                {
                    i.AutoMap();
                    i.SetIgnoreExtraElements(true);/*se tiver um dado a mais no bd q nao tem no  infectado ele ignora*/
                });
            }
        }
    }
}