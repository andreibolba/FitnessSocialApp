using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using API.Dtos;
using API.Models;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Person, PersonDto>();
            CreateMap<GetPeopleInGroupMeeting, PersonDto>();
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
            CreateMap<InternGroup, PersonDto>()
            .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.Intern.PersonId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Intern.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Intern.LastName))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Intern.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Intern.Email))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Intern.Status))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Intern.BirthDate));
            CreateMap<Test, TestDto>();
            CreateMap<Question, QuestionDto>();
            CreateMap<TestGroupIntern, TestGroupInternDto>();
            CreateMap<Meeting, MeetingDto>();
            CreateMap<QuestionSolution, Answer>();
            CreateMap<Post,PostDto>();


            CreateMap<PersonDto, Person>()
                .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => false));
            CreateMap<GroupDto, Group>()
                .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => false));
            CreateMap<InternGroupDto, InternGroup>()
                .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => false));
            CreateMap<LoggingDto, Logging>()
                .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => false));
            CreateMap<TestDto, Test>()
                .ForMember(dest => dest.CanBeEdited, opt => opt.MapFrom(src => src.CanBeEdited==null? true : src.CanBeEdited.Value))
                .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => false));
            CreateMap<QuestionDto, Question>()
                .ForMember(dest => dest.CanBeEdited, opt => opt.MapFrom(src => src.CanBeEdited==null? true : src.CanBeEdited.Value))
                .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => false));
            CreateMap<MeetingDto, Meeting>()
                .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => false));
            CreateMap<PostDto, Post>()
                .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => false));
        }
    }
}