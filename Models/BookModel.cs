using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace BookStorewithMVC.Models
{
    public class BookModel
    {
        public int BookId { get; set; }

        [Required]
        [Display(Name = "User")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "ISBN is required")]
        [StringLength(13, ErrorMessage = "Max 13 characters allowed")]
        [Display(Name = "ISBN")]
        public string? Isbn { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(255)]
        [Display(Name = "Title")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [Display(Name = "Author")]
        public int? AuthorId { get; set; }

        [Required(ErrorMessage = "Publisher is required")]
        [Display(Name = "Publisher")]
        public int? PublisherId { get; set; }

        [Required(ErrorMessage = "Language is required")]
        [Display(Name = "Language")]
        public int? LanguageId { get; set; }

        [Range(0.01, 99999.99)]
        [Required(ErrorMessage = "Price is required")]
        public decimal? Price { get; set; }
        public string? FullName { get; set; }
        public string? AuthorName { get; set; }
        public string? PublisherName { get; set; }
        public string? LanguageName { get; set; }
        public string? CategoryName { get; set; }


        [Range(0, int.MaxValue)]
        public int? Stock { get; set; }

        [Display(Name = "Book Image")]
        public string? BookImg { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
        public List<AuthorDropDownModel>? AuthorList { get; set; }
        public List<PublisherDropDownModel>? PublisherList { get; set; }
        public List<LanguageDropDownModel>? LanguageList { get; set; }
        public List<CategoryDropDownModel>? CategoryList { get; set; }
        public List<UserDropDownModel>? UserList { get; set; }


        public class UserDropDownModel
        {
            public int UserId { get; set; }
            public string FullName { get; set; } = null!;
        }
        public class AuthorDropDownModel
        {
            public int AuthorId { get; set; }
            public string AuthorName { get; set; } = null!;
        }

        public class PublisherDropDownModel
        {
            public int PublisherId { get; set; }
            public string PublisherName { get; set; } = null!;
        }

        public class LanguageDropDownModel
        {
            public int LanguageId { get; set; }
            public string LanguageName { get; set; } = null!;
        }

        public class CategoryDropDownModel
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; } = null!;
        }

    }
}