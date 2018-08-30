using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
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
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(CheckPostParametersFilter));
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                //Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter
                //{
                //    DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
                //});

            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //options.SerializerSettings.ContractResolver = new LowercaseContractResolver();
            }
            );

            //配置身份认证服务
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(new List<ApiResource>{
                    new ApiResource("api2","examapi")
                })
                .AddInMemoryClients(new List<Client> {
                    new Client{
                        ClientId = "clientexam",
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        ClientSecrets = {
                            new Secret("secret".Sha256())
                        },
                        AllowedScopes = { "api2"},
                        AccessTokenLifetime = 10800//,//过期时间秒 3小时
                        //AllowedCorsOrigins = { "http://localhost:8888", "http://60.210.113.42:8888", "http://pems.zbpe.gov.cn:8888" }
                    }
                });
                //.AddTestUsers(new List<TestUser> { new TestUser { SubjectId = "1", Username = "abc", Password = "111111" } });
            //配置身份验证
            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://60.210.113.42:8000";
                    //options.Authority = "http://localhost:8000";
                    options.RequireHttpsMetadata = false;
                    options.JwtValidationClockSkew = TimeSpan.FromSeconds(0);//过期偏移为零
                    options.ApiName = "api2";
                    options.ApiSecret = "secret";
                });
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


            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseSqlServer(Configuration["ConnectionString"],
                            sqlServerOptionsAction: sqlOptions =>
                            {
                                //sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                //sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            });
                    },
                        ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                    )
                    .AddDbContext<VideoDbContext>(options =>
                    {
                        options.UseSqlServer(Configuration["ConnectionString_Video"]);
                    })
                    .AddDbContext<TestDBContext>(options =>
                    {
                        options.UseSqlServer(Configuration["ConnectionString_Test"]);
                    })
                    .AddDbContext<KSDBContext>()
                    .AddUnitOfWork<ApplicationDbContext>();


            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllowSpecificOrigin");//跨域必须设置在这里 一开始这里 切记
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
