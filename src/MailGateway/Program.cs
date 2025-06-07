using Application.Interfaces;
using Infrastructure.Services.Mail;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IEmailConnectionProvider, EmailConnectionProvider>(); // Register the email service
builder.Services.AddScoped<IEmailSenderService, EmailSendServices>(); // Register the email service
builder.Services.AddScoped<IEmailReaderService, EmailReaderService>(); // Register the email service
builder.Services.AddScoped<IEmailReaderMessageService, EmailReaderMessageService>(); // Register the email service
builder.Services.AddScoped<INotificationStore, NotificationStoreServices>(); // Register the email service

// Configure Entity Framework Core with SQL Server


builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));

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
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

app.Run();
