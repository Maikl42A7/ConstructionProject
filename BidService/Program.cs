using BidService.Data;
using BidService.DTO;
using BidService.Services;
using BidService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BidDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBidRepository, BidRepository>();
builder.Services.AddScoped<BidCreateService>();
builder.Services.AddScoped<BidResultService>();
builder.Services.AddScoped<BidService.Services.BidService>();

builder.Host.UseWolverine(opts =>
{
    opts.UseRabbitMq(new Uri("amqp://guest:guest@localhost:5672"))
        .AutoProvision()
        .UseConventionalRouting();

    opts.PublishMessage<BidMessageDto>().ToRabbitQueue("bids.to.resources");
    opts.ListenToRabbitQueue("bids.results"); 
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();

