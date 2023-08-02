using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace FYPfinalWEBAPP.Models;

public class UserLogin
{
    [Required(ErrorMessage = "Please enter User ID")]
    public string UserID { get; set; } = null!;

    [Required(ErrorMessage = "Please enter Password")]
    public string Password { get; set; } = null!;
}
