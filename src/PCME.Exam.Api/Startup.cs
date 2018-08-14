using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PCME.Exam.Api.Infrastructure.Filters;
using PCME.Exam.Api.Infrastructure.ResourceOwnerPasswordValidator;
using PCME.Infrastructure;
using PCME.KSDB;
namespace PCME.Exam.Api
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

            #region 跨域设置
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.AllowAnyOrigin() //builder.WithOrigins("http://localhost:8888", "http://60.210.113.43", "http://60.210.113.42", "http://pems.zbpe.gov.cn")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    );
            });
            #endregion
            //services.AddMvc();
            services.AddMvcCore()
            .AddAuthorization()
            .AddJsonFormatters();
            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(typeof(CheckPostParametersFilter));
            //    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            //    //Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter
            //    //{
            //    //    DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
            //    //});

            //}).AddJsonOptions(options =>
            //{
            //    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            //    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //    //options.SerializerSettings.ContractResolver = new LowercaseContractResolver();
            //}
            //);
            //配置身份验证
            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    //options.Authority = "http://60.210.113.42:5000";
                    options.Authority = "http://localhost:10000";
                    options.RequireHttpsMetadata = false;
                    options.JwtValidationClockSkew = TimeSpan.FromSeconds(0);//过期偏移为零
                    options.ApiName = "api2";
                    options.ApiSecret = "secret";
                });

            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));
            //});

            //services.AddEntityFrameworkSqlServer()
            //        .AddDbContext<ApplicationDbContext>(options =>
            //        {
            //            options.UseSqlServer(Configuration["ConnectionString"],
            //                sqlServerOptionsAction: sqlOptions =>
            //                {
            //                    //sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
            //                    //sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            //                });
            //        },
            //            ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
            //        )
            //        .AddDbContext<VideoDbContext>(options =>
            //        {
            //            options.UseSqlServer(Configuration["ConnectionString_Video"]);
            //        })
            //        .AddDbContext<TestDBContext>(options => {
            //            options.UseSqlServer(Configuration["ConnectionString_Test"]);
            //        })
            //        .AddDbContext<KSDBContext>()
            //        .AddUnitOfWork<ApplicationDbContext>();

            //services.Configure<ApplicationSettings>(Configuration);

            var cors = new DefaultCorsPolicyService(null)
            {
                AllowAll = true
            };
            services.AddSingleton<ICorsPolicyService>(cors);

            //services.AddSingleton(new Mapper(new MapperConfiguration(MappingConfiguration.Configure)));
            //services.AddAutoMapper();

            //services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            //services.AddTransient<IProfileService, ProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
             //跨域必须设置在这里 一开始这里 切记
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseAuthentication();
            app.UseCors("AllowSpecificOrigin");
            //配置identityServer授权
            //app.UseIdentityServer(new IdentityServerAuthenticationOptions()
            //{
            //    Authority = "http://localhost:10000",
            //    RequireHttpsMetadata = false,
            //    JwtValidationClockSkew = TimeSpan.FromSeconds(0),//过期偏移为零
            //    ApiName = "api2",
            //    ApiSecret = "secret"
            //});

            //

            //app.UseDefaultFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Files")),
            //    RequestPath = new PathString("/src")
            //});
            app.UseMvc();
        }
    }
}
