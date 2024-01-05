using MyAnimeVault.Domain.Services;
using MyAnimeVault.MyAnimeListApi.Services;
using Firebase.Auth;
using Firebase.Auth.Providers;
using MyAnimeVault.Services.Authentication;
using Azure.Identity;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;
using MyAnimeVault.Domain.Services.Api.Database;
using MyAnimeVault.Services.Api.Database;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("AzureKeyVaultUri"));
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

//gets the Firebase Api Key and project id from user secrets file (will change this to azure key vault later)

string FirebaseApiKey = builder.Configuration["FirebaseApiKey"];
string FirebaseProjectId = builder.Configuration["FirebaseProjectId"];
string FirebasePrivateKey = builder.Configuration["FirebasePrivateKey"];
string MyAnimeVaultApiKey = builder.Configuration["MyAnimeVaultApiKey"];

FirebaseApp firebaseApp = FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromJson(FirebasePrivateKey),
});

FirebaseAuthConfig FirebaseConfig = new FirebaseAuthConfig
{
    ApiKey = FirebaseApiKey,
    AuthDomain = $"{FirebaseProjectId}.firebaseapp.com",
    Providers = new FirebaseAuthProvider[]
    {
        new EmailProvider()
    }
}; 

FirebaseAuthClient FirebaseClient = new FirebaseAuthClient(FirebaseConfig);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<FirebaseAuth>(provider =>
{
    return FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
});

builder.Services.AddSingleton<FirebaseAuthConfig>(FirebaseConfig); //adding the config and client so I can inject the client into my authentication service
builder.Services.AddTransient<FirebaseAuthClient>(provider =>
{
    FirebaseAuthConfig firebaseAuthConfig = provider.GetRequiredService<FirebaseAuthConfig>();
    return new FirebaseAuthClient(firebaseAuthConfig);
});

builder.Services.AddTransient<IAnimeApiService, AnimeApiService>();
builder.Services.AddTransient<IAuthenticator, Authenticator>();
builder.Services.AddScoped<IUserApiService, UserApiService>();
builder.Services.AddScoped<IUserAnimeApiService, UserAnimeApiService>();

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

//added this for HttpContextAccessor.HttpContext.Session to store currentuser info
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
