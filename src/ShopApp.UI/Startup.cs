using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete;
using ShopApp.DataAccess.Context;
using ShopApp.UI.Identity;

namespace ShopApp.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true; //parola mutlaka sayısal değer içermek zorundadır
                options.Password.RequireLowercase = true; // küçük karakter olmazı zorunlu
                options.Password.RequiredLength = 6; //min 6 karakter
                options.Password.RequireNonAlphanumeric = true; //mutlaka alfanumeric karakter zorunlu 
                options.Password.RequireUppercase = true; //mutlaka büyük harf zorunlu

                options.Lockout.MaxFailedAccessAttempts = 5; //5 kere yanlış şifre girebilme hakkı veriyoruz.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);  //5 dk boyunca kullanıcı giriş yapamaz.
                options.Lockout.AllowedForNewUsers = true;

                //options.User.AllowedUserNameCharacters = ""; //belirli karakter belitebiliriz.
                options.User.RequireUniqueEmail = true; //mail adresi kontorlü yapar.

                options.SignIn.RequireConfirmedEmail = true; //mail addresi onayı gerkiyor.
                options.SignIn.RequireConfirmedPhoneNumber = false; //telefon numrası ile onay gerekiyor.
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60); //60 dk tarayıcıda cookie saklanır.
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = "ShopApp.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<ShopContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, CartService>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddTransient<IEmailSender, EmailService.EmailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "adminProducts",
                    template: "admin/products",
                    defaults: new { Controller = "Admin", Action = "ProductList" });

                routes.MapRoute(
                    name: "adminProducts",
                    template: "admin/products/{id?}",
                    defaults: new { Controller = "Admin", Action = "EditProduct" });

                routes.MapRoute(
                    name: "cart",
                    template: "cart",
                    defaults: new { Controller = "Cart", Action = "Index" });

                routes.MapRoute(
                    name: "orders",
                    template: "orders",
                    defaults: new { Controller = "Cart", Action = "GetOrders" });

                routes.MapRoute(
                    name: "checkout",
                    template: "checkout",
                    defaults: new { Controller = "Cart", Action = "Checkout" });

                routes.MapRoute(
                    name: "products",
                    template: "products/{category?}",
                    defaults: new { Controller = "Shop", Action = "List" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            SeedIdentity.Seed(userManager, roleManager, Configuration).Wait();
        }
    }
}
