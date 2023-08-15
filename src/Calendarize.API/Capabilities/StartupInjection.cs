using Calendarize.Core.Dto;
using Calendarize.Core.Services;
using Calendarize.API.Validation;
using Calendarize.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Calendarize.API.Capabilities
{
    public static class StartupInjection
    {
        public static IServiceCollection ConfigureInjection(this IServiceCollection services)
        {
            services
                .AddServices()
                .AddRepositories()
                .AddValidators();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<EventService, EventService>()
                    .AddTransient<LocationService, LocationService>()
                    .AddTransient<UserService, UserService>()
                    .AddTransient<RegistrationService, RegistrationService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<EventRepository, EventRepository>()
                    .AddSingleton<LocationRepository, LocationRepository>()
                    .AddSingleton<UserRepository, UserRepository>()
                    .AddSingleton<RegistrationRepository, RegistrationRepository>();

            return services;
        }

        private static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<EventCreateDto>, EventValidator>()
                    .AddScoped<IValidator<LocationCreateDto>, LocationValidator>()
                    .AddScoped<IValidator<RegistrationCreateDto>, RegistrationValidator>()
                    .AddScoped<IValidator<UserCreateDto>, UserValidator>();

            return services;
        }
    }
}
