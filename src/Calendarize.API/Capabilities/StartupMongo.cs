using Calendarize.Core.Entities;
using Calendarize.Infrastructure.Mongo;
using Calendarize.Infrastructure.Mongo.Indexes;
using Calendarize.Infrastructure.Providers;
using Calendarize.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calendarize.API.Capabilities
{
    public static class StartupMongo
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration config)
        {
            var configuration = config.GetSection("Mongo").Get<MongoConfig>();
             
            return services
                .AddSingleton<IEventCollectionProvider>(_ =>
                    new MongoCollectionProvider(MongoFactory.Create(configuration), configuration.Collections.Events))
                .AddSingleton<ILocationCollectionProvider>(_ =>
                    new MongoCollectionProvider(MongoFactory.Create(configuration), configuration.Collections.Locations))
                .AddSingleton<IRegistrationCollectionProvider>(_ =>
                    new MongoCollectionProvider(MongoFactory.Create(configuration), configuration.Collections.Registrations))
                .AddSingleton<IUserCollectionProvider>(_ =>
                    new MongoCollectionProvider(MongoFactory.Create(configuration), configuration.Collections.Users))
                .AddMongoIndexConfigs();
        }

        public async static Task AddMongoInitDataAsync(this IServiceProvider services)
        {
            var locationRepository = services.GetService<LocationRepository>();
            var defaultLocations = new List<Location>
            {
                new()
                {
                    Id = new ObjectId("64cff2a8de7286f37fcd0799"),
                    Name = "Žalgirio arena",
                    City = "Kaunas",
                    Address = "Karaliaus Mindaugo pr. 50",
                    Longtitude = 54.8904569M,
                    Latitude = 23.9145678M,
                    CreatedAt = new DateTime(2023, 08, 01, 00, 00, 00),
                    UpdatedAt = new DateTime(2023, 08, 01, 00, 00, 00),
                },

                new()
                {
                    Id = new ObjectId("64c825dc973ee818b42e2ccd"),
                    Name = "Savanoriai",
                    City = "Kaunas",
                    Address = "Savanorių pr. 168",
                    Longtitude = 54.9073944M,
                    Latitude = 23.9259834M,
                    CreatedAt = new DateTime(2023, 08, 01, 00, 00, 00),
                    UpdatedAt = new DateTime(2023, 08, 01, 00, 00, 00),
                }
            };

            foreach (var location in defaultLocations)
            {
                await locationRepository.UpsertAsync(location);
            }

        }

        private static IServiceCollection AddMongoIndexConfigs(this IServiceCollection services)
        {
            var type = typeof(IMongoIndexConfig);

            var configTypes = type.Assembly
                .GetTypes()
                .Where(x => type.IsAssignableFrom(x)
                            && !x.IsInterface
                            && !x.IsAbstract)
                .ToList();

            configTypes.ForEach(x => services.AddTransient(x));

            using (var sp = services.BuildServiceProvider())
            {
                configTypes
                    .Select(x => sp.GetService(x))
                    .OfType<IMongoIndexConfig>()
                    .ToList()
                    .ForEach(x => x.CreateIndexes());
            }

            return services;
        }
    }
}
