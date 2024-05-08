var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCarter()
    .AddMediatR(config =>
        config.RegisterServicesFromAssemblyContaining<Program>())
    .AddMarten(opts =>
    {
        opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    }).UseLightweightSessions()
    ;

var app = builder.Build();

app
    .MapCarter()
    ;

app.Run();
