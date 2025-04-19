using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services.Interfaces;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(users);
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
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                return Ok(user);
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
        public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdUser = await _userService.AddAsync(request);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
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
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedUser = await _userService.UpdateAsync(id, request);
                if (updatedUser == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                return Ok(updatedUser);
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
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var isDeleted = await _userService.DeleteAsync(id);
                if (!isDeleted)
                {
                    return NotFound(new { message = "User not found" });
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