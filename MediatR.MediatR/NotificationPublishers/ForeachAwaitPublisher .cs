﻿using MediatR.MediatR.Contracts;

namespace MediatR.MediatR.NotificationPublishers;
/// <summary>
/// Awaits each notification handler in a single foreach loop:
/// <code>
/// foreach (var handler in handlers) {
///     await handler(notification, cancellationToken);
/// }
/// </code>
/// </summary>
public class ForeachAwaitPublisher : INotificationPublisher
{
    public async Task Publish(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification, CancellationToken cancellationToken)
    {
        foreach (var handler in handlerExecutors)
        {
            await handler.HandlerCallback(notification, cancellationToken).ConfigureAwait(false);
        }
    }
}