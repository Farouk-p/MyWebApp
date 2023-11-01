using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyWebApp.ApiServices;
using MyWebApp.DbContextData;
using MyWebApp.Models;

namespace MyWebApp.Repository
{
    public class UserRepo
    {
        private readonly MyWebAppDbContext _context;
        public UserRepo(MyWebAppDbContext context) 
        { 
            _context = context;
        }

        public async Task<List<Users>> GetUsers()
        {
            var UserList = await _context.users.ToListAsync();
            return UserList;
        }

        public async Task<Users> GetUserByPhone(string Phone)
        {
            var UserList = await GetUsers();
            var UserByPhone = UserList.FirstOrDefault(x => x.Phone == Phone);
            return (UserByPhone == null) ? new Users { Phone = $"Record Not Found for {Phone}"} : UserByPhone;

        }

        public async Task<Users> AddUser(Users req)
        {
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            var UserList = await GetUsers(); 
            var Phone = req.Phone;
            var UserByPhone = UserList.Count(x => x.Phone == Phone);

            var payload = new Users
            {
                LastName = req.LastName,
                FirstName = req.FirstName,
                Address = req.Address,
                Username = req.Username,
                Password = EncryptionService.EncryptString(key, req.Password),
                Phone = Phone
            };

            if (UserByPhone >= 1)
            {
                return new Users { Phone = $"Record exists in the dB {Phone}" };
            }
            else
            {
                await _context.users.AddAsync(payload);
                var AddUsers = _context.SaveChanges();
                return req;
            }

        }

        public async Task<Users> LoginUser(string username, string password)
        {
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            var GetUserDetails_db =   _context.users.FirstOrDefault(x => x.Username == username);
            var decryptPassword = EncryptionService.DecryptString(key, GetUserDetails_db.Password);

            if (GetUserDetails_db == null) return new Users { Username = $"No detail found for {username}" };
            //var GetPassword_db = (GetUserDetails_db == null) ? " " : GetUserDetails_db.Password;
            //var GetUsername_db = (GetUserDetails_db == null) ? " " : GetUserDetails_db.Username;

            if (GetUserDetails_db.Username == username && decryptPassword == password)
            {
                return new Users { Username = "Login Successful" };
            } else
            {
                return new Users { Username = "Username/Password incorrect" };
            }
        }
        public async Task<List<Books>> GetBooks()
        {
            var BookList = await _context.books.ToListAsync();
            return BookList;
        }


    }
}
