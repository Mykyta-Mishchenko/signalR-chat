using Azure.AI.TextAnalytics;
using Azure;
using chat_backend.AppExtensionMethods;
using chat_backend.Modules.OnlineChat;
using chat_backend.Shared.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using monopoly_backend.AppExtensionMethods;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ChatDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString"),
        sqlOptions => sqlOptions.EnableRetryOnFailure());
});

builder.Services.AddCors(opts =>
{
    opts.AddPolicy("CorsPolicy", policyBuilder =>
    {
        policyBuilder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:4200", "https://jolly-wave-058925903.6.azurestaticapps.net");
    });
});

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddApplicationServices();
builder.Services.AddApplicationRepositories();
builder.Services.AddValidators();
builder.Services.AddApplicationMappers();

builder.Services.AddSingleton<IUserIdProvider, NameIdentifierUserIdProvider>();
builder.Services.AddSingleton(serviceProvider =>
{
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    var credential = new AzureKeyCredential(config["TextAnalytics:Key"]);
    var endpoint = new Uri(config["TextAnalytics:Endpoint"]);

    return new TextAnalyticsClient(endpoint, credential);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatsHub>("/chatHub").RequireAuthorization();

app.Use(async (context, next) =>
{
    Console.WriteLine($"Auth Header: {context.Request.Headers["Authorization"]}");
    await next();
});

app.Run();
