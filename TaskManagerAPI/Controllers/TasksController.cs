using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _repository;

        public TasksController(ITaskRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll()
        {
            var tasks = await _repository.GetAllAsync();

            var dtoList = tasks.Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt
            });

            return Ok(dtoList);
        }

        // GET: api/Tasks/1
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponseDto>> GetById(int id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null)
                return NotFound(new { message = $"Task with id {id} not found" });

            var dto = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };

            return Ok(dto);
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> Create(TaskCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = new TaskItem
            {
                Title = createDto.Title,
                IsCompleted = createDto.IsCompleted,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(task);

            var responseDto = new TaskResponseDto
            {
                Id = created.Id,
                Title = created.Title,
                IsCompleted = created.IsCompleted,
                CreatedAt = created.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, responseDto);
        }

        // PUT: api/Tasks/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = new TaskItem
            {
                Id = id,
                Title = updateDto.Title,
                IsCompleted = updateDto.IsCompleted
            };

            var success = await _repository.UpdateAsync(task);
            if (!success)
                return NotFound(new { message = $"Task with id {id} not found" });

            return NoContent();
        }

        // DELETE: api/Tasks/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]   // 👈 ही नवीन लाईन — फक्त Admin लाच परवानगी
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = $"Task with id {id} not found" });

            return NoContent();
        }
    }
}