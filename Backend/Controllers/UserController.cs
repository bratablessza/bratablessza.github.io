using Microsoft.AspNetCore.Mvc;
using project_memu_api_server.Models;
using System.Collections.Generic;
using System.Linq;

namespace project_memu_api_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _context.Users.ToList();
            return users;
        }

        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetUsers), new { id = user.user_id }, user);
        }

        // Implement other CRUD actions (Update, Delete)
        [HttpPut("{id}")]
        public IActionResult UpdateUser(string id, User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.user_id == id);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.user_name = user.user_name;
            existingUser.password = user.password;

            _context.SaveChanges();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.user_id == id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }

    }
}
