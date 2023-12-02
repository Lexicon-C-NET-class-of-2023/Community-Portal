using Community_Portal;
using Community_Portal.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Text.Json;


namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
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
        public async Task<ActionResult<List<Forum>>> GetForums() => await _db.Forums.Include(f => f.Posts).ToListAsync();



        //GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Forum>> GetForumById(int id)
        {
            var forum = await _db.Forums
                .Include(f => f.Posts)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (forum is null) return NotFound("No forum with that id");
            return Ok(forum);
        }


        //POST
        [HttpPost("{userId}")]
        public async Task<ActionResult<Forum>> CreateForum(ForumCreateDTO request, int userId)
        {
            var newForum = new Forum { UserId = userId, Title = request.Title, };
            var posts = request.Posts.Select(p => new Post { UserId = userId, Content = p.Content, Forum = newForum }).ToList();

            newForum.Posts = posts;

            _db.Forums.Add(newForum);
            await _db.SaveChangesAsync();

            var response = _db.Forums.Include(f => f.Posts).ToList();

            return Ok(response);
        }


        //UPDATE
        [HttpPut("{id}")]
        public async Task<ActionResult<Forum>> UpdateForum(int id)
        {
            var forum = await _db.Forums
                .Include(f => f.Posts)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (forum is null) return NotFound("No forum with that id");
            return Ok(forum);
        }


        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<Forum>> DeleteForum(int id)
        {
            var forum = await _db.Forums.FirstOrDefaultAsync(f => f.Id == id);

            _db.Forums.Remove(forum);
            _db.SaveChanges();

            if (forum is null) return NotFound("No forum with that id");
            return Ok(forum);
        }


        //-----------------------------------------------------POSTS-----------------------------------------------------


        //GET ALL POSTS IN FORUM
        [HttpGet("{ForumId}/posts")]
        public async Task<ActionResult<Forum>> GetPostsByForumId(int ForumId)
        {
            var forumPosts = _db.Posts.Where(p => p.ForumId == ForumId);

            if (forumPosts.Count() == 0) return NotFound("No posts on this forum");
            return Ok(forumPosts);
        }


        //POST NEW POST TO FORUM 
        [HttpPost("{ForumId}/posts/{userId}")]
        public async Task<ActionResult<Post>> CreateForumPost(int ForumId, int userId, [FromBody] PostCreateDto request)
        {
            var newPost = new Post { UserId = userId, ForumId = ForumId, Content = request.Content };

            var posts = _db.Posts.Where(p => p.ForumId == ForumId).ToList(); // redundant (just for returning posts)
            posts.Add(newPost); // redundant (just for returning posts)

            _db.Posts.Add(newPost);
            _db.SaveChanges();

            return Ok(posts);
        }

        //REST OF THE OPERATIONS DO NOT REQUIRE FORUMID AND BELONG IN POSTSCONTROLLER 
    }
}