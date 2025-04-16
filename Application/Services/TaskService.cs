using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Repositories;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IBaseRepository<TaskEntity> _taskRepository;
        private readonly IMapper _mapper;

        public TaskService(IBaseRepository<TaskEntity> taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
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
            await _taskRepository.AddAsync(task);
            return _mapper.Map<TaskResponse>(task);
        }

        public async Task<TaskResponse?> UpdateAsync(Guid id, TaskRequest request)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;
            _mapper.Map(request, task);
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