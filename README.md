# DataSource
### An alternative to the repository and unit of work -patterns.

It is important to note that while you can use the `EntityFrameworkCore` and `InMemory` -packages in your own projects, they are not the implementations you ___have to___ use. They exist to provide an example on how you would go on to implement a data source to your data store.

***This project promises to provide only the `Hektonian.DataSource` -package.***

## NuGet packages

| Package | Nuget |
| --- | --- |
| [Hektonian.DataSource](https://www.nuget.org/packages/Hektonian.DataSource/) | [![NuGet](https://img.shields.io/nuget/v/Hektonian.DataSource.svg)](https://www.nuget.org/packages/Hektonian.DataSource/) |
| [Hektonian.DataSource.EntityFrameworkCore](https://www.nuget.org/packages/Hektonian.DataSource.EntityFrameworkCore) | [![NuGet](https://img.shields.io/nuget/v/Hektonian.DataSource.EntityFrameworkCore.svg)](https://www.nuget.org/packages/Hektonian.DataSource.EntityFrameworkCore) |
| [Hektonian.DataSource.InMemory](https://www.nuget.org/packages/Hektonian.DataSource.InMemory) | [![NuGet](https://img.shields.io/nuget/v/Hektonian.DataSource.svg)](https://www.nuget.org/packages/Hektonian.DataSource.InMemory) |

## Rationale

I know all of this comes down to personal preference so please take this with a grain of salt, but I absolutely HATE working with repositories.

I mean, sure, on paper all actions you can take on a set of data can be filtered down to the basic CRUD. But to take the five actions of CRUD (List, Get, Create, Update, Delete) and make that *THE* thing you access your store with is a bad idea. A good start, for sure, but a really REALLY bad idea. Especially so if you're working with relational databases.

***Repositories do not represent anything how a modern data store should be queried or manipulated.***

So! With all that out of the way (and probably with a few new enemies made) here's some points to why repositories are a bad idea:

### Repositories tend to over/under fetch

Repositories are extremely susceptible to retrieving too much or too little data. Frankly, I'd be surprised if any decent application that uses repositories didn't have to deal with this issue.

***"I don't see any problem in that"***  
I hope you'll be able to say that should your application ever serve several thousand requests per second.

### Repositories are inflexible

The repository pattern is very rigid and doesn't work well with complex queries.
While the specification -pattern alleviates this to a degree, it doesn't fully resolve the issue. Specification implementations can rarely, if ever, perform SELECT and JOIN mid-query.  
Not to mention you'll have a folder full of glorified filter functions posing as classes with at least one per entity, most likely more than that.  

***"Specifications allow me to do a single exact same query in multiple places"***  
Fair enough. But you do realize that specification is a design pattern and not a repository -only thing, right? You can use specifications with data sources as well. Which brings me to the next point...

### Repository is a special case

The reason for why I claim this should be clearly visible a [further down below as I make a repository -like service with a data source](#as-a-data-provider-for-repositories) but the short version is that anything a repository (or unit of work) does is doable with data sources.

### Repository violates separation of concerns

Big claim from a nobody.

The repository interface is defined in the business logic/domain, and the implementation is done in the data access/infrastructure. Sounds good, right? Probably if it weren't a repository.

***"But the repository interface, entity base class/interface and business entities are all defined in the business logic/domain! They define how data access should find the entity!"***  
They define the key(s), yes, but not how the key(s) are compared.

Let me try to explain myself:

I'm sure that you've seen it in projects that use the repository pattern: A base class or interface that every business entity inherits/implements.  
The one that tells the repository what is the key to compare against? You know, something like one or both of these guys:
```csharp
public interface IBaseEntity<TKey>
{
  TKey Id { get; set; }
}

public class BaseEntity<TKey>
{
  public TKey Id { get; set; }
}
```

When a repository is queried for a entity, it is given an identifying key (single or composite) that should match an entity in the store.
Repository goes through the store and **compares the given key with the keys of entities in the store and selects the appropriate key**.
Something along these lines:
```csharp
public class EfRepositoryImplementation<T, TKey> : IAsyncRepository<T>
where T : IBaseEntity<TKey>
{
...
  public async Task<T> FindById(TKey id)
  {
    return await _dbContext.Set<T>()
      .FirstOrDefaultAsync( // <- This should be decided on by the business logic.
	entity => entity.Id == id // <- Comparison operation decided for business logic.
      );
  }
...
}
```

Those two lines in repository have dictated to the business logic what entities business logic should receive.

***"The comparison is just equality, right?"***  
Right. Except we have no way to know if data access/infrastructure has defined that comparison correctly, selects the data the correct way, or that it even compares it to the correct data.
Let's take the previous example and make a terrible developer redo it:

```csharp
public class EfRepositoryImplementation<T, TKey> : IAsyncRepository<T>
where T : IBaseEntity<TKey>
{
...
  public async Task<T> FindById(TKey id)
  {
    return await _dbContext.Set<T>()
      .LastAsync( // <- Why this should be a business logic decision.
	entity => entity.Id != id // <- Why comparison in data access is a problem.
      );
  }
...
}
```
I admit that this is nitpicking and that business logic/domain services can can never have complete control over data access/infrastructure, but with data sources business logic/domain services gain better control over this.

***"I use specifications to define what keys should be used and how they're compared"***  
You now have multiple classes that should've been a simple delegate.

***"I have this library that does things X, Y and Z..."***  
What you have is an extra dependency that most likely has even more dependencies. Have fun managing that.

***"This library is an extra dependecy"***  
That it is. So copy-paste it to your project. The `Hektonian.DataSource` package is four interfaces and nothing else. It's not like I can (or will, for that matter) come down on you with a lawyer in tow if you do. You may improve, extend and expand upon it as you like.

***"If I want to use an ORM like EFCore for example, this would be an abstraction on abstraction just like repository and unit of work -patterns"***  
Ok, I'm going to make another big claim: All ORM are implementations with implemented functionality that cannot be decided on by the user. If they have implemented functionality, they can be made abstract.

***"I don't want to use IQueryable/LINQ/Your sh\*tty two-bit library"***  
Drop that attitude. Go on an adventure. You may very well surprise yourself.

***"Why so nitpicky about this?"***  
Because I, for one, want to be able to program and develop and not be stuck with an out-dated design pattern that doesn't allow something I need to do with it. Also because I probably got a screw loose.

***"This is a bad idea/bad practice"***  
Please, enlighten me on the whats, whys and hows. I'm a two-bit developer with lots of bad ideas, I want to know what's wrong with those ideas and I need some honest feedback to do that.

## Usage

### As an alternative to repositories

How data sources should be used. Implementation is left up to the user.

```csharp
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
              .SingleOrDefaultAsync(entity => entity.Id == entityId);
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
    // services.AddScoped<IDataSource, YourDataSourceImplementation>();
    ...
  }
...
}

```

### As a data provider for repositories

***NOT RECOMMENDED***

It is entirely possible to use data source for repositories. This will, however, remove any and all advantages data sources have over repositories.

There really isn't any reason to do this, but it will serve as an example how to implement data source in a repository -like service.

```csharp
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
              .SingleOrDefaultAsync(entity => entity.Id == id);
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
            await mutable.DeleteAsync(entity => entity.Id == id);
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
    // services.AddScoped<IDataSource, YourDataSourceImplementation>();
    ...
  }
...
}
```

## Migrations

### From <=v1.0.2 to v1.0.3

From v1.0.3 onwards all IAsyncReadOnlyDataSet's `LastAsync` and `LastOrDefaultAsync` -methods and the expression filter versions of `FirstAsync` and `FirstOrDefaultAsync` will be marked as obsolete to promote deterministic selection of entities from a data set.

The deprecated methods are either duplicate functionality or rely on the order the entities are stored in the data store.

##### Affected areas

The following methods are duplicate functionality. Reverse the order and use FirstAsync() and FirstOrDefaultAsync()  

```csharp
interface IAsyncReadOnlyDataSet<TSource>
  LastAsync<TOutput>(Func<IQueryable<TSource>, IQueryable<TOutput>> queryBuilder)
  LastOrDefaultAsync<TOutput>(Func<IQueryable<TSource>, IQueryable<TOutput>> queryBuilder)
```

The following methods trust the order data store stores it's entities. You should never trust the order of elements in a data store.  

```csharp
interface IAsyncReadOnlyDataSet<TSource>
  FirstAsync(Expression<Func<TSource, bool>> condition)
  FirstOrDefaultAsync(Expression<Func<TSource, bool>> condition)
  LastAsync(Expression<Func<TSource, bool>> condition)  
  LastOrDefaultAsync(Expression<Func<TSource, bool>> condition)  
```

If you're using any of these methods, here's what to replace them with based on the how large the query result set is:

| Result set | Use                                                    |
| ---------- | ------------------------------------------------------ |
| 1          | Single(expression)                                     |
| 0..1       | SingleOrDefault(expression)                            |
| 1..N       | First(queryBuilder with ORDER BY ASC or DESC)          |
| 0..N       | FirstOrDefault(queryBuilder with ORDER BY ASC or DESC) |

## Contributors

Maintained by [hektonian](https://github.com/hektonian)
Feature requests and bug reports welcome.

## Support, suggestions & feedback

You can get in touch on gitter: https://gitter.im/Hektonian-DataSource