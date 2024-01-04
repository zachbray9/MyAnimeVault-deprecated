using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using MyAnimeVault.EntityFramework;
using MyAnimeVault.EntityFramework.Services;
using MyAnimeVault.RestApi.Authentication.Api;
using MyAnimeVault.RestApi.Services;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("AzureKeyVaultUri"));
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

string ConnectionString = builder.Configuration["ConnectionString"];

// Add services to the container.
builder.Services.AddDbContext<MyAnimeVaultDbContext>(options =>
{
    //options.UseInMemoryDatabase("TestDb");
    options.UseSqlServer(ConnectionString);
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
builder.Services.ConfigureSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MyAnimeVaultApi",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyAuthMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
