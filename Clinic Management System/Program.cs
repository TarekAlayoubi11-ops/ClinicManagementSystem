
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using ClinicManagementSystem.Authorization;


namespace ClinicManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ClincManagementSystemApiCorsPolicy", policy =>
                {
                    policy
                        .WithOrigins(
                            "https://localhost:7208",
                            "http://localhost:5126"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,


            ValidateAudience = true,


            ValidateLifetime = true,


            ValidateIssuerSigningKey = true,


            ValidIssuer = "ClincManagementSystemApi",

            ValidAudience = "ClincManagementSystemApiUsers",



            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_123456"))
        };
    });
            builder.Services.AddSwaggerGen(options =>
            {

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",


                    Type = SecuritySchemeType.Http,



                    Scheme = "Bearer",


                    BearerFormat = "JWT",


                    In = ParameterLocation.Header,


                    Description = "Enter: Bearer {your JWT token}"
                });



                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },



            new string[] {}
        }
    });
            });
            builder.Services.AddAuthorization();

            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy("AuthLimiter", httpContext =>
                {
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: ip,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                });
            });

            builder.Services.AddSingleton<IAuthorizationHandler, UserOwnerOrAdminHandler>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("UserOwnerOrAdmin", policy =>
                    policy.Requirements.Add(new UserOwnerOrAdminRequirement()));
            });

            builder.Services.AddControllers();





            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("ClincManagementSystemApiCorsPolicy");

            app.UseRateLimiter();
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    await context.Response.WriteAsync("Too many login attempts. Please try again later.");
                }
            });


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
