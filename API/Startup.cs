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
using Azure.Storage.Blobs;
using Azure.Core.Extensions;
using DAL.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using API.Auth;

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

            // Allow CORS (for localhost) - Fjærn den kommenterte koden under når du skal kjøre frontend mot lokal database
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
                    });
            });

            services.AddControllers();

            // For JWT tokens autentisering
            // From: https://medium.com/the-innovation/asp-net-core-3-authorization-and-authentication-with-bearer-and-jwt-3041c47c8b1d
            var key = Encoding.ASCII.GetBytes(AuthSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // For BLL
            services.AddTransient<IUserBLL, UserBLL>();
            services.AddTransient<IPostBLL, PostBLL>();
            services.AddTransient<ICommentBLL, CommentBLL>();
            services.AddTransient<ITopicBLL, TopicBLL>();
            services.AddTransient<ISubTopicBLL, SubTopicBLL>();
            services.AddTransient<ICustomBLL, CustomBLL>();
            services.AddTransient<ILikeBLL, LikeBLL>();
            services.AddTransient<IVideoBLL, VideoBLL>();
            services.AddTransient<IInfoTopicBLL, InfoTopicBLL>();

            // For DAL
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ISubTopicRepository, SubTopicRepository>();
            services.AddScoped<ICustomRepository, CustomRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IVideoRepository, VideoRepository>();
            services.AddScoped<IInfoTopicRepository, InfoTopicRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Allow CORS (for localhost) - Fjærn den kommenterte koden under når du skal kjøre frontend mot lokal database
            app.UseCors();

            // For å vise wwwroot/index.html
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            // For JWT tokens autentisering
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

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
