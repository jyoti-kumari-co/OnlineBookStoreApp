using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace OnlineBookStoreApp.Models
{
    public class BookViewModel
    {
        [Key]
        public int BookID { get; set; }
        [Required]
        [DisplayName("Book Name")]
        public string BookName { get; set; }
        [Required]
        public string Author { get; set; }

        [Required, StringLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
        public string ISBN { get; set; }

        [Required]
        [Range(1, int.MaxValue,ErrorMessage ="Price should be greater than 1 or equal to 1")]
        
        public string Price { get; set; }

        [DisplayName("Published year")]
        public string publishedYear { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabetic characters are allowed.")]
        public string Publisher { get; set; }
    }
}
