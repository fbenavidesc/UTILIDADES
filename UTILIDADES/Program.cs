using Microsoft.OpenApi.Models;
using UTILIDADES.Interfaces;
using UTILIDADES.Servicios;
using UTILIDADES.Filters;
using Microsoft.AspNetCore.Mvc;
using UTILIDADES.Modelos;


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Add services to the container.
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EnvioMail API", Version = "v1" });

    // Permitir manejar `IFormFile` correctamente
    c.OperationFilter<FileUploadOperationFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EnvioMail API");
    });
}*/

app.UseAuthorization();

app.MapControllers();

app.Run();
