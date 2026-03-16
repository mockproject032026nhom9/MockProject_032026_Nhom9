using Microsoft.Extensions.DependencyInjection;
using ToDoList.Service.Contracts;
using ToDoList.Service.Implementations;

namespace ToDoList.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITaskService, TaskService>();

            return services;
        }
    }
}
