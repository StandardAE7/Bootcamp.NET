using System.Reflection;
using System.Text;
using Application.Services;
using Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistenceServices();


builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisConnectionString = builder.Configuration.GetConnectionString("Redis");

    if (string.IsNullOrEmpty(redisConnectionString))
    {
        throw new ArgumentNullException("Redis connection string is not configured.");
    }

    var configuration = ConfigurationOptions.Parse(redisConnectionString);
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis"); // Set your Redis connection string here
    options.InstanceName = "YourAppInstanceName:"; // Optional
});

// Register a singleton HashSet for the token blacklist
builder.Services.AddSingleton<HashSet<string>>();

builder.Services.AddSingleton<AuthService>(provider =>
{
    var secretKey = "jwtsecretkey_supersecret123456jwt"; // Use your actual secret key
    var issuer = "issuer"; // Replace with your actual issuer
    var audience = "audience"; // Replace with your actual audience
    var cache = provider.GetRequiredService<IDistributedCache>(); // Make sure this is available
    var tokenBlacklist = new HashSet<string>(); // Initialize as needed

    return new AuthService(secretKey, issuer, audience, cache, tokenBlacklist);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "issuer", // Replace with your actual issuer
        ValidAudience = "audience", // Replace with your actual audience
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jwtsecretkey_supersecret123456jwt")) // Replace with your key
    };
});

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
    return ConnectionMultiplexer.Connect(configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
