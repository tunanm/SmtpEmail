using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using SmtpEmailDemo.Models;
using SmtpEmailDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Load SMTP settings from configuration
builder.Services.AddOptions<EmailSettings>()
    .Bind(builder.Configuration.GetSection("SmtpSettings"))
    .PostConfigure(settings =>
    {
        settings.UserName = builder.Configuration["SmtpSettings:UserName"];
        settings.Host = builder.Configuration["SmtpSettings:Host"];
        settings.Password = builder.Configuration["SmtpSettings:Password"];
        settings.UserName = builder.Configuration["SmtpSettings:UserName"];
        settings.EnableSsl = builder.Configuration.GetValue<bool>("SmtpSettings:EnableSsl");
        settings.Port = builder.Configuration.GetValue<int>("SmtpSettings:Port");
    });

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Example API",
        Version = "v1",
        Description = "An example of an ASP.NET Core Web API",
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Email = "example@example.com",
            Url = new Uri("https://example.com/contact"),
        },
    });
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();