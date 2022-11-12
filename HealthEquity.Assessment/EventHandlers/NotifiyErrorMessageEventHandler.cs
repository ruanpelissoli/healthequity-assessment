using HealthEquity.Assessment.Domain.DomainEvents;
using MediatR;
using NToastNotify;

namespace HealthEquity.Assessment.EventHandlers;

public sealed class NotifiyErrorMessageEventHandler : INotificationHandler<ShowErrorMessageEvent>
{
    private readonly IToastNotification _toastNotification;

    public NotifiyErrorMessageEventHandler(IToastNotification toastNotification)
    {
        _toastNotification = toastNotification;
    }

    public Task Handle(ShowErrorMessageEvent notification, CancellationToken cancellationToken)
    {
        _toastNotification.AddErrorToastMessage(notification.Message);

        return Task.CompletedTask;
    }
}
