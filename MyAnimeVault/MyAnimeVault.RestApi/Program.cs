using Microsoft.EntityFrameworkCore;
using MyAnimeVault.EntityFramework;
using MyAnimeVault.EntityFramework.Services;
using MyAnimeVault.RestApi.Services;

var builder = WebApplication.CreateBuilder(args);

string ConnectionString = builder.Configuration["ConnectionString"];

// Add services to the container.
builder.Services.AddDbContext<MyAnimeVaultDbContext>(options =>
{
    options.UseInMemoryDatabase("TestDb");
    //options.UseSqlServer(ConnectionString);
});

builder.Services.AddScoped(typeof(IGenericDataService<>), typeof(GenericDataService<>));
builder.Services.AddScoped(typeof(MyAnimeVault.RestApi.Services.IUserDataService), typeof(UserDataService));
builder.Services.AddScoped(typeof(IUserAnimeDataService), typeof(UserAnimeDataService));
builder.Services.AddScoped(typeof(IPosterDataService), typeof(PosterDataService));
builder.Services.AddScoped(typeof(IStartSeasonDataService), typeof(StartSeasonDataService));

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

app.MapControllers();

app.Run();
