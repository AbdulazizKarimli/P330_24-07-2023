using System.ComponentModel.DataAnnotations;

namespace P330Pronia.ViewModels;

public class ResetPasswordViewModel
{
    [Required, DataType(DataType.Password)]
    public string NewPassword { get; set; }
    [Required, DataType(DataType.Password), Compare(nameof(NewPassword))]
    public string ConfirmNewPassword { get; set; }
}