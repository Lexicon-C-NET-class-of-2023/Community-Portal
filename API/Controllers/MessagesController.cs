using Community_Portal;
using Community_Portal.DTO_s.Message;
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
    public class MessagesController : ControllerBase
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
        public async Task<ActionResult<List<Message>>> GetMessages(int userId)
        {
            List<Message> messages = new List<Message>();

            if (userId != 0)
            {
                messages = await _db.Messages
                    .Where(m => (m.UserId == userId) || (m.Recipient == userId))
                    .OrderByDescending(m => m.Created)
                    .GroupBy(m => new { m.SenderName, m.RecipientName })
                    .Select(m => m.First())
                    .ToListAsync();
            }

            else messages = await _db.Messages.ToListAsync();
            if (messages.Count() == 0) LoadMockedDataIfTableIsEmpty();

            return Ok(messages);
        }


        //GET (BY ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessageById(int id)
        {
            var message = await _db.Messages
                .Where(message => message.Id == id)
                .FirstOrDefaultAsync();

            if (message == null) return NotFound(Services.NotFoundMessage("message"));
            return Ok(message);
        }


        //POST
        [HttpPost("{userId}")]
        public async Task<ActionResult<Message>> CreateMessage(int userId, [FromBody] MessageCreateDTO request)
        {
            Message message = new Message { UserId = userId, Recipient = request.Recipient, Content = request.Content };

            var sender = _db.Users.Where(u => u.Id == userId).Select(u => new { u.FirstName, u.LastName }).FirstOrDefault();
            var reciever = _db.Users.Where(u => u.Id == request.Recipient).Select(u => new { u.FirstName, u.LastName }).FirstOrDefault();

            message.SenderName = $"{sender.FirstName} {sender.LastName}";
            message.RecipientName = $"{reciever.FirstName} {reciever.LastName}";

            _db.Add(message);
            await _db.SaveChangesAsync();

            return Ok(message);
        }


        //PUT
        [HttpPut("{id}")]
        public async Task<ActionResult<Message>> UpdateMessage(int id, [FromBody] MessageUpdateDTO request)
        {
            //TODO only users with the correct userId should be able to change the content
            var message = _db.Messages
                .Where(message => message.Id == id)
                .FirstOrDefault();

            if (message != null && request.Content != null && request.Content != "")
            {
                message.Content = request.Content;
                await _db.SaveChangesAsync();
            }

            if (message == null) return NotFound(Services.NotFoundMessage("message"));
            return Ok(message);
        }


        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<Boolean>> DeleteMessage(int id)
        {
            var message = _db.Messages
                .Where(message => message.Id == id)
                .FirstOrDefault();

            try
            {
                _db.Messages.Remove(message);
                await _db.SaveChangesAsync();
            }
            catch (ArgumentNullException ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }


            if (message == null) return NotFound(Services.NotFoundMessage("message"));
            return Ok(true);
        }


        //-------------------------------------------------------------------------------------


        //GET (BY USERID & RECIPIENTID)
        [HttpGet("{userId}/message/{recipientId}")]
        public async Task<ActionResult<List<Message>>> GetPrivateConversation(int userId, int recipientId)
        {
            List<Message> messages = new List<Message>();

            Console.WriteLine(recipientId);
            messages = await _db.Messages
                .Where(n => (n.UserId == userId) && (n.Recipient == recipientId) || (n.UserId == recipientId) && (n.Recipient == userId))
                .ToListAsync();

            return Ok(messages);
        }






        //CREATE MOCKED DATA IF TABLE IS EMPTY
        private async void LoadMockedDataIfTableIsEmpty()
        {
            string file = System.IO.File.ReadAllText("./Mocked/messages.json");
            var messages = JsonSerializer.Deserialize<List<Message>>(file);
            _db.AddRange(messages);
            await _db.SaveChangesAsync();
        }
    }
}