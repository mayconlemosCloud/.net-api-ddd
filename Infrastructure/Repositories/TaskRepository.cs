using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TaskRepository : IBaseRepository<TaskEntity>
    {
        private readonly TaskManagementDbContext _context;

        public TaskRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<TaskEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            return await _context.Tasks.
                 Include(t => t.User).
                 Include(t => t.Project).
                 Include(t => t.Comments).
                 ToListAsync();

        }

        public async Task AddAsync(TaskEntity entity)
        {
            await _context.Tasks.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskEntity entity)
        {
            _context.Tasks.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskEntity entity)
        {
            var task = await _context.Tasks.FindAsync(entity.Id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}