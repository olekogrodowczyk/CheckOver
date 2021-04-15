using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckOver.Data;
using Microsoft.AspNetCore.Identity;
using CheckOver.Repository;
using CheckOver.Models;
using CheckOver.Service;
using Azure.Storage.Blobs;

namespace CheckOver
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped(x => new BlobServiceClient(_configuration.GetValue<string>("AzureBlobStorage")));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            });

            services.AddMvc();

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBlobService, BlobService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            //dbInitializer.Initialize();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}