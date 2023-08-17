using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace P330Pronia.Models.Identity;

public class AppUser : IdentityUser
{
    [Required, MaxLength(256)]
    public string Fullname { get; set; }
    public bool IsActive { get; set; }
}