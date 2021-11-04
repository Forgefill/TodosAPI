using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodosAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace TodosAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TodosApiController : ControllerBase
    {
        private readonly DbTodoContext _context;
        public TodosApiController(DbTodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Todo>> GetTodos()
        {
            //List<string> serializedData = new List<string>();
            //foreach(var todo in _context.Todos.ToList())
            //{
            //    serializedData.Add(JsonSerializer.Serialize(todo));
            //}

            return Ok(_context.Todos.ToList());
            //return Ok(serializedData);
        }

        [HttpGet("{id}")]
        public ActionResult<Todo> GetTodo(int id)
        {
            var todo = _context.Todos.FirstOrDefault(c => c.TodoId == id);
            if (todo == null)
            {
                return NotFound();
            }

            return Ok(JsonSerializer.Serialize(todo));
        }

        [HttpPut("id")]
        public IActionResult UpdateTodo(int id, Todo todo)
        {
            if (id != todo.TodoId)
            {
                return BadRequest();
            }
            try
            {
                _context.Update(todo);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Todos.Any(c => c.TodoId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return Ok(todo);
        }

        [HttpPost]
        public ActionResult<Todo> AddTodo(Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                _context.Add(todo);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(ex.Message);
            }
            return CreatedAtAction(nameof(GetTodo), new { id = todo.TodoId }, todo);
        }

        [HttpDelete("{id}")]
        public ActionResult<Todo> DeleteTodo(int id)
        {
            var todo = _context.Todos.FirstOrDefault(c => c.TodoId == id);
            if (todo == null)
            {
                return NotFound();
            }
            try
            {
                _context.Remove(todo);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return Ok();
        }
    }
}
