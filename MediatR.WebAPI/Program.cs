using MediatR.MediatR.MicrosoftExtensionsDI;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(me =>
{
    me.RegisterServicesFromAssembly(typeof(MediatR.WebAPI.Application.TestCommand).Assembly);
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(oapi =>
{
    oapi.AddDocumentTransformer(async (document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "MediatR Test API",
            Version = "v1",
            Description = "MediatR Test Server"
        };
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(so =>
    {
        so.Title = "MediatR API";
        so.Theme = ScalarTheme.Saturn;
        so.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
        so.CustomCss = "";
        so.ShowSidebar = true;
        so.DotNetFlag = true;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
