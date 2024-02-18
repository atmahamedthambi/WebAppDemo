using Microsoft.EntityFrameworkCore;
using OpenIdWebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
        .UseDbContext<DbContext>();
    })
    .AddServer(options =>
    {
        options.AllowClientCredentialsFlow();
        options.SetTokenEndpointUris("token");
        options.AddDevelopmentEncryptionCertificate();
        options.AddDevelopmentSigningCertificate();
        options.DisableAccessTokenEncryption();
        options.UseAspNetCore().EnableTokenEndpointPassthrough();
    });

builder.Services.AddDbContext<DbContext>(options =>
{
    options.UseInMemoryDatabase(nameof(DbContext));
    options.UseOpenIddict();
});

builder.Services.AddHostedService<ClientSeeder>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
