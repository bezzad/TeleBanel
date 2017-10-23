using TeleBanel.Models;
using TeleBanel.Models.Middlewares;

namespace TeleBanel.Test.MiddlewareModels
{
    public class InboxMiddleware : IInboxMiddleware
    {
        public Inbox GetMessage(int id)
        {
            return new Inbox()
            {
                Id = id,
                Email = "test@test.com",
                Name = "test",
                Subject = "test subject",
                Message = $"Message id: {id}"
            };
        }

        public Inbox[] GetMessages()
        {
            return new Inbox[]
            {
                new Inbox()
                {
                    Id = 1,
                    Email = "test1@test.com",
                    Name = "test1",
                    Subject = "test subject 1",
                    Message = "Message id: 1"
                },
                new Inbox()
                {
                    Id = 2,
                    Email = "test2@test.com",
                    Name = "test2",
                    Subject = "test subject 2",
                    Message = "Message id: 2"
                },
                new Inbox()
                {
                    Id = 3,
                    Email = "test3@test.com",
                    Name = "test3",
                    Subject = "test subject 3",
                    Message = "Message id: 3"
                }
            };
        }

        public void SetMessage(Inbox msg)
        {
            
        }
    }
}
