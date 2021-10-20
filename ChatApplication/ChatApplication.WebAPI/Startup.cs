using AspNetCoreRateLimit;
using ChatApplication.Core.Entities;
using ChatApplication.Core.Options;
using ChatApplication.Core.Services;
using ChatApplication.Core.Util;
using ChatApplication.WebAPI.ErrorHandling;
using ChatApplication.WebAPI.Security;
using ChatApplication.WebAPI.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ChatApplication.WebAPI
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
            string apiVersion = Configuration["ApiVersion"];

            services.AddHttpContextAccessor();

            services.AddDbContext<ChatDbContext>(
                  options => options.UseMySql(Configuration.GetConnectionString("Database")
            ));

            services.AddControllers(x => { x.UseGeneralRoutePrefix($"api/v{apiVersion}"); });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc($"v{apiVersion}", new OpenApiInfo { Title = "Chat API", Version = apiVersion });
                string xmlFile = $"Chat API.xml";
                c.IncludeXmlComments(xmlFile);
            });

            // Needed to load configuration from appsettings.json
            services.AddOptions();

            // Needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            // Load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            // Load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            services.Configure<ApiKeyOptions>(Configuration.GetSection("ApiKeyOptions"));
            services.AddInMemoryRateLimiting();

            // Inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddScoped<ICertificateService, CertificateService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IJwtManager, JwtManager>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IMaliciousAttackManager, MaliciousAttackManager>();

            services.Configure<AuthenticationOptions>(Configuration.GetSection(nameof(AuthenticationOptions)));
            services.Configure<CertificateOptions>(Configuration.GetSection(nameof(CertificateOptions)));

            services.AddAuthentication()
                 .AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler>(Authentication.JwtScheme, null)
                 .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(Authentication.ApiKeyScheme, null)
                 .AddScheme<OtpAuthenticationOptions, OtpAuthenticationHandler>(Authentication.OtpApiKeyScheme, null);

        }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string apiVersion = Configuration["ApiVersion"];

            app.UseIpRateLimiting();
#if USE_DEVELOPMENT
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
        }
#endif
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v{apiVersion}/swagger.json", "Chat API");
            });

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseAuthentication();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
