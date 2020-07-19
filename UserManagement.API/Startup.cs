using System.Linq;
using Core.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserManagement.API.BAL;
using UserManagement.Enitites;

namespace UserManagement.API
{
    public class Startup
    {
        public static CosmosDBRepository<Doctor> _doctorContext;
        public static CosmosDBRepository<Patient> _patientContext;
       
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            _doctorContext = new CosmosDBRepository<Doctor>(Configuration.GetValue<string>(
                "DoctorCollection:AccountEndpoint"), Configuration.GetValue<string>(
                "DoctorCollection:AccountKey"), Configuration.GetValue<string>(
                "DoctorCollection:Database"), Configuration.GetValue<string>(
                "DoctorCollection:Collection"), Configuration.GetValue<string>(
                "DoctorCollection:PartitionKey"));


            _patientContext = new CosmosDBRepository<Patient>(Configuration.GetValue<string>(
                "PatientCollection:AccountEndpoint"), Configuration.GetValue<string>(
                "PatientCollection:AccountKey"), Configuration.GetValue<string>(
                "PatientCollection:Database"), Configuration.GetValue<string>(
                "PatientCollection:Collection"), Configuration.GetValue<string>(
                "PatientCollection:PartitionKey"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // DependacyInjection
            services.AddSingleton<ICosmosDBRepository<Doctor>>(_doctorContext);
            services.AddSingleton<ICosmosDBRepository<Patient>>(_patientContext);
            // for filling initial data
            FillInitialData();

            // Register the Swagger services
            services.AddSwaggerDocument();
        }

        private void FillInitialData()
        {
            DoctorManager doctorManager;
            PatientManager patientManager;
            var list = _doctorContext.GetItemsAsync(y => y.Name != "").Result.ToList();
            if (list.Count < 3)
            {
                doctorManager = new DoctorManager(_doctorContext);
                var x = doctorManager.CreateAsync(new Doctor()
                {
                    Name = "Mikael Seström",
                    Email = "wageeh.gerges@gmail.com",
                    ItemId = "201012-1425",
                    Speciality = new Speciality()
                    {
                        Id = "3",
                        Name = "Clinical radiology"
                    },
                    Description = ""
                });

                x = doctorManager.CreateAsync(new Doctor()
                {
                    Name = "Carina Axel",
                    Email = "wageeh.gerges@gmail.com",
                    ItemId = "200911-1758",
                    Speciality = new Speciality()
                    {
                        Id = "2",
                        Name = "Anaesthesia"
                    },
                    Description = ""
                });

                x = doctorManager.CreateAsync(new Doctor()
                {
                    Name = "Martin Eriksson",
                    Email = "wageeh.gerges@gmail.com",
                    ItemId = "199005-1875",
                    Speciality = new Speciality()
                    {
                        Id = "1",
                        Name = "Clinical oncology"
                    },
                    Description = ""
                });
            }

            var list2 = _patientContext.GetItemsAsync(y => y.Name != "").Result.ToList();
            if (list2.Count < 3)
            {
                patientManager = new PatientManager(_patientContext);
                var x = patientManager.CreateAsync(new Patient()
                {
                    Name = "Henrik Karlsson",
                    Email = "wageeh.gerges@gmail.com",
                    ItemId = "19860813-1111",
                    Address = new Address()
                    {
                        ItemId = "111",
                        ZipCode = "121211"
                    },
                    Description = ""
                });

                x = patientManager.CreateAsync(new Patient()
                {
                    Name = "Erik Henriksson",
                    Email = "wageeh.gerges@gmail.com",
                    ItemId = "19750612-2222",
                    Address = new Address()
                    {
                        ItemId = "112",
                        ZipCode = "121211"

                    },
                    Description = ""
                });

                x = patientManager.CreateAsync(new Patient()
                {
                    Name = "Cecilia Eliasson",
                    Email = "wageeh.gerges@gmail.com",
                    ItemId = "19600519-3333",
                    Address = new Address()
                    {
                        ItemId = "113",
                        ZipCode = "122122"
                    },
                    Description = ""
                });
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseStaticFiles();

            // for swagger
            app.UseOpenApi(); // serve OpenAPI/Swagger documents
            app.UseSwaggerUi3(); // serve Swagger UI
            app.UseReDoc(); // serve ReDoc UI

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
