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
### Angular Service
```typescript
// services/user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserService {
  private apiUrl = 'https://localhost:7236/api/users';

  constructor(private http: HttpClient) {}

  createUser(command: CreateUserCommand): Observable<CreateUserResult> {
    return this.http.post<CreateUserResult>(this.apiUrl, command);
  }

  getUser(id: number): Observable<GetUserResult> {
    return this.http.get<GetUserResult>(`${this.apiUrl}/${id}`);
  }
}
```
### Performance Features
#### Built-in Performance Monitoring
```csharp
// Automatically logs slow requests
[Performance Monitoring] CreateUserCommand took 1250ms (threshold: 500ms)
```
### Metrics Collection
```csharp
public class MetricsBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IMetricsCollector _metrics;

    public MetricsBehavior(IMetricsCollector metrics)
    {
        _metrics = metrics;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var timer = _metrics.StartTimer($"mediatr.{typeof(TRequest).Name}");
        return await next();
    }
}
```
### Error Handling
#### Global Exception Handling
```csharp
public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;

    public ExceptionBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling {RequestType}", typeof(TRequest).Name);
            throw;
        }
    }
}
```
### Testing
#### Unit Testing
```csharp
[Test]
public async Task CreateUserCommand_ShouldReturnUser_WhenValidInput()
{
    var command = new CreateUserCommand("John Doe", "john@example.com");
    var handler = new CreateUserCommandHandler(_mockUserService.Object);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.That(result.Name, Is.EqualTo("John Doe"));
    Assert.That(result.Email, Is.EqualTo("john@example.com"));
}
```
### Integration Testing
```csharp
[Test]
public async Task CreateUser_EndToEnd_ShouldWork()
{
    var command = new CreateUserCommand("John Doe", "john@example.com");

    var response = await _client.PostAsJsonAsync("/api/users", command);

    response.EnsureSuccessStatusCode();
    var result = await response.Content.ReadFromJsonAsync<CreateUserResult>();
    Assert.That(result.Name, Is.EqualTo("John Doe"));
}
```
Examples
✅ Full Stack Example – Complete .NET + Angular implementation

✅ Backend Examples – Various architectural patterns

✅ Frontend Examples – Angular integration examples

Contributing
We welcome contributions!

Fork the repository

Create a feature branch

Make your changes

Add tests

Submit a pull request

Why NLabs MediatR?
Performance First
Zero allocation in hot paths

Optimized reflection with caching

Async-first design

Memory efficient pipeline

Enterprise Features
Metrics collection

Logging integration

Validation pipeline

Performance alerting

Distributed tracing

Developer Experience
Rich diagnostics

Configurable options

Clear error handling

Extensive samples & docs

Future Plans
Commercial Release Features
🔄 Advanced Caching

💬 Message Queue Integration (RabbitMQ, Azure SB)

🔍 Advanced Monitoring (App Insights, DataDog)

🔐 Built-in Authorization & Auditing

💼 Enterprise Support

Open Source Roadmap
✅ Full pipeline behaviors

✅ Performance benchmarks

✅ Sample projects

✅ API Documentation

License
🆓 MIT License for open source usage

💼 Commercial License will be available with enterprise features

Support
📧 Email: turkmvc@gmail.com

🐛 Issues: GitHub Issues

💬 Discussions: GitHub Discussions

📚 Docs: Wiki

💼 Commercial Support: Available with commercial license

Roadmap
Phase 1 (Current) - Open Source Foundation
✅ Core implementation

✅ Basic behaviors

✅ DI integration

🔄 Performance tuning

🔄 Full testing

Phase 2 - Commercial Release
Enterprise features

NuGet release

Full docs

Commercial support

Related Packages
NLabs.Extensions – Utility extensions

NLabs.Validation – Validation helpers

NLabs.Logging – Logging utilities
----------------------------------------------
Made with ❤️ by NLabs
