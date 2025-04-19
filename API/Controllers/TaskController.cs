using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tasks = await _taskService.GetAllAsync();
                return Ok(tasks);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = "Validation failed", errors = ex.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var task = await _taskService.GetByIdAsync(id);
                if (task == null)
                    return NotFound(new { message = "Task not found" });
                return Ok(task);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = "Validation failed", errors = ex.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdTask = await _taskService.AddAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = "Validation failed", errors = ex.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedTask = await _taskService.UpdateAsync(id, request);
                if (updatedTask == null)
                    return NotFound(new { message = "Task not found" });
                return Ok(updatedTask);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = "Validation failed", errors = ex.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _taskService.DeleteAsync(id);
                if (!deleted)
                    return NotFound(new { message = "Task not found" });
                return NoContent();
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { message = "Validation failed", errors = ex.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}