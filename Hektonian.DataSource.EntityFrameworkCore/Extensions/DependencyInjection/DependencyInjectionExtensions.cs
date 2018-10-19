using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hektonian.DataSource.EntityFrameworkCore.Internal;
using Hektonian.DataSource.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hektonian.DataSource.EntityFrameworkCore.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkDataSource<TDbContext>(this IServiceCollection services)
        where TDbContext: DbContext => services?.AddScoped<IAsyncDataSource, EfDataSource<TDbContext>>() ?? throw new ArgumentNullException(nameof(services));
    }
}
