using Community_Portal;
using Community_Portal.DTO_s;
using Community_Portal.DTO_s.Forum;
using Community_Portal.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class ForumsController : ControllerBase
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
        public async Task<ActionResult<List<Forum>>> GetForums() => Ok(await _db.Forums.Include(f => f.ForumPosts).ToListAsync());



        //GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Forum>> GetForumById(int id)
        {
            var forum = await _db.Forums
                .Include(f => f.ForumPosts)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (forum is null) return NotFound(Services.NotFoundMessage("forum"));
            return Ok(forum);
        }


        //POST
        [HttpPost("{userId}")]
        public async Task<ActionResult<Forum>> CreateForum(ForumCreateDTO request, int userId)
        {
            var newForum = new Forum { UserId = userId, Title = request.Title, };
            var posts = request.Posts
                .Select(p => new ForumPost { UserId = userId, Content = p.Content, Forum = newForum })
                .ToList();

            newForum.ForumPosts = posts;

            _db.Forums.Add(newForum);
            await _db.SaveChangesAsync();

            var response = _db.Forums.Include(f => f.ForumPosts).ToList();

            return Ok(response);
        }


        //UPDATE
        [HttpPut("{id}")]
        public async Task<ActionResult<Forum>> UpdateForum(int id, [FromBody] ForumUpdateDTO request)
        {
            //TODO only users with the correct userId should be able to change the title
            var forum = await _db.Forums
                .Include(f => f.ForumPosts)
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
        public async Task<ActionResult<Forum>> DeleteForum(int id)
        {
            var forum = await _db.Forums
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            try
            {
                _db.Forums.Remove(forum);
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
        [HttpGet("{ForumId}/posts")]
        public async Task<ActionResult<Forum>> GetPostsByForumId(int ForumId)
        {
            var forumPosts = _db.ForumPosts.Where(p => p.ForumId == ForumId);

            if (forumPosts.Count() == 0) return NotFound("No posts on this forum");
            return Ok(forumPosts);
        }


        //POST NEW POST TO FORUM 
        [HttpPost("{ForumId}/posts/{userId}")]
        public async Task<ActionResult<Post>> CreateForumPost(int ForumId, int userId, [FromBody] PostCreateDto request)
        {
            var newPost = new ForumPost { UserId = userId, ForumId = ForumId, Content = request.Content };

            var posts = _db.ForumPosts.Where(p => p.ForumId == ForumId).ToList(); // redundant (just for returning posts)
            posts.Add(newPost); // redundant (just for returning posts)

            _db.ForumPosts.Add(newPost);
            _db.SaveChanges();

            return Ok(posts);
        }

        //REST OF THE OPERATIONS DO NOT REQUIRE FORUMID AND BELONG IN POSTSCONTROLLER 
    }
}