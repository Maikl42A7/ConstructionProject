using BidService.Data;
using BidService.DTO;
using BidService.Services;
using BidService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Wolverine;
using Wolverine.RabbitMQ;
using BidService.Jwt;
using Microsoft.OpenApi.Models;
using ConstructionProject.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BidDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBidRepository, BidRepository>();
builder.Services.AddScoped<BidService.Services.BidService>();
builder.Services.AddScoped<BidResultHandler>();

builder.Host.UseWolverine(opts =>
{
    var rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMq");

    opts.UseRabbitMq(new Uri(rabbitMqConnectionString))
        .AutoProvision()
        .UseConventionalRouting();

    opts.PublishMessage<BidMessageDto>().ToRabbitQueue("bids.to.resources");
    opts.ListenToRabbitQueue("bids.results");
});

builder.Services.Configure<JwtOptions>(
   builder.Configuration.GetSection("JwtOptions"));

var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
var secretKey = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = false,
           ValidateAudience = false,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(secretKey)
       };
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
        Description = "¬ведите токен в формате: Bearer {ваш_токен}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            System.Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<BidDbContext>();

        await dbContext.Database.MigrateAsync();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();