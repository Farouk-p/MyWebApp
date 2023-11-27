using Microsoft.AspNetCore.Mvc;
using MyWebApp.ApiServices;
using MyWebApp.DbContextData;
using MyWebApp.Models;
using MyWebApp.Repository;

namespace MyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyAppController : Controller
    {
        private readonly MyWebAppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        public MyAppController(MyWebAppDbContext context , IConfiguration config, IEmailService emailService)
        {

            _context = context;
            _config = config;
            _emailService = emailService;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(UserProfiledto req) 
        {
            var service = new ProfileRepo(_context, _config, _emailService);
            var response = await service.RegisterUser(req);
            return response == "User exists already. Kindly Login" ? BadRequest ("User exists already. Kindly Login") : Ok("Successful");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto req) 
        {
            var service = new ProfileRepo(_context, _config, _emailService);
            var response = await service.Login(req);
            return response == "Username/Password incorrect" ? BadRequest("Username/Password incorrect") : Ok("Login Successful");
        }
    }
}
