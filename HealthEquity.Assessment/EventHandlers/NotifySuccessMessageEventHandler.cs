using HealthEquity.Assessment.Domain.DomainEvents;
using MediatR;
using NToastNotify;

namespace HealthEquity.Assessment.EvevntHandlers;

public sealed class NotifySuccessMessageEventHandler : INotificationHandler<ShowSuccessMessageEvent>
{
    private readonly IToastNotification _toastNotification;

    public NotifySuccessMessageEventHandler(IToastNotification toastNotification)
    {
        _toastNotification = toastNotification;
    }

    public Task Handle(ShowSuccessMessageEvent notification, CancellationToken cancellationToken)
    {
        _toastNotification.AddSuccessToastMessage(notification.Message);

        return Task.CompletedTask;
    }
}
