using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApplication1
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
                    builder => builder.AllowAnyOrigin()
                    //builder => builder.WithOrigins("http://60.210.113.43","http://localhost:1841")
                    //builder=>builder.WithOrigins("http://192.168.1.101:1841")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("authorization", "Content-Type", "Cookie", "x-requested-with", "Set-Cookie")
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
                        AccessTokenLifetime = 10800//,//,//过期时间秒 3小时
                        //AllowedCorsOrigins = { "http://60.210.113.43", "http://localhost:1841" }
                    }
                }).AddTestUsers(new List<TestUser>
                    {
                        new TestUser
                        {
                            SubjectId = "1",
                            Username = "a",
                            Password = "1111"
                        }
                    });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://60.210.113.42";
                    options.RequireHttpsMetadata = false;
                    options.JwtValidationClockSkew = TimeSpan.FromSeconds(0);//过期偏移为零
                    options.ApiName = "api1";
                    options.ApiSecret = "secret";
                });
            #endregion

            services.AddMvc();
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
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Files")),
                RequestPath = new PathString("/src")
            });
            app.UseMvc();
        }
    }
}
