using Community_Portal;
using Community_Portal.DTO_s;
using Community_Portal.DTO_s.News;
using Community_Portal.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly AppDbContext _db;


        public NewsController(ILogger<NewsController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        //GET
        [HttpGet()]
        public async Task<ActionResult<List<News>>> GetNews()
        {
            List<News> news = await _db.News.ToListAsync();


            if (news is null) return NotFound(Services.NotFoundMessage("news"));
            return Ok(news);
        }


        //GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNewsById(int id)
        {
            News newsArticle = await _db.News
                .Where(n => n.Id == id)
                .FirstOrDefaultAsync();

            if (newsArticle is null) return NotFound(Services.NotFoundMessage("news"));
            return Ok(newsArticle);
        }


        ////POST
        [HttpPost("{userId}")]
        public async Task<ActionResult<News>> CreateNews(NewsCreateDTO request, int userId)
        {
            News newsArticle = new News { UserId = userId, Title = request.Title, Content = request.Content };

            _db.News.Add(newsArticle);
            _db.SaveChanges();

            if (newsArticle is null) return NotFound(Services.NotFoundMessage("news"));
            return Ok(newsArticle);
        }


        ////UPDATE
        [HttpPut("{id}")]
        public async Task<ActionResult<News>> UpdateNews(int id, [FromBody] NewsUpdateDTO request)
        {
            var newsArticle = await _db.News
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (newsArticle is null) return NotFound(Services.NotFoundMessage("news"));

            newsArticle.Title = request.Title;
            newsArticle.Content = request.Content;

            _db.SaveChanges();

            return Ok(newsArticle);
        }


        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<News>> DeleteNews(int id)
        {
            var newsArticle = await _db.News
                .Where(n => n.Id == id)
                .FirstOrDefaultAsync();

            try
            {
                _db.News.Remove(newsArticle);
                _db.SaveChanges();
            }
            catch (ArgumentNullException ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            if (newsArticle is null) return NotFound(Services.NotFoundMessage("news"));
            return Ok(newsArticle);
        }
    }
}