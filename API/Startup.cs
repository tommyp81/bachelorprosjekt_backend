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
            });

            services.AddControllers();

            // For JWT tokens autentisering
            var jwtTokenConfig = Configuration.GetSection("AuthSettings").Get<AuthSettings>();
            services.AddSingleton(jwtTokenConfig);
            var key = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = true;
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    RequireExpirationTime = false
                };
            });

            services.AddSingleton<ITokenService, TokenService>();

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

            // For å vise wwwroot/index.html
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            // For JWT tokens autentisering
            app.UseRouting();
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
    }
}
