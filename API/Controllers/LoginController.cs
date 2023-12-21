using Community_Portal;
using Community_Portal.DTO_s;
using Community_Portal.DTO_s.User;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly AppDbContext _db;

        public LoginController(ILogger<LoginController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        //POST
        [HttpPost()]
        public async Task<ActionResult<UserDTO>> CreateMessage([FromBody] LoginDTO request)
        {
            UserDTO user = _db.Users
                .Where(u => u.Email == request.email && u.Password == request.password)
                .Select(u => new UserDTO(u.Id, u.Created, u.FirstName, u.LastName, u.Email))
                .FirstOrDefault();

            if (user.Email == null) return NotFound(Services.NotFoundMessage("login"));
            return Ok(user);
        }
    }
}