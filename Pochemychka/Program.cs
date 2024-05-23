using Pochemychka.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Pochemychka
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.ConfigureServices(services =>
                        {
                            services.AddCors(options =>
                            {
                                options.AddDefaultPolicy(builder =>
                                {
                                    builder.WithOrigins("http://localhost:3000") // Добавьте здесь ваш origin
                                        .AllowAnyMethod()
                                        .AllowAnyHeader();
                                });
                            });
                        });

                        webBuilder.Configure(app =>
                        {
                            app.UseCors();
                            // Другие конфигурации приложения
                        });
                    });
            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<PochemychkaContext>(options =>
                options.UseSqlServer(connectionString));
            //builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<PochemychkaContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.ClaimsIdentity.UserIdClaimType = "id";
            })
              .AddEntityFrameworkStores<PochemychkaContext>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // убедитесь, что у вас есть это
            });
           

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}");
            //app.MapRazorPages();
            //  app.UseIdentity();
            app.Run();
        }
    }
}