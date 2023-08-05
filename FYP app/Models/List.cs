using System.ComponentModel.DataAnnotations;

namespace FYPfinalWEBAPP.Models;

public class List
{

    public int Id { get; set; }

    [Required]
    public string FoodName { get; set; } = null!;

    [Required]
    public string Brand { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public int Quantity { get; set; }
    public double Cost { get; set; }
    public int HPno { get; set; }

    [Required]
    public string Description { get; set; } = null!;
    public IFormFile Photo { get; set; } = null!;
    public string Picture { get; set; } = null!;
    public string SubmittedBy { get; set; } = null!;
}
