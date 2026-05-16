
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Personal_Blogging_Platform.Data.Entities;
using Personal_Blogging_Platform.Data.Repositories;
using Personal_Blogging_Platform.Service;
using Serilog;
using System.Text;
using System.Threading.RateLimiting;

namespace Personal_Blogging_Platform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<PostService>();
            builder.Services.AddScoped<PostRepository>();
            builder.Services.AddScoped<AuthRepository>();
            builder.Services.AddScoped<CommentService>();
            builder.Services.AddScoped<CommentRepository>();
            builder.Services.AddScoped<EMailService>();
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))

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
                    Description = "Enter 'Bearer' [space] and then your valid token.\n\nExample: Bearer eyJhbG..."
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
            Array.Empty<string>()
        }
    });
            });

            Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()                         
    .WriteTo.File("logs/log-.txt",             
        rollingInterval: RollingInterval.Day,   
        retainedFileCountLimit: 7)             
    .CreateLogger();
            builder.Host.UseSerilog();
            builder.Services.AddRateLimiter(options =>
            {
               
                options.AddFixedWindowLimiter("AuthPolicy", opt =>
                {
                    opt.PermitLimit = 5;           
                    opt.Window = TimeSpan.FromMinutes(1);  
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 0;            
                });

                
                options.AddFixedWindowLimiter("GeneralPolicy", opt =>
                {
                    opt.PermitLimit = 100;         
                    opt.Window = TimeSpan.FromMinutes(1);  
                    opt.QueueLimit = 0;
                });

                
                options.RejectionStatusCode = 429;  
            });
            var app = builder.Build();
            app.UseExceptionHandler("/error");
            
            app.UseRateLimiter();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
