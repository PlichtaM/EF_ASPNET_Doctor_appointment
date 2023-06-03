using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplication2.Data;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WebApplication2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebApplication2Context") ?? throw new InvalidOperationException("Connection string 'WebApplication2Context' not found.")));

// Dodaj konfigurację sesji
builder.Services.AddSession();

// Dodaj usługi do kontenera.
builder.Services.AddControllersWithViews();

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

// Dodaj middleware uwierzytelniania
app.UseAuthentication();
app.UseAuthorization();

// Dodaj middleware sesji
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
