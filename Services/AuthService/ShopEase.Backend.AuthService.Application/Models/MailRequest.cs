namespace ShopEase.Backend.AuthService.Application.Models
{
    public class MailRequest
    {
        public required List<string> Recipients { get; set; }
       
        public required string Subject { get; set; }
        
        public required string Body { get; set; }
    }
}
