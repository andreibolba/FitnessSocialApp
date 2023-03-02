using AutoMapper;
using API.Models;
using API.Dtos;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Person, PersonDto>();
            CreateMap<Group, GroupDto>();
            CreateMap<InternGroup, InternGroupDto>()
            .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.Intern.PersonId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Intern.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Intern.LastName))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Intern.Username))
            .ForMember(dest => dest.IsChecked, opt => opt.MapFrom(src => true));
            CreateMap<Logging, LoggingDto>();
            CreateMap<Person, InternGroupDto>()
            .ForMember(dest => dest.IsChecked, opt => opt.MapFrom(src => false));
        }
    }
}