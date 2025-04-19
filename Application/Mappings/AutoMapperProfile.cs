using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProjectRequest, Project>();
            CreateMap<Project, ProjectResponse>();

            CreateMap<TaskRequest, TaskEntity>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => (src.Comments != null)
                    ? src.Comments.Select(content => new Comment { Content = content, UserId = src.UserId }).ToList()
                    : new List<Comment>()));
            CreateMap<TaskEntity, TaskResponse>()
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
            CreateMap<Comment, CommentResponse>();
        }
    }
}