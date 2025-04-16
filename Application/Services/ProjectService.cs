using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using AutoMapper;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IBaseRepository<Project> _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(IBaseRepository<Project> projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectResponse>> GetAllAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectResponse>>(projects);
        }

        public async Task<ProjectResponse?> GetByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            return project == null ? null : _mapper.Map<ProjectResponse>(project);
        }

        public async Task<ProjectResponse> AddAsync(ProjectRequest request)
        {
            var project = _mapper.Map<Project>(request);
            await _projectRepository.AddAsync(project);
            return _mapper.Map<ProjectResponse>(project);
        }

        public async Task<ProjectResponse?> UpdateAsync(Guid id, ProjectRequest request)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return null;
            _mapper.Map(request, project);
            await _projectRepository.UpdateAsync(project);
            return _mapper.Map<ProjectResponse>(project);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return false;
            await _projectRepository.DeleteAsync(project);
            return true;
        }
    }
}