using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using AutoMapper;
using Application.Validations;
using FluentValidation;

namespace Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IBaseRepository<Project> _projectRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<Project> _validator;

        public ProjectService(IBaseRepository<Project> projectRepository, IMapper mapper, IValidator<Project> validator)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<ProjectResponse>> GetAllAsync()
        {
            var projects = await _projectRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<ProjectResponse>>(projects);
        }

        public async Task<ProjectResponse?> GetByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return null;

            return _mapper.Map<ProjectResponse>(project);
        }

        public async Task<ProjectResponse> AddAsync(ProjectRequest request)
        {
            var project = _mapper.Map<Project>(request);
            var validationResult = await _validator.ValidateAsync(project);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _projectRepository.AddAsync(project);
            return _mapper.Map<ProjectResponse>(project);
        }

        public async Task<ProjectResponse?> UpdateAsync(Guid id, ProjectRequest request)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return null;
            _mapper.Map(request, project);
            var validationResult = await _validator.ValidateAsync(project);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _projectRepository.UpdateAsync(project);
            return _mapper.Map<ProjectResponse>(project);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return false;
            var validationResult = await _validator.ValidateAsync(project);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _projectRepository.DeleteAsync(project);
            return true;
        }
    }
}