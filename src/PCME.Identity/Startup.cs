using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PCME.Identity
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
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(new List<ApiResource>{
                    new ApiResource("api2","examapi")
                })
                .AddInMemoryClients(new List<Client> {
                    new Client{
                        ClientId = "client",
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        ClientSecrets = {
                            new Secret("secret".Sha256())
                        },
                        AllowedScopes = { "api2"},
                        AccessTokenLifetime = 10800//,//过期时间秒 3小时
                        //AllowedCorsOrigins = { "http://localhost:8888", "http://60.210.113.43", "http://60.210.113.42", "http://pems.zbpe.gov.cn" }
                    }
                });
            //var cors = new DefaultCorsPolicyService(null)
            //{
            //    AllowAll = true
            //};
            //services.AddSingleton<ICorsPolicyService>(cors);
            //services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllowSpecificOrigin");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();
           // app.UseMvc();
        }
    }
}
