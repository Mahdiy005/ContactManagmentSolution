
using ContactManagment.Models;
using ContactManagment.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ContactManagment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add services to the container.
            builder.Services.AddDbContext<ContactContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("myQueryString")).UseLazyLoadingProxies();
            });

            //------
            builder.Services.AddIdentity<User, IdentityRole<int>>(option =>
            {
                //option.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ContactContext>()
            .AddDefaultTokenProviders();

            //-----
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            //----- Cystom Service 
            builder.Services.AddScoped<JWTTokenService>();

            //---- Register Cors middleware -> all consumer can show it 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFront", ploicy =>
                {
                    ploicy.AllowAnyOrigin();
                });
            });
            //========= JWT Config
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
            options.SaveToken = true; // ensute that token i created it , by making reverse engineer
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"]??""))
                };
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowFront");

            app.MapControllers();

            app.Run();
        }
    }
}
