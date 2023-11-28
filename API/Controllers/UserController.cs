using Community_Portal;
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
            return _db.Users.Where(User => User.Id == id).FirstOrDefault();
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
            var User = _db.Users.Where(User => User.Id == id).FirstOrDefault();

            //if (value.Content != null && value.Content != "")
            //{
                //User.Content = value.Content;
                //_db.SaveChanges();
            //}
        }


        //DELETE
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public void Delete(int id)
        {
            _db.Remove(_db.Users.Where(User => User.Id == id).FirstOrDefault());
            _db.SaveChanges();
        }


        //CREATE MOCKED DATA IF TABLE IS EMPTY
        private void LoadMockedDataIfTableIsEmpty()
        {
            if (_db.Users.Count() == 0)
            {
                string file = File.ReadAllText("./Mocked/Users.json");
                var Users = JsonSerializer.Deserialize<List<User>>(file);
                _db.AddRange(Users);
                _db.SaveChanges();
            }
        }
    }
}