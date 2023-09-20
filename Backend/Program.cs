using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using project_memu_api_server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(Options => Options.AddPolicy(name:"MyCorsPolicy",
            builder =>
            {
                builder.WithOrigins("http://192.168.1.62:8881/", "https://192.168.1.62:8882/", "http://localhost:5210", "https://localhost:7190") //jgn lupa cmd-> ipconfig ambil latest IP4 Address sblm run
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            })
);

// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false);

// Services
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("MyCorsPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();
