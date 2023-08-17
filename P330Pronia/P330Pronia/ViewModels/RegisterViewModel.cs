using System.ComponentModel.DataAnnotations;

namespace P330Pronia.ViewModels;

public class RegisterViewModel
{
    [Required, MaxLength(256)]
    public string Fullname { get; set; }
    [Required, MaxLength(256)]
    public string UserName { get; set; }
    [Required, MaxLength(256), DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string PasswordConfirm { get; set; }
}