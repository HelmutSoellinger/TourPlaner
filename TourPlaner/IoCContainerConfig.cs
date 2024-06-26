using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TourPlaner.BL;
using TourPlaner.DAL;
using TourPlaner.ViewModels;

namespace TourPlaner
{
    public class IoCContainerConfig
    {
        private readonly IServiceProvider _serviceProvider;
        public IoCContainerConfig()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ITourManager, TourManager>();
            services.AddSingleton<ITourRepository, TourRepository>();
            services.AddSingleton<TourPlanerDbContext, TourPlanerDbContext>();
            services.AddTransient<EditButtonViewModel>();
            services.AddSingleton<MainViewModel>();

            _serviceProvider = services.BuildServiceProvider();
        }

        public MainViewModel MainViewModel => _serviceProvider.GetRequiredService<MainViewModel>();
    }
}
