using DevIO.API.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services)
        {
            // Add all controllers
            services.AddControllers();

            services
            .AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            //Disable formatting and validation and automatic errors
            // More control, in change of verify the modelState in all methods of the controller. 
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //Allow cors for development
            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    builder =>
                    {
                        builder
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowAnyHeader();
                       
                    });

                //options.AddDefaultPolicy(
                //    builder =>                   
                //        builder
                //        .AllowAnyMethod()
                //        .AllowAnyHeader()
                //        .AllowCredentials()
                //        .SetIsOriginAllowed(hostName => true));

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

        public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {

            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                //Sets Development option for cors
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //Sets Production option for cors
                app.UseCors("Production");
                app.UseHsts();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //Gera o endpoint que retornara os dados que serao utilizados no dashboard
                endpoints.MapHealthChecks("/api/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                
                endpoints.MapHealthChecksUI(options =>
                {
                    //Where UI api will be server by the browser
                    options.UIPath = "/api/hc-ui";
                    options.ResourcesPath = "/api/hc-ui-resources";

                    options.UseRelativeApiPath = false;
                    options.UseRelativeResourcesPath = false;
                    options.UseRelativeWebhookPath = false;
                });
            });
            return app;
        }


    }
}
