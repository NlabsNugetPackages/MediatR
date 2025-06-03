# MediatR Clone

## NLabs MediatR

A high-performance, feature-rich MediatR implementation for .NET 9 applications. Built from the ground up to provide CQRS (Command Query Responsibility Segregation) patterns with enhanced functionality and enterprise-grade features. This implementation offers an alternative to the original MediatR with additional capabilities and optimizations.

## Features

- 🚀 **High Performance** – Optimized implementation with reduced allocations  
- 📝 **Complete CQRS Support** – Commands, Queries, and Notifications  
- 🔄 **Advanced Pipeline** – Extensible request/response pipeline  
- 🛡️ **Built-in Validation** – Integrated validation behaviors  
- 📊 **Performance Monitoring** – Built-in performance tracking and metrics  
- 🔧 **Easy Configuration** – Streamlined setup and configuration  
- 🎯 **Type Safety** – Strong typing throughout the pipeline  
- 💼 **Enterprise Ready** – Production-ready with comprehensive features  
- 🔌 **Extensible** – Rich extensibility points and custom behaviors  

---

## Installation

### Clone Repository

```bash
git clone https://github.com/NlabsNugetPackages/MediatR.git
cd MediatR
```
### Build Project
```bash
dotnet build
```
### Reference in Your Project
```xml
<ProjectReference Include="path/to/NLabs.MediatR/NLabs.MediatR.csproj" />
```
💡 Note: Commercial NuGet package release planned for Q3 2025 with additional enterprise features and support.
### Quick Start
### 1. Registration in Program.cs (.NET 9)
```csharp
using NLabs.MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNLabsMediatR(configuration =>
{
    configuration.RegisterFromAssembly(typeof(Program).Assembly);
});

var app = builder.Build();
```
### 2. Create a Command
```csharp
using MediatR;

public record TestCommand(string Name) : IRequest<string>;

public class TestCommandHandler : IRequestHandler<TestCommand, string>
{
    public async Task<string> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(100, cancellationToken);
        return $"Test command processed with name: {request.Name}";
    }
}
```
### 3. Create a Query
```csharp
using MediatR;

public record GetUserQuery(int Id) : IRequest<GetUserResult>;

public record GetUserResult(int Id, string Name, string Email);

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserResult>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUserResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        return new GetUserResult(user.Id, user.Name, user.Email);
    }
}
```
### 4. Use in Controller
```csharp
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ISender _sender;

    public TestController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Create(CancellationToken cancellationToken)
    {
        var command = new TestCommand("Test Name");
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }
}
```
### Project Structure
NLabs.MediatR/
├── src/
│   ├── Interfaces/          # Core interfaces (IRequest, IRequestHandler, etc.)
│   ├── Behaviors/           # Pipeline behaviors
│   ├── Configuration/       # DI setup and configuration
│   └── Extensions/          # Extension methods
├── tests/
│   ├── Unit/                # Unit tests
│   └── Integration/         # Integration tests
├── samples/
│   └── WebApi/              # Sample web API project
└── docs/                    # Documentation
### Custom Behaviors
```csharp
builder.Services.AddNLabsMediatR(configuration =>
{
    configuration.RegisterFromAssembly(typeof(Program).Assembly);
    configuration.AddValidationBehavior();
    configuration.AddPerformanceBehavior();
    configuration.AddLoggingBehavior();
});
```
### Validation Behavior
```csharp
using FluentValidation;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
```
### Custom Pipeline Behavior
```csharp
public class CustomBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Processing {typeof(TRequest).Name}");
        var response = await next();
        Console.WriteLine($"Processed {typeof(TRequest).Name}");
        return response;
    }
}
```
### Advanced Configuration
#### NLabsMediatRConfiguration
```csharp
public class NLabsMediatRConfiguration
{
    public bool EnableValidation { get; set; } = true;
    public bool EnablePerformanceTracking { get; set; } = true;
    public bool EnableLogging { get; set; } = true;
    public TimeSpan PerformanceThreshold { get; set; } = TimeSpan.FromMilliseconds(500);
}
```
```csharp
builder.Services.AddNLabsMediatR(configuration =>
{
    configuration.Options.EnableValidation = true;
    configuration.Options.EnablePerformanceTracking = true;
    configuration.Options.PerformanceThreshold = TimeSpan.FromMilliseconds(1000);
    configuration.RegisterFromAssembly(typeof(Program).Assembly);
});
```
### Integration with Angular Frontend
#### TypeScript Models
```typescript
// models/user.model.ts
export interface CreateUserCommand {
  name: string;
  email: string;
}

export interface CreateUserResult {
  id: number;
  name: string;
  email: string;
}

export interface GetUserResult {
  id: number;
  name: string;
  email: string;
}
```
