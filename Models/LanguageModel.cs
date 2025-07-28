using System.ComponentModel.DataAnnotations;

namespace BookStorewithMVC.Models
{
    public class LanguageModel
    {
        public int LanguageId { get; set; }

        [Required(ErrorMessage = "Language name is required.")]
        [StringLength(100, ErrorMessage = "Name can't exceed 100 characters.")]
        public string? LanguageName { get; set; }

        [Required(ErrorMessage = "User Name is required.")]
        public int? UserId { get; set; }

        public string? FullName { get; set; } 

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public List<UserDropDownModel> UserList { get; set; } = new List<UserDropDownModel>();

        public class UserDropDownModel
        {
            public int UserId { get; set; }
            public string FullName { get; set; } = string.Empty;
        }
    }
}
