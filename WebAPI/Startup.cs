using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Models;

namespace WebAPI
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
            //Inject AppSettings so that we can use it any where
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //Injecting AuthenticationContext class in our Application using AddDbcontext from services variable.
            services.AddDbContext<AuthenticationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
            //So from now onwards in order to create instance of AuthenticationContext class we dont have to that using new keyword syntax
            //ASP.NET core will inject this AuthenticationDBContext automatically when we have a constructor parameter of AuthicationContext type.

            //Now after configuring DBContext for Indentity Core  and customising EntityType for user we need to configure IdentityCore.
            services.AddDefaultIdentity<ApplicationUser>() //This function will add common services from IdentityCore to ApplicationUser
               .AddEntityFrameworkStores<AuthenticationContext>();
            //Customising Validations
            services.Configure<IdentityOptions>(options => 
            { options.Password.RequireDigit = false;
              options.Password.RequireNonAlphanumeric = false;
              options.Password.RequiredLength = 4;
              options.Password.RequireLowercase = false;
              options.Password.RequireUppercase = false;
            }
            );
            //Calling cors function for cross origin resource sharing
            services.AddCors();

            //JWT authentication
            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
              builder.WithOrigins(Configuration["ApplicationSettings:Client_Url"].ToString())
              .AllowAnyHeader()
              .AllowAnyMethod());
            //Finally in configure function we need to call UseAuthentication().
            app.UseAuthentication();

            app.UseMvc();
         

         
        }
    }
}
