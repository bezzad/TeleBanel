using System.Collections.Generic;

namespace TeleBanel.Models.Middlewares
{
    public interface IInboxMiddleware
    {
        IList<Inbox> GetMessages();
        void DeleteMessage(int msgId);
    }
}