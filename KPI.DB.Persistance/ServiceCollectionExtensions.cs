using KPI.DB.Persistance.Configurations;
using KPI.DB.Persistance.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace KPI.DB.Persistance
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddRepositories();

            return services;
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionAccessor, ConnectionAccesor>();
            services.AddTransient<IHomeAssignmentRepository, HomeAssignmentRepository>();
            services.AddTransient<IPersonsRepository, PersonsRepository>();
            services.AddTransient<ILessonsRepository, LessonsRepository>();
            services.AddTransient<IGroupsRepository, GroupsRepository>();
        }
    }
}
