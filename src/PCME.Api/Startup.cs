using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PCME.Api.Infrastructure.AutofacModules;
using PCME.Infrastructure;
using System;
using System.Collections.Generic;

namespace PCME.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
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
                    builder => builder.WithOrigins("http://localhost:1841", "http://192.168.4.102:5000").AllowAnyMethod().AllowAnyHeader());
            });
            #endregion
            services.AddMvc();

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
                        AllowedScopes = { "api1"}
                    }
                });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:6000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api1";
                    options.ApiSecret = "secret";
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
                    ).AddUnitOfWork<ApplicationDbContext>();

            services.Configure<ApplicationSettings>(Configuration);

            //services.AddSingleton(new Mapper(new MapperConfiguration(MappingConfiguration.Configure)));
            services.AddAutoMapper();

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

            app.UseMvc();
        }
    }
}
