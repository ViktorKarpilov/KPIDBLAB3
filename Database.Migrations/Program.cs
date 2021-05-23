using FluentMigrator;
using FluentMigrator.Runner;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;

using Polly;
using Polly.Retry;

using Serilog;
using Serilog.Events;

using System;

namespace KPI.DB.Database.Migrations
{
    internal class Program
    {
        private static IConfiguration _configuration;

        private static int Main(string[] args)
        {
            try
            {
                LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                                                      .ReadFrom.Configuration(Configuration);

                loggerConfiguration.WriteTo.Console();

                Log.Logger = loggerConfiguration.CreateLogger();

                IServiceProvider serviceProvider = CreateServices();

                Log.Information("Starting db migrator");

                // Put the database update into a scope to ensure
                // that all resources will be disposed.
                using IServiceScope scope = serviceProvider.CreateScope();

                MigrationDirection migrationDirection = args.Length > 0 ? Enum.Parse<MigrationDirection>(args[0]) : MigrationDirection.Up;

                RetryPolicy retryPolicy = Policy
                         .Handle<NpgsqlException>()
                         .WaitAndRetry(5,
                                       i =>
                                       {
                                           TimeSpan sleepDuration = new TimeSpan(TimeSpan.TicksPerSecond * 3);
                                           Log.Warning("Wait for {duration}s and retry.", sleepDuration.Seconds);
                                           return sleepDuration;
                                       });

                IServiceProvider scopeServiceProvider = scope.ServiceProvider;

                retryPolicy.Execute(() =>
                {
                    try
                    {
                        UpdateDatabase(scopeServiceProvider, migrationDirection);
                    }
                    catch (NpgsqlException exception)
                    {
                        Console.WriteLine("Failed to update database.");
                        if (!exception.IsTransient && exception.InnerException != null)
                        {
                            throw exception.InnerException;
                        }
                        throw;
                    }
                });

                return 0;
            }
            catch (Exception)
            {
                Console.WriteLine("Db migrator terminated unexpectedly");
                return 1;
            }
        }

        /// <summary>
        ///     Configure the dependency injection services
        /// </summary>
        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                  .AddScoped(provider => Configuration)
                  // Add common FluentMigrator services
                  .AddFluentMigratorCore()
                  .ConfigureRunner(runnerBuilder =>
                                       runnerBuilder
                                          // Add Postgressql support to FluentMigrator
                                          .AddPostgres()
                                          // Set the connection string
                                          .WithGlobalConnectionString(provider =>
                                          {
                                              IConfiguration configuration =
                                                  provider.GetRequiredService<IConfiguration>();

                                              string connectionString = configuration.GetConnectionString("migration-db");
#if DEBUG
                                              Log.Information("connectionString = {0}", connectionString);
#endif
                                              return connectionString;
                                          })
                                          // Define the assembly containing the migrations
                                          .ScanIn(typeof(Program).Assembly)
                                          .For.Migrations())
                  // Enable logging to console in the FluentMigrator way
                  .AddLogging(lb => lb.AddFluentMigratorConsole())
                  // Build the service provider
                  .BuildServiceProvider(false);
        }

        private static IConfiguration Configuration
        {
            get
            {
                if (_configuration != null)
                {
                    return _configuration;
                }

                IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                                                      .AddJsonFile("appsettings.json")
                                                      .AddEnvironmentVariables()
                                                      .Build();
                _configuration = configurationRoot;

                return configurationRoot;
            }
        }

        /// <summary>
        ///     Update the database
        /// </summary>
        private static void UpdateDatabase(IServiceProvider serviceProvider, MigrationDirection migrationDirection)
        {
            // Instantiate the runner
            IMigrationRunner runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            if (Log.IsEnabled(LogEventLevel.Debug))
            {
                Console.WriteLine("List all migrations");
                runner.ListMigrations();
            }
            runner.ListMigrations();

            if (migrationDirection == MigrationDirection.Up)
            {
                if (!runner.HasMigrationsToApplyUp())
                {
                    Log.Warning("No applicable migrations to apply up");
                    return;
                }

                Log.Information("Execute all found (and not applied) migrations");
                runner.MigrateUp();
            }
            else
            {
                if (!runner.HasMigrationsToApplyRollback())
                {
                    Log.Warning("No applicable migrations to apply rollback");
                    return;
                }

                Log.Information("Rollback one step");
                runner.Rollback(1);
            }
        }

        //public static void ConfigureLogging(IHostEnvironment hostingEnvironment, IConfigurationRoot configuration)
        //{
        //    LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
        //                                              .ReadFrom.Configuration(configuration);

        //    if (hostingEnvironment.IsLocal())
        //        loggerConfiguration.WriteTo.Console();
        //    else
        //        loggerConfiguration.WriteTo.Console(new CompactJsonFormatter());

        //    Log.Logger = loggerConfiguration.CreateLogger();
        //}
    }
}
