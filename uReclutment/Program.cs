using Entities.DataContext;
using Microsoft.EntityFrameworkCore;
using uReclutment.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgresSQLConnection");
builder.Services.AddDbContext<WebApiDbContext>(options => 
options.UseNpgsql(connectionString, b => b.MigrationsAssembly("uReclutment")));

IoC.AddDependency(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
