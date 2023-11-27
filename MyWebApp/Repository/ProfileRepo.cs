using Microsoft.AspNetCore.Mvc;
using MyWebApp.DbContextData;
using MyWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyWebApp.ApiServices;


namespace MyWebApp.Repository
{
    public class ProfileRepo
    {
        private readonly MyWebAppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public ProfileRepo(MyWebAppDbContext context, IConfiguration config, IEmailService emailService)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
        }

        public async Task<String> RegisterUser(UserProfiledto req)
        {
            try
            {


                var FirstName = req.FirstName;
                var LastName = req.LastName;
                var Email = req.Email;
                var PhoneNumber = req.Phone;

                //Get current date and time
                DateTime currentLocalDateTime = DateTime.Now;

                //Generate random four digits and create UserId
                Random random = new Random();
                int randomDigit = random.Next(10000);
                var fourDigits = randomDigit.ToString();
                var UserId = $"{FirstName[0]}" + $"{LastName[0]}" + fourDigits;

                //Create ID
                var uniqueId = Guid.NewGuid().ToString();





                var ProfileList = await _context.userProfile.ToListAsync();
                var ProfileByPhone = ProfileList.Count(x => x.Phone == PhoneNumber);
                var ProfileByEmail = ProfileList.Count(x => x.Email == Email);

                if (ProfileByEmail > 0)
                {
                    return "User exists already. Kindly Login";
                }

                if (ProfileByPhone > 0)
                {
                    return "User exists already. Kindly Login";
                }

                var key = _config.GetSection("encryptionKey").Value;
                var encPassword = EncryptionService.EncryptString(key, req.Password);

                var payload = new UserProfile
                {
                    Id = uniqueId,
                    FirstName = req.FirstName,
                    Lastname = req.LastName,
                    Email = req.Email,
                    Password = encPassword,
                    Phone = req.Phone,
                    UserId = UserId,
                    CreatedDate = currentLocalDateTime,
                    IsActive = "1",
                    Status = "ACTIVE"

                };

                var activity = new ActivityLog
                {
                    Id = 1,
                    Email = req.Email,
                    Activities = "Successful Registration",
                    Description = $"{req.FirstName}" + " " + $"{req.LastName}" + " " + "Signed up successfully",
                    Date = currentLocalDateTime

                };
                await _context.activityLog.AddAsync(activity);
                var AddActivity = _context.SaveChanges();

                await _context.userProfile.AddAsync(payload);
                var AddUsers = _context.SaveChanges();

                //Sign Up Email
                var body = "Thank You for registering. Kindly login to the platform with " + $"{UserId}" + " " + "and your password";
                var Subject = "Welcome to Profile App";

                await _emailService.SendMailAsync(req.FirstName, req.Email, body, Subject);
                return "Success";
            }
            catch (Exception ex)
            {
                return "An Error Occured";
            }
        }

        public async Task<String> Login(LoginDto req)
        {
            try
            {
                //Get current date and time
                DateTime currentLocalDateTime = DateTime.Now;

                var GetUserDetails_db = _context.userProfile.FirstOrDefault(x => x.UserId == req.UserId);
                if (GetUserDetails_db == null)
                {
                    return "Username/Password incorrect";
                }

                var key = _config.GetSection("encryptionKey").Value;
                var decryptPassword = EncryptionService.DecryptString(key, GetUserDetails_db.Password);

                // Assuming "Id" is your primary key 
                //var lastInsertedRecord = _context.activityLog.OrderByDescending(e => e.Id).FirstOrDefault();
                //var lastid = lastInsertedRecord.Id;
                //var newid = lastid + 1;


                if (GetUserDetails_db.UserId == req.UserId && decryptPassword == req.Password)
                {
                    var activity = new ActivityLog
                    {
                        Id = 1,
                        Email = GetUserDetails_db.Email,
                        Activities = "Successful Login",
                        Description = $"{GetUserDetails_db.FirstName}" + " " + $"{GetUserDetails_db.Lastname}" + " " + "Logged in successfully",
                        Date = currentLocalDateTime

                    };
                    await _context.activityLog.AddAsync(activity);
                    var AddActivity = _context.SaveChanges();

                    //Login Email Body
                    Random random = new Random();
                    int randomDigit = random.Next(10000);
                    var fourDigits = randomDigit.ToString();

                    var body = "OTP Verification code :" + " " + $"{fourDigits}";
                    var subject = "OTP Email";

                    await _emailService.SendMailAsync(GetUserDetails_db.FirstName, GetUserDetails_db.Email, body, subject);

                    return "Login Successful";
                }
                else
                {
                    return "Username/Password incorrect";
                }

            }
            catch (Exception ex)
            {
                return "An Error Occured";
            }
        }
    }
}