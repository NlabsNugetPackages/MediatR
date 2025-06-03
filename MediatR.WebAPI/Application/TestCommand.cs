using MediatR.MediatR;
using MediatR.MediatR.Contracts;

namespace MediatR.WebAPI.Application;
public sealed record TestCommand(string Name) : IRequest<string>;
internal sealed class TestCommandHandler : IRequestHandler<TestCommand, string>
{
    public Task<string> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult($"Test command processed with name: {request.Name}");
    }
}