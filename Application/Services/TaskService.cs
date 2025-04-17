using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Repositories;
using AutoMapper;
using Domain.Entities;
using Application.Validations;
using FluentValidation;

namespace Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IBaseRepository<TaskEntity> _taskRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<TaskEntity> _validator;

        public TaskService(IBaseRepository<TaskEntity> taskRepository, IMapper mapper, IValidator<TaskEntity> validator)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IEnumerable<TaskResponse>> GetAllAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskResponse>>(tasks);
        }

        public async Task<TaskResponse?> GetByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task == null ? null : _mapper.Map<TaskResponse>(task);
        }

        public async Task<TaskResponse> AddAsync(TaskRequest request)
        {
            var task = _mapper.Map<TaskEntity>(request);
            var validationResult = await _validator.ValidateAsync(task);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _taskRepository.AddAsync(task);
            return _mapper.Map<TaskResponse>(task);
        }

        public async Task<TaskResponse?> UpdateAsync(Guid id, TaskRequest request)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;
            _mapper.Map(request, task);
            var validationResult = await _validator.ValidateAsync(task);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            await _taskRepository.UpdateAsync(task);
            return _mapper.Map<TaskResponse>(task);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return false;
            await _taskRepository.DeleteAsync(task);
            return true;
        }
    }
}