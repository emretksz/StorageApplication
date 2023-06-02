//using Application.Mappings.AutoMapper;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using StorageApplication.Helpers;

namespace WebApp
{
    public class Startup
    {
        //public IConfiguration Configuration { get; }
        public IConfiguration configuration { get; }
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration _configuration, IWebHostEnvironment env)
        {
            configuration = _configuration;
            _env = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opt => {
            opt.Cookie.Name = "TEST";
                opt.Cookie.HttpOnly = true;
                opt.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                opt.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
                opt.ExpireTimeSpan = TimeSpan.FromSeconds(99999);
                opt.Cookie.IsEssential = true;
                opt.ReturnUrlParameter = "/Account/SignIn";
                opt.LogoutPath = "/Account/SignIn";
                opt.LoginPath = "/Account/SignIn";
                opt.AccessDeniedPath= "/Account/SignIn";
            });

            services.AddRazorPages();
       
            //services.AddMvc().AddRazorRuntimeCompilation();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddHttpContextAccessor();
            services.AddSession();

            //var configuration = new MapperConfiguration(opt =>
            //{
            //    opt.AddProfile(new ProductProfile());
                
            //});

            //var mapper = configuration.CreateMapper();
            //services.AddSingleton(mapper);

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(99999);
                options.Cookie.HttpOnly = true;
     
            });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 100000000;
                options.ValueLengthLimit = 134217728;
                options.MultipartHeadersCountLimit = 1999129;
                options.MultipartHeadersLengthLimit = 1241121;
            });
            var dependencyModule = new ConfigurationsService();
            dependencyModule.RegisterServices(services);
     

        }
       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsStaging())
            {
             
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions() { FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot").Replace("\\", "/")) });
            
            app.UseRouting();
            app.UseSession();
        
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "areaRoute",
                   pattern: "{area:exists}/{controller}/{action}"
               );
                endpoints.MapControllerRoute(
                    name: "default",
                    //pattern: "{controller=Home}/{action=Index1}/{id?}");
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

