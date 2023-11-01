using Microsoft.EntityFrameworkCore;

namespace MyWebApp.Models
{
    public class EmailTemplate
    {
        
        public int ID { get; set; }

        public string Body { get; set; }
    }
}
