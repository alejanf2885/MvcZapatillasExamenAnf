using System.Text.Json;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.EntityFrameworkCore;
using MvcZapatillasExamenAnf.Data;
using MvcZapatillasExamenAnf.Repositories;

var builder = WebApplication.CreateBuilder(args);

string secretName = "zapatillas/rds-connection";
string region = "us-east-2";

AmazonSecretsManagerClient client = new AmazonSecretsManagerClient(
    RegionEndpoint.GetBySystemName(region)
);

GetSecretValueRequest request = new GetSecretValueRequest
{
    SecretId = secretName,
    VersionStage = "AWSCURRENT",
};

GetSecretValueResponse response = await client.GetSecretValueAsync(request);
JsonDocument secret = JsonDocument.Parse(response.SecretString);

string host = secret.RootElement.GetProperty("host").GetString();
string username = secret.RootElement.GetProperty("username").GetString();
string password = secret.RootElement.GetProperty("password").GetString();
string dbname = secret.RootElement.GetProperty("dbname").GetString();

string connectionString =
    $"Server={host};Port=3306;Database={dbname};User={username};Password={password};";

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySQL(connectionString));
builder.Services.AddScoped<RepositoryZapatillas>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
