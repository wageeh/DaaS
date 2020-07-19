using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appointments.API.BAL;
using Appointments.Entities;
using Core.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Appointments.API
{
    public class Startup
    {
        public static CosmosDBRepository<Appointment> _appointmentContext;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();
            Configuration = builder.Build();

            _appointmentContext = new CosmosDBRepository<Appointment>(Configuration.GetValue<string>(
                "AppointmentsCollection:AccountEndpoint"), Configuration.GetValue<string>(
                "AppointmentsCollection:AccountKey"), Configuration.GetValue<string>(
                "AppointmentsCollection:Database"), Configuration.GetValue<string>(
                "AppointmentsCollection:Collection"), Configuration.GetValue<string>(
                "AppointmentsCollection:PartitionKey"));

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // DependacyInjection
            services.AddSingleton<ICosmosDBRepository<Appointment>>(_appointmentContext);
            // for filling initial data
            FillInitialData();

            // Register the Swagger services
            //services.AddSwaggerDocument();
        }

        private void FillInitialData()
        {
            AppointmentManager appointmentManager = new AppointmentManager(_appointmentContext);
            var list =  appointmentManager.GetAllItemsAsync().Result;
            if (list.Count < 3)
            {
                var x = appointmentManager.CreateAsync(new Appointment()
                {
                    PatientId= "19860813-1111",
                    DoctorId = "201012-1425",
                    AppointmentDate= DateTime.UtcNow.AddDays(1).Date,
                    AppointmentTimeSlot = 1
                }).Result;

                x = appointmentManager.CreateAsync(new Appointment()
                {
                    PatientId = "19750612-2222",
                    DoctorId = "201012-1425",
                    AppointmentDate = DateTime.UtcNow.AddDays(1).Date,
                    AppointmentTimeSlot = 3
                }).Result;

                x = appointmentManager.CreateAsync(new Appointment()
                {
                    PatientId = "19860813-1111",
                    DoctorId = "201012-1425",
                    AppointmentDate = DateTime.UtcNow.AddDays(1).Date,
                    AppointmentTimeSlot = 1
                }).Result;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
