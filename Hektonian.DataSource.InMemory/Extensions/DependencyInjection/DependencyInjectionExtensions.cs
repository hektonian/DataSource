using System;
using System.Collections.Generic;
using System.Text;
using Hektonian.DataSource.InMemory.Interfaces;
using Hektonian.DataSource.InMemory.Internal;
using Hektonian.DataSource.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Hektonian.DataSource.InMemory.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInMemoryDataSource<T>(this IServiceCollection services)
        where T : class, IInMemoryDataStore => services.AddSingleton<IInMemoryDataStore, T>()
                                                .AddScoped<IAsyncDataSource, InMemoryDataSource>();

        public static IServiceCollection AddInMemoryDataSource(this IServiceCollection services)
            => services.AddInMemoryDataSource<InMemoryDataStore>();
    }
}