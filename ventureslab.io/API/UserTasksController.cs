using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserTaskApi.Data;
using UserTaskApi.Models;

namespace UserTaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTasksController : ControllerBase
    {
        private readonly UserTaskContext _context;

        public UserTasksController(UserTaskContext context)
        {
            _context = context;
        }

        // GET: api/UserTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetUserTasks()
        {
            return await _context.Tasks.Include(t => t.User).ToListAsync();
        }

        // GET: api/UserTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModel>> GetUserTask(int id)
        {
            var task = await _context.Tasks.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // POST: api/UserTasks
        [HttpPost]
        public async Task<ActionResult<TaskModel>> PostUserTask(TaskModel task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserTask), new { id = task.Id }, task);
        }

        // PUT: api/UserTasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTask(int id, TaskModel task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/UserTasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/UserTasks/Today
        [HttpGet("Today")]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTodayTasks()
        {
            var today = DateTime.Today;
            var tasks = await _context.Tasks
                .Where(task => task.Start.Date == today)
                .Include(t => t.User)
                .ToListAsync();

            return tasks;
        }

        // GET: api/UserTasks/Upcoming
        [HttpGet("Upcoming")]
        public async Task<ActionResult<IEnumerable<IGrouping<DateTime, TaskModel>>>> GetUpcomingTasks()
        {
            var today = DateTime.Today;
            var tasks = await _context.Tasks
                .Where(task => task.Start.Date > today)
                .Include(t => t.User)
                .ToListAsync();

            var groupedTasks = tasks.GroupBy(task => task.Start.Date)
                .OrderBy(group => group.Key)
                .ToList();

            return Ok(groupedTasks);
        }

        private bool UserTaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
