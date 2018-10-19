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
        public static IServiceCollection AddInMemoryDataSource<TDataStore>(this IServiceCollection services)
        where TDataStore: class, IInMemoryDataStore
            => services?.AddSingleton<IInMemoryDataStore, TDataStore>()
                       ?.AddScoped<IAsyncDataSource, InMemoryDataSource>()
            ?? throw new ArgumentNullException(nameof(services));

        public static IServiceCollection AddInMemoryDataSource(this IServiceCollection services)
            => services?.AddInMemoryDataSource<InMemoryDataStore>()
            ?? throw new ArgumentNullException(nameof(services));
    }
}