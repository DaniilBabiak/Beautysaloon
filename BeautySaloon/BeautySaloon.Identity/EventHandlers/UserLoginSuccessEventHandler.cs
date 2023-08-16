using BeautySaloon.Identity.RabbitMQ;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;

namespace BeautySaloon.Identity.EventHandlers;

public class UserLoginSuccessEventHandler : IEventSink
{
    private readonly CustomerPublisher _publisher;

    public UserLoginSuccessEventHandler(CustomerPublisher publisher)
    {
        _publisher = publisher;
    }

    public Task PersistAsync(Event evt)
    {
        if (evt is UserLoginSuccessEvent loginSuccessEvent)
        {
            var customer = new
            {
                Id = loginSuccessEvent.SubjectId,
                Name = loginSuccessEvent.DisplayName
            };

            _publisher.Enqueue(customer);
        }

        return Task.CompletedTask;
    }
}

