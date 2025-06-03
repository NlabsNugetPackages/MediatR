using MediatR.MediatR.Contracts;

namespace MediatR.MediatR;
public record NotificationHandlerExecutor(object HandlerInstance, Func<INotification, CancellationToken, Task> HandlerCallback);