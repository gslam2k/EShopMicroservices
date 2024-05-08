using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;

using Catalog.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCarter()
    .AddMediatR(config =>
    {
        config.RegisterServicesFromAssemblyContaining<Program>();
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    })
    .AddValidatorsFromAssemblyContaining<Program>()
    .AddExceptionHandler<CustomExceptionHandler>()
    .AddMarten(opts =>
    {
        opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    }).UseLightweightSessions()
    ;

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}


builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);


var app = builder.Build();

app.MapCarter();
app
    .UseExceptionHandler(options => { })
    .UseHealthChecks("/health");

app.Run();
