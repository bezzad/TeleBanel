using System.Collections.Generic;
using System.Linq;
using TeleBanel.Models;
using TeleBanel.Models.Middlewares;

namespace TeleBanel.Test.MiddlewareModels
{
    public class InboxMiddleware : IInboxMiddleware
    {
        private IList<Inbox> Messages { get; set; }

        public InboxMiddleware()
        {
            Messages = new List<Inbox>
            {
                new Inbox()
                {
                    Id = 1,
                    Email = "One@test.com",
                    Name = "Behzad",
                    Subject = "Buy some thing",
                    Message = "I have to buy the new project..."
                },
                new Inbox()
                {
                    Id = 2,
                    Email = "Two@test.com",
                    Name = "Json",
                    Subject = "Report bugs",
                    Message = "Your website crashed when I press the subscribe button!"
                },
                new Inbox()
                {
                    Id = 3,
                    Email = "Three@test.com",
                    Name = "David",
                    Subject = "I love your page",
                    Message = "Thanks a lot to sending this page :)"
                }
            };
        }

        public IList<Inbox> GetMessages()
        {
            return Messages;
        }

        public void DeleteMessage(int msgId)
        {
            Messages = Messages.Where(mbox => mbox.Id != msgId).ToList();
        }
    }
}
