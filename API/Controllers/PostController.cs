using Community_Portal;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers
{
    public class PostController
    {
        private readonly ILogger<PostController> _logger;
        private readonly AppDbContext _db;


        public PostController(ILogger<PostController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        //GET
        [HttpGet("Posts")]
        [ProducesResponseType(200)]
        public IEnumerable<Post> Get()
        {
            //LoadMockedDataIfTableIsEmpty();
            return _db.Posts.ToList();
        }


        //GET (BY ID)
        [HttpGet("Posts/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public Post Get(int id)
        {
            return _db.Posts.Where(Post => Post.Id == id).FirstOrDefault();
        }


        //POST
        [HttpPost("Posts")]
        [ProducesResponseType(201)]
        public void Post([FromBody] Post value)
        {
            //AUTOINCREMENTS ID AUTOMATICALLY IF NOT SENT
            _db.Add(value);
            _db.SaveChanges();
        }


        //PUT
        [HttpPut("Posts/{id}")]
        [ProducesResponseType(200)]
        public void Put(int id, [FromBody] Post value)
        {
            _db.Remove(_db.Posts.Where(Post => Post.Id == id).FirstOrDefault());
            _db.Add(value);
            _db.SaveChanges();
        }


        //DELETE
        [HttpDelete("Posts/{id}")]
        [ProducesResponseType(200)]
        public void Delete(int id)
        {
            _db.Remove(_db.Posts.Where(Post => Post.Id == id).FirstOrDefault());
            _db.SaveChanges();
        }


        //CREATE MOCKED DATA IF TABLE IS EMPTY
        private void LoadMockedDataIfTableIsEmpty()
        {
            if (_db.Posts.Count() == 0)
            {
                string file = File.ReadAllText("./Mocked/Posts.json");
                var Posts = JsonSerializer.Deserialize<List<Post>>(file);
                _db.AddRange(Posts);
                _db.SaveChanges();
            }
        }
    }
}