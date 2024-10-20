using HttpLogParser.Loaders;
using HttpLogParser.Models;
using HttpLogParser.Parsers;
using HttpLogParser.Repositories;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppOptions>(builder.Configuration.GetSection("App"));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddTransient<ILoader, FileSystemLoader>();
builder.Services.AddTransient<IRepository, InMemoryRepository>();
builder.Services.AddTransient<IParser, RegexParser>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/HttpLogReport", async (IMediator mediator, ILogger<Program> logger, CancellationToken cancellationToken) =>
{
    var result = await mediator.Send(new HttpLogReportQuery(), cancellationToken);
    return Results.Ok(result);
})
.WithName("GetHttpLogReport")
.WithOpenApi();

app.Run();
