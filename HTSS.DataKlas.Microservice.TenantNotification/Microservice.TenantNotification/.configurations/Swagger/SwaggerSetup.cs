﻿using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace Microservice.TenantNotification.configurations.Swagger
{
    internal static class SwaggerSetup
    {
        public static string SwaggerEndpointUrlFactory(SwaggerOptions swaggerOptions)
        {
            return string.IsNullOrEmpty(swaggerOptions?.RelativePath) switch
            {
                true => "/swagger/v1/swagger.json",
                _ => swaggerOptions?.RelativePath
            };
        }

        public static Action<Swashbuckle.AspNetCore.Swagger.SwaggerOptions> SwaggerConfigFactory(
            SwaggerOptions swaggerOptions)
        {
            return !string.IsNullOrEmpty(swaggerOptions?.ServerPath) switch
            {
                true => config => config.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    swaggerDoc.Servers = new List<OpenApiServer> {new OpenApiServer {Url = swaggerOptions.ServerPath}}),
                _ => null
            };
        }
    }
}