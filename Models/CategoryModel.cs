using System.ComponentModel.DataAnnotations;

namespace BookStorewithMVC.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Full Name is required.")]
        [Display(Name = "User")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        [StringLength(100, ErrorMessage = "Category Name cannot exceed 100 characters.")]
        [Display(Name = "Category Name")]
        public string? CategoryName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Created At")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Modified At")]
        public DateTime? ModifiedAt { get; set; }

        public string? FullName { get; set; }

        public List<UserDropDownModel>? UserList { get; set; }

        public class UserDropDownModel
        {
            public int UserId { get; set; }

            [Display(Name = "FullName")]
            public string? FullName { get; set; } = string.Empty;
        }
    }
}