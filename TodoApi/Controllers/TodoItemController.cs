using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/todoList/1/todoItem
    [HttpGet("todolists/{id}/todoitems") ]
    public async Task<ActionResult<IList<TodoList>>> GetTodoLists(long id)
    {
      if (_context.TodoList == null)
      {
        return NotFound();
      }
      var todoList = _context.TodoList
          .Include(t => t.Items)
          .FirstOrDefault(t => t.Id == id);
      return Ok(todoList.Items);
    }
    [HttpGet("todolists/{id}/todoitems/{idItem}")]
        public async Task<ActionResult<IList<TodoList>>> GetTodoLists(long id,long IdItem)
        {
            TodoList todoList = new TodoList();
            TodoItem item = new TodoItem();
            if (_context.TodoList == null)
            {
                return NotFound();
            }
            todoList = _context.TodoList
                .Include(t => t.Items)
                .FirstOrDefault(t => t.Id == id);
                item = todoList.Items.FirstOrDefault(t => t.Id == IdItem);
            if (item == null)
            {
                return NotFound();
            }


            return Ok(item);
        }
        // GET: api/todoItems
        [HttpGet("todoitems")]
        public async Task<ActionResult<IList<TodoList>>> GetTodoLists()
        {
            if (_context.TodoItem == null)
            {
                return NotFound();
            }

            return Ok(await _context.TodoItem.ToListAsync());
        }


        // PUT: api/todoitems/5
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("todoitems/{id}")]
        public async Task<ActionResult> PutTodoList(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
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

        // POST: api/todoitems
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("todoitems")]
        public async Task<ActionResult<TodoList>> PostTodoList(TodoItem todoItem)
        {
            if (_context.TodoItem == null)
            {
                return Problem("Entity set 'TodoContext.TodoItem'  is null.");
            }
            _context.TodoItem.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/todoitems/5
        [HttpDelete("todoitems/{id}")]
        public async Task<ActionResult> DeleteTodoList(long id)
        {
            if (_context.TodoItem == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItem.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return (_context.TodoItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
