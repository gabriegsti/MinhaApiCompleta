using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace DevIO.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection WebApiConfig(this IServiceCollection services)
        {
            //Desable formatting and validation and automatic errors
            // More control, in change of verify the modelState in all methods of the controller. 
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services
            .AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            //Always needed if it is not minimal api.
            .AddMvc();

            //Allow cors for development
            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    builder =>
                    {
                        builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });

                options.AddDefaultPolicy(
                    builder =>                   
                        builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed(hostName => true));

                options.AddPolicy("Production",
                    builder =>
                    {
                        // It's always possible to restrict by Header, methods, origins and etc
                        builder
                        //Always able to write more methods
                        .WithMethods("GET","POST")
                        //Always able to write more domains
                        .WithOrigins("http://desenvolvedor.io")
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader();

                    });
            });

         

            return services;
        }

        public static IApplicationBuilder UseMvcConfiguration(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

           
            app.UseAuthorization();


            return app;
        }


    }
}
