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

            CreateMap<TaskRequest, TaskEntity>();
            CreateMap<TaskEntity, TaskResponse>();

            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}