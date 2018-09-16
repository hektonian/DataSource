# DataSource

This project is an queryable alternative to the repository -pattern.

It is important to note that the `EFCore` and `InMemory` -projects are not the implementations you *have to* use. They only really exist to provide example of how to implement a data source to your data store.

## NuGet packages

| Package | Nuget |
| --- | --- |
| [Hektonian.DataSource](https://www.nuget.org/packages/Hektonian.DataSource/) | [![NuGet](https://img.shields.io/nuget/v/Hektonian.DataSource.svg)](https://www.nuget.org/packages/Hektonian.DataSource/) |
| [Hektonian.DataSource.EntityFrameworkCore](https://www.nuget.org/packages/Hektonian.DataSource.EntityFrameworkCore) | [![NuGet](https://img.shields.io/nuget/v/Hektonian.DataSource.EntityFrameworkCore.svg)](https://www.nuget.org/packages/Hektonian.DataSource.EntityFrameworkCore) |
| [Hektonian.DataSource.InMemory](https://www.nuget.org/packages/Hektonian.DataSource.InMemory) | [![NuGet](https://img.shields.io/nuget/v/Hektonian.DataSource.svg)](https://www.nuget.org/packages/Hektonian.DataSource.InMemory) |

## Rationale

All of this comes down to personal preference, but I find that the repository pattern is a clunky thing to work with:

* Over/under fetching: Repositories are extremely susceptible to retrieving too much or too little data. Frankly, I'd be surprised if any decent application that uses repositories didn't have to deal with this issue.
* Rigidness: The repository pattern is very rigid and doesn't work well with special cases. While the specification -pattern alleviates this, it doesn't fully resolve the issue, the implementation can rarely if ever perform SELECT and JOIN -queries, and will fill your code with glorified filter functions.
* Sillyness: In DDD the repository interface is defined in the domain, and the implementation is done in the infrastructure. Sounds good, right? Wrong. You're leaving business logic up to data access -layer. Everytime you implement a repository you're implementing a small but ever-so important portion of the business logic: the comparison of primary keys. Base entity for your entities doesn't select your keys for you, your repository does when it inspects the store and selects the database entity's property to compare given key(s) against.

## How to use

### As an alternative to repositories

```
// Entities/Entity.cs
public class Entity
{
  public int Id { get; set; }
  public string Name { get; set; }
}

...

// Interfaces/IService.cs
public interface IService
{
  Task<IEnumerable<Entity>> GetAllAsync();
  Task<Entity> FindByIdAsync(int entityId);
  Task<IEnumerable<Entity>> SearchByNameAsync(string nameFragment);
  Task<T> CreateAsync(Entity entity);
}

// Services/Service.cs
public class Service : IService
{
  // Instead of a repository, use a data source
  private readonly IAsyncDataSource _source;
  
  public Service(IAsyncDataSource source)
  {
    _source = source;
  }
  
  public Task<IEnumerable<Entity>> GetAllAsync()
  {
    return _source
              .Set<Entity>()
              .GetAllAsync(data => data);
  }
  
  Task<Entity> FindByIdAsync(int entityId)
  {
    return _source
              .Set<Entity>()
              .FirstOrDefaultAsync(entity => entity.Id ==entityId);
  }
  
  Task<IEnumerable<Entity>> SearchByNameAsync(string nameFragment)
  {
    return _source
              .Set<Entity>()
              .GetAllAsync(data =>
                data.Where(entity => entity.Name?.Contains(nameFragment) ?? false)
              );
  }
  
  Task<T> CreateAsync(Entity entity)
  {
    return _source
              .MutateAsync(async mutable =>
              {
                await mutable.AddAsync(entity);
                await mutable.SaveChangesAsync();
                return entity;
              });
  }
}

// E.g. Startup.cs
public class Startup
{
...
  public void ConfigureServices(IServiceCollection services)
  {
    ...
    // services.AddInMemoryDataSource(); // Adds the in-memory -data source and uses the default in-memory store
    // services.AddEntityFrameworkDataSource<AppDbContext>(); // Adds entity framework core -data source and uses the given DbContext
    // services.AddScoped<IDataSource, YourDataSourceImplementatino>();
    ...
  }
...
}

```

### As a data provider for repositories (not recommended)

It is entirely possible to use data source for repositories. This will, however, remove all the advantages a data source has over repositories.

There really isn't any reason to do this, but it will serve as an example how to implement data source as a repository -like service.

```
public class YourAsyncRepository, IYourAsyncRepository
{
  private readonly IAsyncDataSource _source;
  
  public YourRepository(IAsyncDataSource source)
  {
    _source = source;
  }
  
  public Task<IEnumerable<T>> GetAllAsync<T>() where T : class, IYourEntityBaseClass
  {
    return _source
              .Set<T>()
              .GetAllAsync(data => data);
  }
  
  public Task<T> GetAsync(int id) where T : class, IYourEntityBaseClass
  {
    return _source
              .Set<T>()
              .FirstOrDefaultAsync(entity => entity.Id == id);
  }
  
  public Task<T> AddAsync<T>(T entity)
  {
    return _source
            .MutateAsync(async mutable => 
            {
              await mutable.AddAsync(entity);
              await mutable.SaveChangesAsync();
              return entity;
            });
  }
  
  public Task<T> UpdateAsync<T>(T entity) where T : class, IYourEntityBaseClass
  {
    return _source
            .MutateAsync(async mutable =>
            {
              await mutable.UpdateAsync(entity);
              await mutable.SaveChangesAsync();
              return entity;
            });
  }
  
  public Task DeleteAsync<T>(int id) where T : class, IYourEntityBaseClass
  {
    return _source
          .MutateAsync(async mutable =>
          {
            await mutable.DeleteAsync(data => data.Where(entity.Id == id));
            await mutable.SaveChangesAsync();
          });
  }
}

// E.g. Startup.cs
public class Startup
{
...
  public void ConfigureServices(IServiceCollection services)
  {
    ...
    // services.AddInMemoryDataSource(); // Adds the in-memory -data source and uses the default in-memory store
    // services.AddEntityFrameworkDataSource<AppDbContext>(); // Adds entity framework core -data source and uses the given DbContext
    ...
  }
...
}
```
