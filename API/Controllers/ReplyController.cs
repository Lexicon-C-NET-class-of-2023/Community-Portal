using Community_Portal;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RepliesController
    {
        private readonly ILogger<RepliesController> _logger;
        private readonly AppDbContext _db;

        public RepliesController(ILogger<RepliesController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        //GET
        [HttpGet()]
        [ProducesResponseType(200)]
        public IEnumerable<Reply> Get()
        {
            LoadMockedDataIfTableIsEmpty();
            return _db.Replies.ToList();
        }


        //GET (BY ID)
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Reply Get(int id)
        {
            return _db.Replies.Where(Reply => Reply.Id == id).FirstOrDefault();
        }


        //POST
        [HttpPost()]
        [ProducesResponseType(201)]
        public void Post([FromBody] Reply value)
        {
            //AUTOINCREMENTS ID AUTOMATICALLY IF NOT SENT
            _db.Add(value);
            _db.SaveChanges();
        }


        //PUT
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        public void Put(int id, [FromBody] Reply value)
        {
            var Reply = _db.Replies.Where(Reply => Reply.Id == id).FirstOrDefault();

            //if (value.Content != null && value.Content != "")
            //{
                //Reply.Content = value.Content;
                //_db.SaveChanges();
            //}
        }


        //DELETE
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public void Delete(int id)
        {
            _db.Remove(_db.Replies.Where(Reply => Reply.Id == id).FirstOrDefault());
            _db.SaveChanges();
        }


        //CREATE MOCKED DATA IF TABLE IS EMPTY
        private void LoadMockedDataIfTableIsEmpty()
        {
            if (_db.Replies.Count() == 0)
            {
                string file = File.ReadAllText("./Mocked/Replies.json");
                var Replies = JsonSerializer.Deserialize<List<Reply>>(file);
                _db.AddRange(Replies);
                _db.SaveChanges();
            }
        }
    }
}