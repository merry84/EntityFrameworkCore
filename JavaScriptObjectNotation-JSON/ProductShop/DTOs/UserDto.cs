using System.ComponentModel.DataAnnotations;

namespace ProductShop.DTOs;

public class UserDto
{

    public string? FirstName { get; set; }
    [Required] 
    public string LastName { get; set; } = null!;

    public string? Age { get; set; }
   
}