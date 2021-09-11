namespace Microsoft.AspNetCore.Builder;

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

internal static partial class ApplicationBuilderExtensions
{
    /// <summary>
    /// Register Swagger endpoints.
    /// </summary>
    public static IApplicationBuilder UseSwaggerEndpoints(this IApplicationBuilder app, string routePrefix)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
            c.RoutePrefix = routePrefix;
        });

        return app;
    }
}
