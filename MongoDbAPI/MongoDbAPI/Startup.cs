using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDbAPI.Models;
using MongoDbAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDbAPI
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
            services.AddControllers();

            //this will configure mapping from appsettings.json file settings StudentStoredatabaseSettings to 
            //the instnace of class StudentDatastoreDatabaseSettings
            services.Configure<StudentDatastoreDatabaseSettings>(
                Configuration.GetSection("StudentStoreDatabaseSettings"));

            //this will configure that whenever the instance of IStudentDatastoreDatabase is requested, then
            //the instance of StudentDatastoreDatabaseSettings will be sent 
            services.AddSingleton<IStudentDatastoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<StudentDatastoreDatabaseSettings>>().Value );

            //here we register MongoClient to dependecy injection contianer and we specify connection string 
            //to our mongoDb database
            services.AddSingleton<IMongoClient>(s =>
                new MongoClient(Configuration.GetValue<string>("StudentStoreDatabaseSettings:ConnectionString")));

            //this is like repository 
            services.AddScoped<IStudentService, StudentService>(); 

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
