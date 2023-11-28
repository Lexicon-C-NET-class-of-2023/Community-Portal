using Community_Portal;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessagesController
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly AppDbContext _db;

        public MessagesController(ILogger<MessagesController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        //GET
        [HttpGet()]
        [ProducesResponseType(200)]
        public IEnumerable<Message> Get()
        {
            LoadMockedDataIfTableIsEmpty();
            return _db.Messages.ToList();
        }


        //GET (BY ID)
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Message Get(int id)
        {
            return _db.Messages.Where(message => message.Id == id).FirstOrDefault();
        }


        //POST
        [HttpPost()]
        [ProducesResponseType(201)]
        public void Post([FromBody] Message value)
        {
            //AUTOINCREMENTS ID AUTOMATICALLY IF NOT SENT
            _db.Add(value);
            _db.SaveChanges();
        }


        //PUT
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        public void Put(int id, [FromBody] Message value)
        {
            var message = _db.Messages.Where(message => message.Id == id).FirstOrDefault();

            if (value.Content != null && value.Content != "")
            {
                message.Content = value.Content;
                _db.SaveChanges();
            }
        }


        //DELETE
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public void Delete(int id)
        {
            _db.Remove(_db.Messages.Where(message => message.Id == id).FirstOrDefault());
            _db.SaveChanges();
        }


        //CREATE MOCKED DATA IF TABLE IS EMPTY
        private void LoadMockedDataIfTableIsEmpty()
        {
            if (_db.Messages.Count() == 0)
            {
                string file = File.ReadAllText("./Mocked/messages.json");
                var messages = JsonSerializer.Deserialize<List<Message>>(file);
                _db.AddRange(messages);
                _db.SaveChanges();
            }
        }
    }
}