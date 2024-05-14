using BuildingBlocks.Messaging.MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<IBasketRepository, BasketRepository>()
    .Decorate<IBasketRepository, CachedBasketRepository>()
    .AddStackExchangeRedisCache(opts =>
    {
        opts.Configuration = builder.Configuration.GetConnectionString("Redis");
    })
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
        opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    }).UseLightweightSessions();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    return handler;
});

builder.Services.AddMessageBroker(builder.Configuration);

var app = builder.Build();

app.MapCarter();
app
    .UseExceptionHandler(options => { })
    .UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    });

app.Run();
