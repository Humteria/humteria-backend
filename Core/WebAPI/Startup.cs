using Humteria.Data;
using Humteria.Data.Services;
using Humteria.Application.Profiles;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Humteria.WebAPI;

public class Startup
{
    public IConfiguration Configuration { get; } 

    public Startup(IConfiguration configuration) =>
        Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddDatabaseServices(Configuration);

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
                string _TokenSecret = Configuration.GetValue<string>("JWT:TokenSecret") ?? throw new NullReferenceException("missing token secret!");
                var key = Encoding.UTF8.GetBytes(_TokenSecret);
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = Configuration.GetValue<string>("JWT:Issuer"),
                    ValidAudience = Configuration.GetValue<string>("JWT:Audience"),
                    ValidateIssuerSigningKey = true,
                };
            });

                services.AddTransient<IMainInterface, MainSQLServices>();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ApplicationServices.ConfigureDatabaseServices();

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
