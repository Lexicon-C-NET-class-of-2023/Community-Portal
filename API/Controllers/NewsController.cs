using Community_Portal;
using Community_Portal.DTO_s;
using Community_Portal.DTO_s.News;
using Community_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
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
        public async Task<ActionResult<List<News>>> GetNews() => Ok(await _db.News.Include(f => f.NewsPosts).ToListAsync());



        //GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNewsById(int id)
        {
            var forum = await _db.News
                .Include(f => f.NewsPosts)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (forum is null) return NotFound(Services.NotFoundMessage("forum"));
            return Ok(forum);
        }


        //POST
        [HttpPost("{userId}")]
        public async Task<ActionResult<News>> CreateNews(NewsCreateDTO request, int userId)
        {
            var newNews = new News { UserId = userId, Title = request.Title, };

            var posts = request.Posts
                .Select(p => new NewsPost { UserId = userId, Content = p.Content, News = newNews })
                .ToList();

            newNews.NewsPosts = posts;

            _db.News.Add(newNews);
            await _db.SaveChangesAsync();

            var response = _db.News.Include(f => f.NewsPosts).ToList();

            return Ok(response);
        }


        //UPDATE
        [HttpPut("{id}")]
        public async Task<ActionResult<News>> UpdateNews(int id, [FromBody] NewsUpdateDTO request)
        {
            //TODO only users with the correct userId should be able to change the title
            var forum = await _db.News
                .Include(f => f.NewsPosts)
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            if (forum != null && request.Title != null && request.Title != "")
            {
                forum.Title = request.Title;
                await _db.SaveChangesAsync();
            }

            if (forum is null) return NotFound(Services.NotFoundMessage("forum"));
            return Ok(forum);
        }


        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<News>> DeleteNews(int id)
        {
            var forum = await _db.News
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            try
            {
                _db.News.Remove(forum);
                _db.SaveChanges();
            }
            catch (ArgumentNullException ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            if (forum is null) return NotFound(Services.NotFoundMessage("forum"));
            return Ok(forum);
        }


        //-----------------------------------------------------POSTS-----------------------------------------------------


        //GET ALL POSTS IN FORUM
        [HttpGet("{NewsId}/posts")]
        public async Task<ActionResult<News>> GetPostsByNewsId(int NewsId)
        {
            var forumPosts = _db.NewsPosts.Where(p => p.NewsId == NewsId);

            if (forumPosts.Count() == 0) return NotFound("No posts on this forum");
            return Ok(forumPosts);
        }


        //POST NEW POST TO FORUM 
        [HttpPost("{NewsId}/posts/{userId}")]
        public async Task<ActionResult<Post>> CreateNewsPost(int NewsId, int userId, [FromBody] PostCreateDto request)
        {
            var newPost = new NewsPost { UserId = userId, NewsId = NewsId, Content = request.Content };

            var posts = _db.NewsPosts.Where(p => p.NewsId == NewsId).ToList(); // redundant (just for returning posts)
            posts.Add(newPost); // redundant (just for returning posts)

            _db.NewsPosts.Add(newPost);
            _db.SaveChanges();

            return Ok(posts);
        }

        //REST OF THE OPERATIONS DO NOT REQUIRE FORUMID AND BELONG IN POSTSCONTROLLER 
    }
}
