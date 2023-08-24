using System.ComponentModel.DataAnnotations;

namespace AdoNetCrud.Models
{
    public class BlogModel
    {
        [Required]
        public string title { get; set; }

        [Required]
        public string summary { get; set; }

        [Required]
        public string content { get; set; }

        [Required]
        public string slug { get; set; }
    }
    public class BlogPost
    {
        public int id { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public string content { get; set; }
        public string slug { get; set; }
    }
    public class BlogPostDetail
    {
        public string title { get; set; }
        public string content { get; set; }
    }
    public class BlogUpdateModel : BlogModel
    {
        [Required]
        public int id { get; set; }
    }

}
