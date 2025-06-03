using MediatR.MediatR.Contracts;

namespace MediatR.MediatR;
public interface INotificationPublisher
{
    Task Publish(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification,
        CancellationToken cancellationToken);
}