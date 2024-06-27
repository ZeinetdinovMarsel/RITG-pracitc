﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TMS.API.Endpoints;
using TMS.Application;
using TMS.Core.Enums;
using TMS.Infrastructure;
namespace TMS.API.Extentions
{
    public static class ApiExtentions
    {
        public static void AddMappedEndpoints(
            this IEndpointRouteBuilder app)
        {
            app.MapTsksEndpoints();
            app.MapUsersEndpoints();
        }

        public static void AddApiAuthentification(
            this IServiceCollection services,
            IOptions<JwtOptions> jwtOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["jwt"];
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddScoped<IPermissionService, PermissionService>();
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            
            services.AddAuthorization();
        }

        public static IEndpointConventionBuilder RequirePermissions<TBuilder>(
            this TBuilder builder, params Permission[] permissions)
            where TBuilder : IEndpointConventionBuilder
        {
            return builder.RequireAuthorization(policy =>
            policy.AddRequirements(new PermissionRequirement(permissions)));
        }
    }
}
