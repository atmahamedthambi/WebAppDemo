using BlazorWebApp.Components;
using BlazorWebApp.Service;
using BlazorWebApp.Service.IService;
using BlazorWebApp.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

ApiHelper.AuthWebApiUrl = builder.Configuration["AuthWebApiUrl"];
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IHttpRequestService, HttpRequestService>();
builder.Services.AddScoped<IHttpRequestService, HttpRequestService>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // cookie will be expired after 30 minutes and so the logged in user will be removed from httpcontext
        options.LoginPath = "/UserLogin"; // this is the page user will be redirected it is working as expected
        options.AccessDeniedPath = "/AccessDenied";
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireRole("Admin");
    });

    options.AddPolicy("ITSupportEngineer", policy =>
    {
        policy.RequireClaim("department", "it");
        policy.RequireClaim("role", "supportengineer");
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
