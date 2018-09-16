# DataSource

This project is an queryable alternative to the repository -pattern.

It is important to note that while you can use the `EFCore` and `InMemory` -packages in your own projects, they are not the implementations you *have to* use. They only really exist to provide example of how to implement a data source to your data store.

## NuGet packages

| Package | Nuget |
| --- | --- |
| [Hektonian.DataSource](https://www.nuget.org/packages/Hektonian.DataSource/) | [![NuGet](https://img.shields.io/nuget/v/Hektonian.DataSource.svg)](https://www.nuget.org/packages/Hektonian.DataSource/) |
| [Hektonian.DataSource.EntityFrameworkCore](https://www.nuget.org/packages/Hektonian.DataSource.EntityFrameworkCore) | [![NuGet](https://img.shields.io/nuget/v/Hektonian.DataSource.EntityFrameworkCore.svg)](https://www.nuget.org/packages/Hektonian.DataSource.EntityFrameworkCore) |
| [Hektonian.DataSource.InMemory](https://www.nuget.org/packages/Hektonian.DataSource.InMemory) | [![NuGet](https://img.shields.io/nuget/v/Hektonian.DataSource.svg)](https://www.nuget.org/packages/Hektonian.DataSource.InMemory) |

## Rationale

All of this comes down to personal preference, but I find that the repository pattern is a clunky thing to work with:

#### Over/under fetching

Repositories are extremely susceptible to retrieving too much or too little data. Frankly, I'd be surprised if any decent application that uses repositories didn't have to deal with this issue.

#### Rigidness

The repository pattern is very rigid and doesn't work well with special cases.
While the specification -pattern alleviates this, it doesn't fully resolve the issue. The implementations rarely if ever do or can perform SELECT and JOIN -queries. Not to mention you'll have a bunch of glorified filter functions posing as classes.

#### Repository is a special case

The reason for why I claim this should be clearly visible a [further down below](#as-a-data-provider-for-repositories) but the short version is that anything a repository (or unit of work) does is doable with data sources.

#### Repository violates separation of concerns

Big claim from a nobody.

In DDD the repository interface is defined in the domain, and the implementation is done in the infrastructure. Sounds good, right?

Probably not.

I'm sure that you've seen it in projects that use the repository pattern: the base class or interface that every business entity inherits/implements. The one that tells the repository what is the key to compare agains? You know the guy.

When a repository is queried for a entity, it is given an identifying key (single or composite) that should match an entity in the store. Repository goes through the store and *compares the given key with the keys of entities in the store*.
The repository has dictated to any business logic that "any entities I use should be found in this manner".

*"But the repository interface, entity base class/interface and business entities are all defined in the business logic/domain! They define how data access should find the entity!"*
They define the key(s), yes, but not how those keys are compared.

*"I use specifications to define what keys should be used and how they're compared"*
You now have multiple classes that should've been a simple delegate.

*"I have this library that does things X, Y and Z..."*
What you have is an extra dependency that most likely has even more dependencies. Have fun managing that.

*"This library is an extra dependecy"*
That it is. So copy-paste it to your project. The `Hektonian.DataSource` package is literally four interfaces and nothing else. It's not like I'll (or can for that matter) come down on you with a lawyer in tow if you do.

"I don't want to use IQueryable"
Drop that attitude right this second and go on an adventure. You may surprise yourself.

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

### As a data provider for repositories

***NOT RECOMMENDED***

It is entirely possible to use data source for repositories. This will, however, remove any and all advantages a data source has over repositories.

There really isn't any reason to do this to your repository, but it will serve as an example how to implement data source as a repository -like service.

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
