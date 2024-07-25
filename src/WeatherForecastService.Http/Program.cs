using AuthorizationService.Grpc;
using Grpc.Net.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.Use(async (context, next) => {
    // Create grpc client and call authorization service passing request Authorization token.
    using var channel = GrpcChannel.ForAddress("http://localhost:5258");
    var client = new Authorization.AuthorizationClient(channel);
    var reply = await client.AuthorizeAsync(
        new AuthorizationRequest
        {
            Token = context.Request.Headers.Authorization.ToString()
        }
    );
    // Pass request forward of return 401 (Unauthorized) depending on authorization result.
    if (reply.IsAuthorized)
    {
        await next(context);
    }
    else
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("You are not authorized to use this API");
    }
});

app.MapControllers();

app.Run();
