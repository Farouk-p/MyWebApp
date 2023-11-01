using Microsoft.AspNetCore.Mvc;
using MyWebApp.ApiServices;
using MyWebApp.DbContextData;
using MyWebApp.Models;
using MyWebApp.Repository;

namespace MyWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Farouk", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly MyWebAppDbContext _context;
        private readonly IConfiguration _config;


    

        public WeatherForecastController(MyWebAppDbContext context, IConfiguration config)
        {

            _context = context;
            _config = config;
        }

        [HttpGet("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 6).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("GetMoney")]
        public decimal GetMoney()
        {
            return 1000; //"I have $1000";
        }

        [HttpPost("DoubleMyNumber")]
        public decimal DoubleMyNumber(int number)
        {
            return number * 2;
        }

        [HttpPost("IsEven")]
        public Boolean IsEven(int number)
        {
            return number % 2 == 0;
        }

        [HttpPost("IsLongName")]
        public Boolean IsLongName(string name)
        {
            if (name.Length < 6)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [HttpGet("GetAllUsers")]
        public async Task<List<Users>> GetAllUsers()
        {
            var GetUsers = await new UserRepo(_context).GetUsers();
            return GetUsers;

        }

        [HttpGet("GetUserByPhone")]
        public async Task<Users> GetUserByPhone(string Phone)
        {
            var GetAUser = await new UserRepo(_context).GetUserByPhone(Phone);
            return GetAUser;
        }

        [HttpPost("AddUser")]
        public async Task<Users> AddUser(Users req)
        {
            var AddUser = await new UserRepo(_context).AddUser(req);
            return AddUser;
        }

        [HttpPost("LoginUser")]
        public async Task<Users> LoginUser(string username, string password)
        {
            //var key = "b14ca5898a4e4133bbce2ea2315a1916";
            //var encPassword = EncryptionService.EncryptString(key, password);
            //var getPassword = EncryptionService.DecryptString(key, encPassword);
            var LoginUser = await new UserRepo(_context).LoginUser(username, password);
            return LoginUser;
        }

        [HttpGet("GetBooks")]
        public async Task<MyBooks> GetBooks()
        {
            //Call GetBook api
            var service = new MyServices(_context, _config);
            var response = await service.BookList();

            return response;
        }

        [HttpGet("GetSomeBooks")]
        public async Task<List<Books>> GetSomeBooks()
        {
            var service = new MyServices(_context, _config);

            var response = await service.SomeBookList();
            return response;
        }

        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await new MyServices(_context, _config).GetBooks());
        }

        [HttpGet("GetEncryptedValue")]
        public async Task<Encrypt> GetEncryptedValue(string input)
        {
            var GetEncValue = await new MyServices(_context, _config).EncryptData(input);
            return GetEncValue;

        }

        [HttpGet("GetDecryptedValue")]
        public async Task<Encrypt> GetDecryptedValue(string input)
        {
            var GetDecValue = await new MyServices(_context, _config).DecryptData(input);
            return GetDecValue;

        }

    }
}