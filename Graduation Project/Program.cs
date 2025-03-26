using Graduation_Project.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Graduation_Project.services;

namespace Graduation_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddTransient<IEmailService, EmailService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:3000", "https://graduation-project-sage.vercel.app")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            builder.Services.AddDistributedMemoryCache();



            
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<context>()  // ÊÍÏíÏ ÞÇÚÏÉ ÇáÈíÇäÇÊ
               .AddDefaultTokenProviders();
            



            // builder.Services.AddSession(options =>
            //{
            //  options.IdleTimeout = TimeSpan.FromMinutes(20);
            // options.Cookie.HttpOnly = true;
            // options.Cookie.IsEssential = true;
            //});

            builder.Services.AddControllers();
            builder.Services.AddDbContext<context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("con"));
            });



           

            //------------------------authentication----------------------
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // check jwt token header
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    // unauthorized
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => // check verified key
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["jwt:IssuerIP"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["jwt:AudienceIP"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:secretKey"]))
                };
            });

            #region Swagger Setting
            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET 8 Web API",
                    Description = "Graduation Project"
                });

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] { }
                    }
                });
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowSpecificOrigin");
            //app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            // Adding default admin user
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<context>();

                if (!dbContext.Admins.Any(a => a.Email == "admin@example.com"))
                {
                    dbContext.Admins.Add(new Admin
                    {
                        FirstName = "Mariam",
                        LastName = "Hossam",
                        Email = "Mariam@gmail.com",
                        Password = "mar123"
                    });
                    dbContext.SaveChanges();
                }
            }

            app.Run();
        }
    }
}
