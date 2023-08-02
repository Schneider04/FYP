using System.ComponentModel.DataAnnotations;


namespace FYPfinalWEBAPP.Models
{
    public class User
    {

        public string UserID { get; set; } = null!;

        [Required(ErrorMessage = "Please enter Password")]
        public string UserPw { get; set; } = null!;

        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter Email")]
        public string UserEmail { get; set; } = null!;

        public int HPno { get; set; }




    }
}
