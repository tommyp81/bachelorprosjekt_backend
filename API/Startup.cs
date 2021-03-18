using BLL.Interfaces;
using BLL.Repositories;
using DAL.Database_configuration;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Azure;
//using Azure.Storage.Queues; // Må installeres først (NuGet) om den skal brukes
using Azure.Storage.Blobs;
using Azure.Core.Extensions;
using Azure.Storage.Queues;

namespace API
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
            // Database konfigureres i appsettings.json
            services.AddDbContext<DBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AzureDatabase")));

            // Azure Storage Key konfigureres i appsettings.json
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(Configuration["ConnectionStrings:AzureStorageKey:blob"], preferMsi: true);
                //builder.AddQueueServiceClient(Configuration["ConnectionStrings:AzureStorageKey:queue"], preferMsi: true);
            });

            services.AddControllers();

            // For BLL
            services.AddTransient<IUserBLL, UserBLL>();
            services.AddTransient<IPostBLL, PostBLL>();
            services.AddTransient<ICommentBLL, CommentBLL>();
            services.AddTransient<ITopicBLL, TopicBLL>();
            services.AddTransient<ISubTopicBLL, SubTopicBLL>();
            services.AddTransient<ICustomBLL, CustomBLL>();

            // For DAL
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ISubTopicRepository, SubTopicRepository>();
            services.AddScoped<ICustomRepository, CustomRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // For å vise wwwroot/index.html
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    // Bruker kun Blobs for nå, kan legge til Queues om det blir nødvendig?!
    internal static class StartupExtensions
    {
        public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(
            this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
            {
                return builder.AddBlobServiceClient(serviceUri);
            }
            else
            {
                return builder.AddBlobServiceClient(serviceUriOrConnectionString);
            }
        }

        //public static IAzureClientBuilder<QueueServiceClient, QueueClientOptions> AddQueueServiceClient(
        //    this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        //{
        //    if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
        //    {
        //        return builder.AddQueueServiceClient(serviceUri);
        //    }
        //    else
        //    {
        //        return builder.AddQueueServiceClient(serviceUriOrConnectionString);
        //    }
        //}
    }
}
