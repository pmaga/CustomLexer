using CustomLexer.Api.Configuration;
using CustomLexer.Api.Services;
using CustomLexer.Infrastructure;
using CustomLexer.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;

namespace CustomLexer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var fileConfigurationSection = Configuration.GetSection(nameof(FileConfiguration));
            services.Configure<FileConfiguration>(fileConfigurationSection);
            services.Configure<FormOptions>(options =>
            {
                FileConfiguration fileConfiguration = new FileConfiguration();
                fileConfigurationSection.Bind(fileConfiguration);
                options.MultipartBodyLengthLimit = fileConfiguration.MaxSizeInBytes;
            });
            services.AddTransient<ILexicalAnalysisService, LexicalAnalysisService>();
            services.AddSingleton<ILexicalParser, SimpleLexicalRegexParser>();
            services.AddSingleton<ITokenizer, RegexTokenizer>();

            var storageConnectionString = Configuration.GetValue<string>("StorageConnectionString");
            services.AddTransient<ITableStorage>(_ => new TableStorage(storageConnectionString));
            services.AddTransient<IStatisticsRepository, StatisticsRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
