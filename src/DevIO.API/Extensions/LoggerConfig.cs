
using Elmah.Io.Extensions.Logging;

namespace DevIO.API.Extensions
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "b2ee58b072fb49cea3b872343e3ac532";
                o.LogId = new Guid("f2d47edf-ca7a-4aa2-9d3d-b85a5558c828");
            });

            services.AddLogging(builder =>
            {
                builder.AddElmahIo(o =>
                {
                    o.ApiKey = "b2ee58b072fb49cea3b872343e3ac532";
                    o.LogId = new Guid("f2d47edf-ca7a-4aa2-9d3d-b85a5558c828");
                });

                builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
            });

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }
    }
}
