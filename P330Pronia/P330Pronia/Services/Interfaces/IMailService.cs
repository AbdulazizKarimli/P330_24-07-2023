using P330Pronia.ViewModels;

namespace P330Pronia.Services.Interfaces;

public interface IMailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}