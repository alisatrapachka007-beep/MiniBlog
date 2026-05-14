using System.ComponentModel.DataAnnotations;

namespace MiniBlog.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите заголовок")]
        [StringLength(100, ErrorMessage = "Заголовок не должен превышать 100 символов")]
        [MinLength(3, ErrorMessage = "Заголовок должен содержать минимум 3 символа")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите содержание")]
        [MinLength(10, ErrorMessage = "Содержание должно содержать минимум 10 символов")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите автора")]
        [StringLength(50, ErrorMessage = "Имя автора не должно превышать 50 символов")]
        [MinLength(2, ErrorMessage = "Имя автора должно содержать минимум 2 символа")]
        public string Author { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}