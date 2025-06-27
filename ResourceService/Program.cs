using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ResourceService.Data;
using ResourceService.DTO; 
using ResourceService.Services.Interfaces;
using ResourceService.Services;
using System.Text;
using Wolverine;
using Wolverine.RabbitMQ;
using ConstructionProject.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ResourceDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<BidMessageHandler>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration["JwtOptions:SecretKey"] ?? throw new InvalidOperationException("SecretKey not configured");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });



builder.Host.UseWolverine(opts =>
{
    var rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMq") ?? throw new InvalidOperationException("RabbitMq connection string not configured");

    opts.UseRabbitMq(new Uri(rabbitMqConnectionString))
        .AutoProvision();

    opts.ListenToRabbitQueue("bids.to.resources", q =>
    {
        q.IsDurable = true;
    });

    opts.PublishMessage<BidResultDto>().ToRabbitQueue("bids.results");
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "¬ведите токен: Bearer {ваш_токен}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            System.Array.Empty<string>()
        }
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<ResourceDbContext>();
        await dbContext.Database.MigrateAsync();
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();  
app.MapControllers();

app.Run();