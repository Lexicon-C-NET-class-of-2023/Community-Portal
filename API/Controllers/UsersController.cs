using Community_Portal;
using Community_Portal.DTO_s;
using Community_Portal.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly AppDbContext _db;

        public UsersController(ILogger<UsersController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        //TODO HASH PASSWORD

        //GET
        [HttpGet()]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _db.Users.ToListAsync();
            if (users.Count() == 0) LoadMockedDataIfTableIsEmpty();

            return Ok(users);
        }


        //GET (BY ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _db.Users.Where(user => user.Id == id).FirstOrDefaultAsync();

            if (user == null) return NotFound(Services.NotFoundMessage("user"));
            return Ok(user);
        }


        //POST
        [HttpPost()]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserCreateDTO request)
        {
            var user = new User { FirstName = request.FirstName, LastName = request.LastName, Email = request.Email, Password = request.password };

            _db.Add(request);
            await _db.SaveChangesAsync();

            return Ok(user);
        }


        //PUT
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] User value)
        {
            var user = await _db.Users.Where(user => user.Id == id).FirstOrDefaultAsync();

            //TODO--------------------

            //await _db.SaveChangesAsync();

            if (user == null) return NotFound(Services.NotFoundMessage("user"));
            return Ok(user);
        }


        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _db.Users.Where(user => user.Id == id).FirstOrDefaultAsync();

            try
            {
                _db.Remove(user);
                _db.SaveChanges();
            }
            catch (ArgumentNullException ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }


            if (user == null) return NotFound(Services.NotFoundMessage("user"));
            return Ok(user);
        }


        //CREATE MOCKED DATA IF TABLE IS EMPTY
        private void LoadMockedDataIfTableIsEmpty()
        {
            string file = System.IO.File.ReadAllText("./Mocked/users.json");
            var user = JsonSerializer.Deserialize<List<User>>(file);
            _db.AddRange(user);
            _db.SaveChanges();
        }
    }
}
