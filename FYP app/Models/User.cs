using System.ComponentModel.DataAnnotations;


namespace FYPfinalWEBAPP.Models
{
    public class User
    {
        [Required(ErrorMessage = "Please enter User ID")]
        public string UserId { get; set; } = null!;

        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be 5-20 char")]
        [DataType(DataType.Password)]
        public string UserPw { get; set; } = null!;

        [Required(ErrorMessage = "Please enter Full Name")]
        public string FullName { get; set; } = null!;
    }
}