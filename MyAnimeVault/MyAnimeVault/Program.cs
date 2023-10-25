using MyAnimeVault.Domain.Services;
using MyAnimeVault.MyAnimeListApi.Services;
using Firebase.Auth;
using Firebase.Auth.Providers;
using MyAnimeVault.Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

//gets the Firebase Api Key and project id from user secrets file (will change this to azure key vault later)
string Firebase_Api_Key = builder.Configuration["Firebase_Api_Key"];
string Firebase_Project_Id = builder.Configuration["Firebase_Project_Id"];

FirebaseAuthConfig FirebaseConfig = new FirebaseAuthConfig
{
    ApiKey = Firebase_Api_Key,
    AuthDomain = $"{Firebase_Project_Id}.firebaseapp.com",
    Providers = new FirebaseAuthProvider[]
    {
        new EmailProvider()
    }
}; 

FirebaseAuthClient FirebaseClient = new FirebaseAuthClient(FirebaseConfig);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddTransient<IAnimeApiService, AnimeApiService>();

builder.Services.AddSingleton<FirebaseAuthConfig>(FirebaseConfig); //adding the config and client so I can inject the client into my authentication service
builder.Services.AddTransient<FirebaseAuthClient>(provider =>
{
    FirebaseAuthConfig firebaseAuthConfig = provider.GetRequiredService<FirebaseAuthConfig>();
    return new FirebaseAuthClient(firebaseAuthConfig);
});
builder.Services.AddTransient<IAuthenticator, Authenticator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
