using Drivers.BLL.Managers;
using Drivers.BLL.Managers.Contracts;
using Drivers.DAL.ADO.Repositories;
using Drivers.DAL.ADO.Repositories.Contracts;
using Drivers.DAL.ADO.UOW;
using Drivers.DAL.ADO.UOW.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Data;
using System.Windows;
using WPFDrivers.ViewModels;

namespace WPFDrivers
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _appHost;

        public App()
        {
            try
            {
                _appHost = Host.CreateDefaultBuilder()

                   //////////////////////////////
                   // НАЛАШТУВАННЯ КОНФІГУРУВАННЯ
                   .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
                   {
                       hostBuilderContext.HostingEnvironment.EnvironmentName = System.Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
                       var env = hostBuilderContext.HostingEnvironment;
                       configurationBuilder.AddEnvironmentVariables();
                       configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                       configurationBuilder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);
                   })

                    ////////////////////////
                    // НАЛАШТУВАННЯ СЕРВІСІВ
                    .ConfigureServices((hostContext, services) =>
                    {
                        // Connection/Transaction for ADO.NET/DAPPER database
                        services.AddScoped((s) => new SqlConnection(hostContext.Configuration.GetConnectionString("MSSQLConnection")));
                        services.AddScoped<IDbTransaction>(s =>
                        {
                            SqlConnection conn = s.GetRequiredService<SqlConnection>();
                            var x = conn.ConnectionString;
                            conn.Open();
                            return conn.BeginTransaction();
                        });

                        // Connection for EF database + DbContext
                        services.AddDbContext<DriversManagementContext>(options =>
                        {
                            string connectionString = hostContext.Configuration.GetConnectionString("MSSQLConnection");
                            options.UseSqlServer(connectionString);
                        });


                        //string connectionString = hostContext.Configuration.GetConnectionString("MSSQLConnection");
                        //services.AddSingleton<IDriversManagementDbContextFactory>(new DriversManagementDbContextFactory(connectionString));

                        // Dependendency Injection for Repositories/UOW from ADO.NET DAL
                        services.AddScoped<IDriverRepository, DriverRepository>();
                        services.AddScoped<ICompanyRepository, CompanyRepository>();
                        services.AddScoped<IPhotoRepository, PhotoRepository>();
                        services.AddScoped<IExpensesRepository, ExpensesRepository>();
                        services.AddScoped<IInspectionRepository, InspectionRepository>();
                        services.AddScoped<ITruckRepository, TruckRepository>();
                        services.AddScoped<IRepairRepository, RepairRepository>();
                        services.AddScoped<IUnitOfWork, UnitOfWork>();


                        // Dependendency Injection for Repositories/UOW from EF DAL
                        services.AddScoped<IEFDriverRepository, EFDriverRepository>();
                        services.AddScoped<IEFCompanyRepository, EFCompanyRepository>();
                        services.AddScoped<IEFExpenseRepository, EFExpenseRepository>();
                        services.AddScoped<IEFInspectionRepository, EFInspectionRepository>();
                        services.AddScoped<IEFPhotoRepository, EFPhotoRepository>();
                        services.AddScoped<IEFRepairRepository, EFRepairRepository>();
                        services.AddScoped<IEFTruckRepository, EFTruckRepository>();
                        services.AddScoped<IEFUnitOfWork, EFUnitOfWork>();

                        // Dependendency Injection for Helpers
                        services.AddScoped<ISortHelper<EFDriver>, SortHelper<EFDriver>>();

                        // Dependendency Injection for Managers
                        services.AddScoped<IDriversManager, DriversManager>();
                        services.AddScoped<ICompanyManager, CompanyManager>();
                        services.AddScoped<IExpenseManager, ExpenseManager>();
                        services.AddScoped<IInspectationManager, InspectationManager>();
                        services.AddScoped<IPhotoManager, PhotoManager>();
                        services.AddScoped<IRepairManager, RepairManager>();
                        services.AddScoped<ITruckManager, TruckManager>();

                        services.AddTransient<MainViewModel>();
                        services.AddSingleton(sp => new MainWindow(sp.GetRequiredService<MainViewModel>()));
                    })
                    .UseSerilog((context, services, loggerConfiguration) =>
                        loggerConfiguration.ReadFrom.Configuration(context.Configuration))
                    .Build();
            }
            catch (Exception ex)
            {
                string type = ex.GetType().Name;
                if (type.Equals("StopTheHostException", StringComparison.Ordinal))
                {
                    throw;
                }

                Log.Fatal(ex, "Unexpected error");
                Application.Current.Shutdown();
            }
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await _appHost.StartAsync();

            using IServiceScope scope = _appHost.Services.CreateScope();
            
            Window mainWindow = scope.ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            try
            {
                if (_appHost != null)
                {
                    await _appHost.StopAsync();
                    _appHost.Dispose();
                }

                Log.CloseAndFlush();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unexpected error");
            }
            finally
            {
                base.OnExit(e);
            }
        }
    }
}
