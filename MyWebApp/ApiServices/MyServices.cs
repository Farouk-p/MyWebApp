using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApp.DbContextData;
using MyWebApp.Models;
using Newtonsoft.Json;
using System.Security.Policy;
using System.Text.Json.Serialization;
using static System.Reflection.Metadata.BlobBuilder;

namespace MyWebApp.ApiServices
{
    public class MyServices
    {
        private readonly MyWebAppDbContext _context;
        private readonly IConfiguration _config;
        
        public MyServices(MyWebAppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<MyBooks> BookList()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://fakerapi.it/api/v1/books?_quantity=1");
            var result = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<MyBooks>(result);
            return (books == null) ? null : books;
        }

        public async Task<List<Books>> GetBooks()
        {
            var BookList = await _context.books.ToListAsync();
            return BookList;
        }

        public async Task<List<Books>> SomeBookList()
        {
            try
            {
                //Look through the dB first if there's any record

                var anyRecord = await _context.books.AnyAsync();



                if (anyRecord == false)
                {
                    HttpClient client = new HttpClient();
                    var response = await client.GetAsync("https://fakerapi.it/api/v1/books?_quantity=4");
                    var result = await response.Content.ReadAsStringAsync();
                    var book = JsonConvert.DeserializeObject<MyBooks>(result);


                    //Loop through response stored in book, to get and save the data
                    foreach (var apiResponse in book.Data)
                    {
                        var books = new Books()
                        {
                            Id = apiResponse.Id,
                            Title = apiResponse.Title,
                            Author = apiResponse.Author,
                            Genre = apiResponse.Genre,
                            Description = apiResponse.Description,
                            Isbn = apiResponse.Isbn,
                            Image = apiResponse.Image,
                            Published = apiResponse.Published,
                            Publisher = apiResponse.Publisher
                        };

                        await _context.books.AddAsync(books);
                        var AddBooks = _context.SaveChanges();
                    }

                    var BookList = await GetBooks();
                    return BookList;

                }
                else
                {

                    var BookList = await GetBooks();

                    return BookList;
                }
            }

            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<Encrypt> EncryptData(string input)
        {
            var key = _config.GetSection("encryptionKey").Value;
            var encValue = EncryptionService.EncryptString(key, input);

            var response = new Encrypt
            {
                Value = encValue,
            };

            return response;
        }

        public async Task<Encrypt> DecryptData(string input)
        {
            var key = _config.GetSection("encryptionKey").Value;
            var decValue = EncryptionService.DecryptString(key, input);

            var response = new Encrypt
            {
                Value = decValue,
            };

            return response;
        }


    }
}
