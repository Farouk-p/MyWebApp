using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp.Models
{
    public class MyBooks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrimaryId { get; set; }
        public string Status { get; set; }
        public int Code { get; set; }
        public int Total { get; set; }
        public List<Data> Data { get; set; }


    }

    public class Data
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Isbn { get; set; }
        public string Image { get; set; }
        public string Published { get; set; }
        public string Publisher { get; set; }
    }

}
