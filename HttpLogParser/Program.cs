using Hellang.Middleware.ProblemDetails;
using HttpLogParser.Loaders;
using HttpLogParser.Models;
using HttpLogParser.Parsers;
using HttpLogParser.Repositories;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppOptions>(builder.Configuration.GetSection("App"));

// configure exception handling middleware
builder.Services.AddProblemDetails(options =>
{
    options.IncludeExceptionDetails = (ctx, ex) => false;
    options.ShouldLogUnhandledException = (context, ex, problem) => false;
    options.GetTraceId = ctx => null;

    options.Map<Exception>(ex => new ProblemDetails
    {
        Title = "Internal Server Error",
        Status = (int)HttpStatusCode.InternalServerError,
        Detail = "Oops, something went wrong"
    });
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddTransient<ILoader, FileSystemLoader>();
builder.Services.AddTransient<IRepository, InMemoryRepository>();
builder.Services.AddTransient<IParser, RegexParser>();

var app = builder.Build();

app.UseProblemDetails();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

// use Minimal API endpoints
app.MapGet("/HttpLogReport", async (IMediator mediator, ILogger<Program> logger, CancellationToken cancellationToken) =>
{
    var result = await mediator.Send(new GetHttpLogReportQuery(), cancellationToken);
    return Results.Ok(result);
})
.WithName("GetHttpLogReport")
.WithOpenApi();

// let's rock!
app.Run();
