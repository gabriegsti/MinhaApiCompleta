using DevIO.API.Extensions;
using Elmah.Io.Extensions.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace DevIO.API.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddElmahIo(o =>
            //{
            //    o.ApiKey = "b2ee58b072fb49cea3b872343e3ac532";
            //    o.LogId = new Guid("f2d47edf-ca7a-4aa2-9d3d-b85a5558c828");
            //});

            //services.AddLogging(builder =>
            //{
            //    builder.AddElmahIo(o =>
            //    {
            //        o.ApiKey = "b2ee58b072fb49cea3b872343e3ac532";
            //        o.LogId = new Guid("f2d47edf-ca7a-4aa2-9d3d-b85a5558c828");
            //    });

            //    builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
            //});

            services.AddHealthChecks()
            //.AddElmahIoPublisher(options =>
            //{
            //    options.ApiKey = "b2ee58b072fb49cea3b872343e3ac532";
            //    options.LogId = new Guid("f2d47edf-ca7a-4aa2-9d3d-b85a5558c828");
            //    options.Application = "Api Fornecedores";
            //})
            .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
            .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSql");


            services.AddHealthChecksUI()
                .AddSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));



            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            //app.UseElmahIo();

            app.UseHealthChecks("/api/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseRouting();

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/api/hc-ui";
                options.ApiPath = $"/api/hc-ui-api";
                options.UseRelativeApiPath = false;
                options.UseRelativeResourcesPath = false;
            });

            return app;
        }
    }
}
