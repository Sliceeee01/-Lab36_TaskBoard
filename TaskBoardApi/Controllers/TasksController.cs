using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBoardApi.Models;
using TaskBoardApi.Data;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase {
    private readonly AppDbContext _db;
    public TasksController(AppDbContext db) {
        _db = db;
    }
    [HttpGet]
    public async Task<ActionResult<List<TaskItem>>> GetAll() {
        var tasks = await _db.Tasks
        .OrderBy(t => t.IsCompleted)
        .ThenByDescending(t => t.CreateAt)
        .ToListAsync();
        return Ok(tasks);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetById(int id) {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null) {
            return NotFound(new { message = $"Задача с id= {id} не найдена" });
        }
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create([FromBody] TaskItem task) {
        if (string.IsNullOrWhiteSpace(task.Title)) {
            return BadRequest(new { message = "Название задачи не может быть пустым" });
        }
        task.id = 0;
        task.IsCompleted = false;
        task.CreateAt = DateTime.UtcNow;
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = task.id }, task);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskItem>> Update(int id,
    [FromBody] TaskItem update) {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null) {
            return NotFound(new { message = $"Задача с id={id} не найдена" });
        }
        if (string.IsNullOrWhiteSpace(update.Title)) {
            return BadRequest(new { message = "Название задачи не может быть пустым" });
        }
        task.Title = update.Title;
            task.Description = update.Description;
            task.IsCompleted = update.IsCompleted;
            await _db.SaveChangesAsync();
            return Ok(task);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete( int id) {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null) {
            return NotFound(new { message = $"Задача с id = {id} не найдена" });
        }
        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}