using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using PCME.Api.Infrastructure.AutofacModules;
using PCME.Api.Infrastructure.Filters;
using PCME.Api.Infrastructure.NewtonsoftResolver;
using PCME.Api.Infrastructure.ResourceOwnerPasswordValidator;
using PCME.Infrastructure;
using PCME.Infrastructure.Repositories;
using PCME.MOPDB;
using System;
using System.Collections.Generic;
using System.IO;

namespace PCME.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region 跨域设置
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:1841", "http://60.210.113.43", "http://60.210.113.42","http://pems.zbpe.gov.cn")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    );
            });
            #endregion
            #region api密码token身份认证设置

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(new List<ApiResource>{
                    new ApiResource("api1","myapi")
                })
                .AddInMemoryClients(new List<Client> {
                    new Client{
                        ClientId = "client",
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        ClientSecrets = {
                            new Secret("secret".Sha256())
                        },
                        AllowedScopes = { "api1"},
                        AccessTokenLifetime = 10800,//过期时间秒 3小时
                        AllowedCorsOrigins = { "http://localhost:1841", "http://60.210.113.43", "http://60.210.113.42", "http://pems.zbpe.gov.cn" }
                    }
                });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    //options.Authority = "http://60.210.113.42:5000";
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.JwtValidationClockSkew = TimeSpan.FromSeconds(0);//过期偏移为零
                    options.ApiName = "api1";
                    options.ApiSecret = "secret";
                });
            #endregion

            services.AddMvc(options => {
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
                    .AddDbContext<MOPDBContext>()
                    .AddUnitOfWork<ApplicationDbContext>().AddUnitOfWork<MOPDBContext>();

            services.Configure<ApplicationSettings>(Configuration);

            //services.AddSingleton(new Mapper(new MapperConfiguration(MappingConfiguration.Configure)));
            services.AddAutoMapper();

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();

            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new MeditorModule());
            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllowSpecificOrigin"); //跨域必须设置在这里 一开始这里 切记
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions() {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Files")),
                RequestPath = new PathString("/src")
            });
            app.UseMvc();
        }
    }
}
