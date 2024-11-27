using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Artifact_Service_Api.Service;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Artifact_Service_Api.Extensions {
    public static class ApiExtentions
    {
        public static void AddAddAuthentication(
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
                        ValidateActor = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["cokes"];
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization();
        }

    }
}



