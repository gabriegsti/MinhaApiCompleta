using Asp.Versioning.ApiExplorer;
using AutoMapper;
using DevIO.API.Configuration;
using DevIO.API.Extensions;
using DevIO.Data.Context;
using HealthChecks.SqlServer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using SqlServerHealthCheck = DevIO.API.Extensions.SqlServerHealthCheck;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();



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

builder.Services.AddLoggingConfiguration(builder.Configuration);


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

//Copiar versão da minhaapi dotnet6 do curso dev.io
//Ponto fundamental, é entender porque a app quebrou. 


app.UseHsts();
app.UseMvcConfiguration();
app.UseLoggingConfiguration();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerConfig(apiVersionDescriptionProvider);



app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run();
