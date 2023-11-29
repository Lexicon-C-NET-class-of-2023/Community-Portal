using Community_Portal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController
    {
        private readonly ILogger<UsersController> _logger;
        private readonly AppDbContext _db;

        public UsersController(ILogger<UsersController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        //GET
        [HttpGet()]
        [ProducesResponseType(200)]
        public IEnumerable<User> Get()
        {
            LoadMockedDataIfTableIsEmpty();
            return _db.Users.ToList();
        }


        //GET (BY ID)
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public User Get(int id)
        {
            return _db.Users.Where(user => user.Id == id).FirstOrDefault();
        }


        //POST
        [HttpPost()]
        [ProducesResponseType(201)]
        public void Post([FromBody] User value)
        {
            //AUTOINCREMENTS ID AUTOMATICALLY IF NOT SENT
            _db.Add(value);
            _db.SaveChanges();
        }


        //PUT
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        public void Put(int id, [FromBody] User value)
        {
            var User = _db.Users.Where(user => user.Id == id).FirstOrDefault();

           //NOT DONE YET
        }


        //DELETE
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public void Delete(int id)
        {
            _db.Remove(_db.Users.Where(user => user.Id == id).FirstOrDefault());
            _db.SaveChanges();
        }


        //CREATE MOCKED DATA IF TABLE IS EMPTY
        private void LoadMockedDataIfTableIsEmpty()
        {
            if (_db.Users.Count() == 0)
            {
                string file = File.ReadAllText("./Mocked/users.json");
                var user = JsonSerializer.Deserialize<List<User>>(file);
                _db.AddRange(user);
                _db.SaveChanges();
            }
        }
    }
}
