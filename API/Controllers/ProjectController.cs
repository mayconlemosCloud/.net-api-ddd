using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services.Interfaces;
using System.Threading.Tasks;
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                var projects = await _projectService.GetAllAsync();
                return Ok(projects);
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
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            try
            {
                var project = await _projectService.GetByIdAsync(id);
                if (project == null)
                {
                    return NotFound(new { message = "Project not found" });
                }
                return Ok(project);
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
        public async Task<IActionResult> CreateProject([FromBody] ProjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdProject = await _projectService.AddAsync(request);
                return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.Id }, createdProject);
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
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] ProjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedProject = await _projectService.UpdateAsync(id, request);
                if (updatedProject == null)
                {
                    return NotFound(new { message = "Project not found" });
                }
                return Ok(updatedProject);
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
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            try
            {
                var isDeleted = await _projectService.DeleteAsync(id);
                if (!isDeleted)
                {
                    return NotFound(new { message = "Project not found" });
                }
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