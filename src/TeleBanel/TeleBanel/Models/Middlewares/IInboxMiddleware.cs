namespace TeleBanel.Models.Middlewares
{
    public interface IInboxMiddleware
    {
        Inbox[] GetMessages();
        Inbox GetMessage(int id);
        void SetMessage(Inbox msg);
    }
}
