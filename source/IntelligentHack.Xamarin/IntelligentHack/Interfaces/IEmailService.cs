namespace IntelligentHack.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject);
    }
}