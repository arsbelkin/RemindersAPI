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
    
    public async Task SendAsync(NotificationWrapper message)
    {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:notifications"));

        await endpoint.Send(message);
    }
}