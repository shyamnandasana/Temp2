using System.ComponentModel.DataAnnotations;


namespace BookStorewithMVC.Models
{
    public class PublisherModel
    {
         public int PublisherId { get; set; }

         [Display(Name = "User")]
        [Required(ErrorMessage = "Please select a user.")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Publisher Name is required.")]
         [StringLength(150, ErrorMessage = "Publisher Name cannot exceed 150 characters.")]
         [Display(Name = "Publisher Name")]
         public string? Name { get; set; }

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
