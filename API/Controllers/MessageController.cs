using Community_Portal;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers
{
    public class MessageController
    {
        private readonly ILogger<MessageController> _logger;
        private readonly AppDbContext _db;


        public MessageController(ILogger<MessageController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet("messages")]
        [ProducesResponseType(200)]

        public IEnumerable<Message> Get()
        {
            LoadMockedDataIfTableIsEmpty();
            var messages = _db.Messages.ToList();
            return messages;
        }



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