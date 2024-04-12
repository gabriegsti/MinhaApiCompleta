using Asp.Versioning.ApiExplorer;
using AutoMapper;
using DevIO.API.Configuration;
using DevIO.API.Extensions;
using DevIO.Data.Context;
using HealthChecks.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<MeuDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.WebApiConfig();

builder.Services.AddSwaggerConfig();

builder.Services.AddLoggingConfiguration();

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), name: "BancoSql");


builder.Services.AddHealthChecksUI()
    .AddSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));


builder.Services.ResolveDependencies();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //Sets Development option for cors
    app.UseCors("Development");
}
else
{
    //Sets Production option for cors
    app.UseCors("Production");
}
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseAuthentication();

app.UseHsts();

app.UseMiddleware<ExceptionMiddleware>();   

app.UseMvcConfiguration();

app.UseSwaggerConfig(apiVersionDescriptionProvider);

app.UseLoggingConfiguration();

app.UseHealthChecks("/api/health");

app.UseHealthChecksUI(options => 
{
  options.UIPath = "/api/hc-ui";
});

app.MapControllers();
app.Run();
