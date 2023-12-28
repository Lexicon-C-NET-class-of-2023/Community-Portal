using API;
using API.Controllers;
using Community_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestProject
{
    public class ForumTest
    {
        private readonly ILogger<ForumsController> _logger;
        private readonly AppDbContext _db;
        public ForumsController controller;

        public ForumTest()
        {
            controller = new ForumsController(_logger, _db);
        }


        [Fact]
        public async void GetAllForums()
        {
            try
            {
                var actionResult = await controller.GetForums();

                ActionResult<List<Message>> response = actionResult.Result as OkObjectResult;
                Console.WriteLine(response);
                Assert.IsType<OkObjectResult>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


    }
}