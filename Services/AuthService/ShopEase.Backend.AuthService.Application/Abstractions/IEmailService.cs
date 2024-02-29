using ShopEase.Backend.AuthService.Application.Models;

namespace ShopEase.Backend.AuthService.Application.Abstractions
{
    /// <summary>
    /// Email ServiceS
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// To Send Mail
        /// </summary>
        /// <param name="mailRequest"></param>
        /// <returns></returns>
        Task SendMailAsync(MailRequest mailRequest);
    }
}
