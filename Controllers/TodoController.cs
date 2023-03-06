using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet7_webapi_jwt;
using dotnet7_webapi_jwt.Data;

namespace dotnet7_webapi_jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public TodoController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoList>>> GetTodoList()
        {
          if (_context.TodoList == null)
          {
              return NotFound();
          }
            return await _context.TodoList.ToListAsync();
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> GetTodoList(int id)
        {
          if (_context.TodoList == null)
          {
              return NotFound();
          }
            var todoList = await _context.TodoList.FindAsync(id);

            if (todoList == null)
            {
                return NotFound();
            }

            return todoList;
        }

        // PUT: api/Todo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoList(int id, TodoList todoList)
        {
            if (id != todoList.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoListExists(id))
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

        // POST: api/Todo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoList>> PostTodoList(TodoList todoList)
        {
          if (_context.TodoList == null)
          {
              return Problem("Entity set 'ApiDbContext.TodoList'  is null.");
          }
            _context.TodoList.Add(todoList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoList", new { id = todoList.Id }, todoList);
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoList(int id)
        {
            if (_context.TodoList == null)
            {
                return NotFound();
            }
            var todoList = await _context.TodoList.FindAsync(id);
            if (todoList == null)
            {
                return NotFound();
            }

            _context.TodoList.Remove(todoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoListExists(int id)
        {
            return (_context.TodoList?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
