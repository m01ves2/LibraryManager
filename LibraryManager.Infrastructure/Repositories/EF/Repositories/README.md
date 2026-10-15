# Repository with Expressions — Learning Notes

## 1. The Main Question

Our generic repository exposes the following method:

```csharp
IReadOnlyList<T> Find(
    Expression<Func<T, bool>> predicate,
    int skip,
    int take);
```

In `EfRepository<T>`, its implementation is very simple:

```csharp
public IReadOnlyList<T> Find(
    Expression<Func<T, bool>> predicate,
    int skip,
    int take)
{
    return _dbSet
        .Where(predicate)
        .Skip(skip)
        .Take(take)
        .ToList();
}
```

**Question:** How can we implement the same interface using a completely different storage mechanism, such as a JSON file, without EF Core or SQL?

## 2. EF Core Implementation

When working with `DbSet<T>`, LINQ expressions are processed by the EF Core query provider.

The execution flow:

1. Application creates an expression describing the filtering condition.
2. Repository passes that expression to EF Core.
3. EF Core analyzes the expression tree and translates the query into SQL.
4. The database performs filtering and pagination.
5. `ToList()` triggers execution and materializes the results.

For example:

```csharp
b => b.AuthorId == 5
```

can be translated into a SQL condition:

```sql
WHERE AuthorId = 5
```

The filtering happens in the database, not in application memory.

## 3. FileRepository Implementation

Suppose the same entities are stored in a JSON file.

A simplified implementation could look like this:

```csharp
public IReadOnlyList<T> Find(
    Expression<Func<T, bool>> predicate,
    int skip,
    int take)
{
    List<T> items = LoadFromFile();

    return items
        .Where(predicate.Compile())
        .Skip(skip)
        .Take(take)
        .ToList();
}
```

Here, `LoadFromFile()` represents reading and deserializing the JSON data.

Unlike EF Core, this repository does not translate expressions into SQL.

Instead, `predicate.Compile()` converts the expression tree into an executable `Func<T, bool>` delegate.

LINQ to Objects then filters the collection in memory.

Alternatively:

```csharp
return items
    .AsQueryable()
    .Where(predicate)
    .Skip(skip)
    .Take(take)
    .ToList();
```

This also works, but `AsQueryable()` does not magically create a database query provider. The collection is still processed in memory.

## 4. Comparison

| Feature | EfRepository | FileRepository |
|---|---|---|
| Storage | Database | JSON file |
| Expression processing | EF Core query provider | Compiled delegate |
| Filtering | Database | Application memory |
| Pagination | Database | Application memory |
| Data loading | Required rows | Usually entire collection |
| SQL translation | Yes | No |

## 5. Architectural Conclusion

Application depends only on the repository interface:

```csharp
IRepository<Book>
```

It does not know whether the data comes from SQLite, JSON files, or an in-memory collection.

The same expression:

```csharp
b => b.AuthorId == 5
```

can be handled by completely different infrastructure implementations.

This demonstrates **Dependency Inversion and the separation of abstraction from implementation**.

However, there is an important limitation: not every expression supported by LINQ to Objects can be translated into SQL by EF Core.

Therefore, interchangeable repository implementations must agree not only on method signatures but also on the supported query semantics.

### Key takeaway

**An Expression describes a computation. The query provider or repository implementation determines how that computation is executed.**

In this experiment, the repository interface provides flexibility without exposing EF Core directly to Application, but it does not provide all the capabilities of `IQueryable<T>`.