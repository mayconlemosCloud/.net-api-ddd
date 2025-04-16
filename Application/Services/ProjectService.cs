using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class ProjectService
    {
        private readonly IBaseRepository<Project> _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(IBaseRepository<Project> projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectDTO>>(projects);
        }

        public async Task<ProjectDTO?> GetProjectByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            return _mapper.Map<ProjectDTO?>(project);
        }

        public async Task AddProjectAsync(CreateProjectDTO createProjectDto)
        {
            var project = _mapper.Map<Project>(createProjectDto);
            await _projectRepository.AddAsync(project);
        }

        public async Task UpdateProjectAsync(Guid id, UpdateProjectDTO updateProjectDto)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
            {
                throw new KeyNotFoundException("Project not found");
            }

            _mapper.Map(updateProjectDto, project);
            await _projectRepository.UpdateAsync(project);
        }

        public async Task DeleteProjectAsync(Guid id)
        {
            await _projectRepository.DeleteAsync(id);
        }
    }
}