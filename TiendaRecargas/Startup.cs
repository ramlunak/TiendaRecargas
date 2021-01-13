using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using TiendaRecargas.Data;
using TiendaRecargas.Models.Enums;


namespace TiendaRecargas
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
            services.AddControllersWithViews().AddSessionStateTempDataProvider();

            //Para las TagHelpers Authorization
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddRazorPages().AddSessionStateTempDataProvider();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                   .AddCookie(options =>
                   {
                       options.Cookie.Name = "TiendaRecargas";
                       options.LoginPath = $"/Login/Index";
                       options.AccessDeniedPath = $"/Home/Erro";
                   });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Administrador", pol => pol.RequireClaim(ClaimTypes.Role, "Administrador"));
            //    options.AddPolicy(RolesSistema.Vendedor.ToString(), pol => pol.RequireClaim(ClaimTypes.Role, RolesSistema.Vendedor.ToString()));
            //    options.AddPolicy(RolesSistema.Subvendedor.ToString(), pol => pol.RequireClaim(ClaimTypes.Role, RolesSistema.Subvendedor.ToString()));
            //});

            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true; // false by default
            });

            services.AddDbContext<AppDbContext>(optoins => optoins.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));
            services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new DateTimeConverter()); });

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;

                if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                    response.StatusCode == (int)HttpStatusCode.Forbidden)
                    response.Redirect("/Login");

                //if (response.StatusCode == (int)HttpStatusCode.NotFound)
                //    response.Redirect("/Login/Salir");
            });

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            app.UseExceptionHandler("/Shared/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }

        public class DateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                Debug.Assert(typeToConvert == typeof(DateTime));
                return DateTime.Parse(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
            }
        }

    }
}


//public class ConfigureModelBindingLocalization : IConfigurationOptions<MvcOptions>
//{
//    private readonly IServiceScopeFactory _serviceFactory;
//    public ConfigureModelBindingLocalization(IServiceScopeFactory serviceFactory)
//    {
//        _serviceFactory = serviceFactory;
//    }

//    public void Configure(MvcOptions options)
//    {
//        using (var scope = _serviceFactory.CreateScope())
//        {
//            var provider = scope.ServiceProvider;
//            var localizer = provider.GetRequiredService<IStringLocalizer>();

//            options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) =>
//                localizer["The value '{0}' is not valid for {1}.", x, y]);

//            options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor((x) =>
//                localizer["A value for the '{0}' parameter or property was not provided.", x]);

//            options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() =>
//                localizer["A value is required."]);

//            options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() =>
//                localizer["A non-empty request body is required."]);

//            options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor((x) =>
//                localizer["The value '{0}' is not valid.", x]);

//            options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() =>
//                localizer["The supplied value is invalid."]);

//            options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() =>
//                localizer["The field must be a number."]);

//            options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor((x) =>
//                localizer["The supplied value is invalid for {0}.", x]);

//            options.ModelBindingMessageProvider.SetValueIsInvalidAccessor((x) =>
//                localizer["The value '{0}' is invalid.", x]);

//            options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((x) =>
//                localizer["The field {0} must be a number.", x]);

//            options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor((x) =>
//                localizer["The value '{0}' is invalid.", x]);
//        }
//    }
//}
