using Application.Interfaces;
using Application.Interfaces.Mail;
using Application.Interfaces.Seguridad;
using Domain.Entidades.Mail;
using Domain.Entidades.Seguridad;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Seeders.Core;
using Infrastructure.Repository;
using Infrastructure.Seguridad;
using Infrastructure.Services;
using Infrastructure.Services.Mail;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Shared.Helper;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        //options.AccessDeniedPath = "/Acceso/Denegado";
    });
// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

builder.Services.AddScoped<IEmailConnectionProvider, EmailConnectionProvider>();

// Register the email service
builder.Services.AddScoped<IEmailSenderService, EmailSendServices>(); // Register the email service
builder.Services.AddScoped<IEmailReaderService, EmailReaderService>(); // Register the email service
builder.Services.AddScoped<IEmailReaderMessageService, EmailReaderMessageService>(); // Register the email service
builder.Services.AddScoped<INotificationStore, NotificationStoreServices>(); // Register the email service
builder.Services.AddScoped<ICredentialProvider, CredentialProvider>();
builder.Services.AddScoped<UsuarioAutenticado>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<UsuarioRepositoy>();
builder.Services.Configure<CryptoSettings>(builder.Configuration.GetSection("Crypto"));
builder.Services.AddSingleton<CryptoHelper>();

//builder.Services.AddHostedService<EmailBackgroundService>(); // Register the background service for email processing
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Agrega log en consola
builder.Logging.AddDebug();   // Para ver en Output (Visual Studio)

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.Migrate();

    // Registrar seeders
    SeederRegistry.RegisterAll();
    // Ejecutar todos los seeders
    DatabaseSeeder.SeedAll(context);
}



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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

app.Run();
