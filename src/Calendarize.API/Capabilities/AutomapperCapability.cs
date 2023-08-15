using AutoMapper;
using Calendarize.Core.Dto;
using Calendarize.Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using System;

namespace Calendarize.API.Capabilities
{
    public static class AutomapperCapability
    {
        public static IServiceCollection AddAutomapper(this IServiceCollection services)
        {
            return services.AddAutoMapper(typeof(CustomMappingProfile));
        }
    }

    public class CustomMappingProfile : Profile
    {
        public CustomMappingProfile() 
        {
            CreateMap<string, ObjectId>().ConstructUsing(x => new ObjectId(x));

            CreateMap<EventCreateDto, Event>().ReverseMap();
            CreateMap<EventDto, Event>().ReverseMap();
            
            CreateMap<LocationCreateDto, Location>().ReverseMap();
            CreateMap<LocationDto, Location>().ReverseMap();

            CreateMap<RegistrationDto, Registration>().ReverseMap();
            CreateMap<RegistrationCreateDto, Registration>().ReverseMap();

            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<UserCreateDto, User>().ReverseMap();
        }
    }

}
