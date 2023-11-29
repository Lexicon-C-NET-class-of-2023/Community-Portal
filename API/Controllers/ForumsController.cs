using Community_Portal;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ForumsController
    {
        private readonly ILogger<ForumsController> _logger;
        private readonly AppDbContext _db;


        public ForumsController(ILogger<ForumsController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        //GET
        [HttpGet()]
        [ProducesResponseType(200)]
        public IEnumerable<Forum> Get()
        {
            //LoadMockedDataIfTableIsEmpty();
            return _db.Forums.ToList();
        }


        //GET (BY ID)
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public Forum Get(int id)
        {
            return _db.Forums.Where(Forum => Forum.Id == id).FirstOrDefault();
        }


        //POST
        [HttpPost("{userId}")]
        [ProducesResponseType(201)]
        public void Post([FromBody] Forum value)
        {
            //AUTOINCREMENTS ID AUTOMATICALLY IF NOT SENT
            _db.Add(value);
            _db.SaveChanges();
        }


        //PUT
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        public void Put(int id, [FromBody] Forum value)
        {
            //NOT DONE YET
            
        }


        //DELETE
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public void Delete(int id)
        {
            _db.Remove(_db.Forums.Where(Forum => Forum.Id == id).FirstOrDefault());
            _db.SaveChanges();
        }


        //CREATE MOCKED DATA IF TABLE IS EMPTY
        private void LoadMockedDataIfTableIsEmpty()
        {
            if (_db.Forums.Count() == 0)
            {
                string file = File.ReadAllText("./Mocked/Forums.json");
                var Forums = JsonSerializer.Deserialize<List<Forum>>(file);
                _db.AddRange(Forums);
                _db.SaveChanges();
            }
        }
    }
}