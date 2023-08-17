using System.ComponentModel.DataAnnotations;

namespace P330Pronia.ViewModels;

public class ForgotPasswordViewModel
{
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}