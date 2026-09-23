using MassTransit;
using Reminders.Application.Services.Interfaces;
using Reminders.Application.TransferModels.Notification;

namespace Reminders.Infrastructure.Messaging;

public class MassTransitNotificationPublisher : INotificationPublisher
{
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public MassTransitNotificationPublisher(ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }
    
    public async Task SendAsync(NotificationMessageDTO message, CancellationToken cancellationToken)
    {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:notifications"));

        await endpoint.Send(message, cancellationToken);
    }
}