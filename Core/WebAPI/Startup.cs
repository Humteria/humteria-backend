using System.Text;
using Humteria.Application;
using Humteria.Application.Profiles;
using Humteria.Data.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Humteria.WebAPI.Helpers;

namespace Humteria.WebAPI;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) =>
        _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSingleton<IJwtGenerator, JwtTokenHelper>();
        
        services.AddApplicationServices(_configuration);

        services.AddSwaggerGen(
            options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {{
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }});
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Humteria API", Version = "v1" });
            });

        services.AddAutoMapper(typeof(AutoMapperProfile));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(config =>
            {
                string tokenSecret = _configuration.GetValue<string>("JWT:TokenSecret") ?? throw new NullReferenceException("missing token secret!");
                byte[] key = Encoding.UTF8.GetBytes(tokenSecret);
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = _configuration.GetValue<string>("JWT:Issuer"),
                    ValidAudience = _configuration.GetValue<string>("JWT:Audience"),
                    ValidateIssuerSigningKey = true,
                };
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ApplicationServices.ConfigureApplicationServices();

        app.UseRouting();
        
        app.UseCors(policy => policy
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin());

        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
            endpoints.MapControllers());
    }
}
