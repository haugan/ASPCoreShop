using CoreShop.Data;
using CoreShop.Models;
using CoreShop.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace CoreShop
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>(); // holds SendGrid key for user email verification
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // REGISTER SERVICES WITH DEPENDENCY INJECTION CONTAINER
        public void ConfigureServices(IServiceCollection svc)
        {
            // FRAMEWORK SERVICES
            svc.AddDbContext<ApplicationDbContext>(options =>
                                                   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // IDENTITY
            svc.AddIdentity<ApplicationUser, IdentityRole>(config => 
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            svc.Configure<IdentityOptions>(options =>
            {
                // PASSWORD CONFIG
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // LOCKOUT CONFIG
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // COOKIE CONFIG
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";
                options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOut";

                // USER CONFIG
                options.User.RequireUniqueEmail = false;
            });

            svc.AddMvc();
            svc.AddMemoryCache();
            svc.AddSession();

            // APPLICATION SERVICES
            svc.AddTransient<IEmailSender, AuthMessageSender>();
            svc.AddTransient<ISmsSender, AuthMessageSender>();
            svc.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            svc.AddScoped<ShoppingCart>(svcProvider => ShoppingCart.GetCart(svcProvider));

            // SENDGRID (USER EMAIL VERIFICATION)
            svc.Configure<AuthMessageSenderOptions>(Configuration);
        }

        // ADD MIDDLEWARE COMPONENTS TO HTTP REQUEST PIPELINE
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ApplicationDbContext ctx)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseIdentity();

            // EXTERNAL AUTH MIDDLEWARE BELOW ? https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // SEED DATABASE WITH INITIAL DATA
            DbInitializer.Seed(ctx);
        }
    }
}
