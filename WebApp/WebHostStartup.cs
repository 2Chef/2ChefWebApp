using Microsoft.Extensions.FileProviders;
using System.Text.Json;

namespace WebApp
{
    public class WebHostStartup
    {
        public IConfiguration Configuration { get; }

        public WebHostStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            });
            services.AddControllersWithViews();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "wwwroot", "miniapp")),
                RequestPath = "/miniapp"
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile(Configuration["DefaultRoute"]
                    ?? throw new InvalidOperationException("Не задана настройка appsettings DefaultRoute"));
            });
        }

        /// <summary>
        /// В чем проблема, Telegram отправлеят нам Update, но... его поля описаны в snake_case.
        /// А Telegram.Bot.Update описан в PascalCase. Вот так вот
        /// </summary>
        private class SnakeCaseNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name)
            {
                return string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + char.ToLower(x) : char.ToLower(x).ToString()));
            }
        }
    }
}
