Question:
А теперь один вопрос для закрепления, прежде чем перейдём к CQRS.
Представь, что у нас появился FileRepository<Book>, который хранит книги в JSON.
Можем ли мы реализовать тот же Find(Expression<Func<Book, bool>> predicate, ...), если никакого EF Core и SQL там нет?
Подсказка: ответ уже фактически содержится в приведённом выше примере InMemory. :D
===========================================

Answer:
сейчас в EF работа выглядит:
  public class EfRepository<T> : IRepository<T> where T : class
  {
      protected readonly LibraryDbContext _context;
      protected readonly DbSet<T> _dbSet;

      public EfRepository(LibraryDbContext context)
      {
          _context = context;
          _dbSet = _context.Set<T>();
      }

      public void Add(T entity) => _dbSet.Add(entity);

      public void Remove(T entity) => _dbSet.Remove(entity);

      public T? GetById(int id) => _dbSet.Find(id);

      public IReadOnlyList<T> Find( Expression<Func<T, bool>> predicate, int skip, int take)
      {
          return _dbSet
              .Where(predicate)
              .Skip(skip)
              .Take(take)
              .ToList();
      }

      public int Count(Expression<Func<T, bool>> predicate)
      {
          return _dbSet.Count(predicate);
      }

      public bool Any(Expression<Func<T, bool>> predicate)
      {
          return _dbSet.Any(predicate);
      }
  }

то есть по факту, хоть мы вызываем методы репозитория, они транслируются в соответствующие методы EF. тут работает не мы ручками, а сам EF(кажется, это называется LINQ over EF).
если же писать такую же штуку но для FileRepository<T>, то придется метод 
 FileRepository<T>.Find(Expression<Func<Book, bool>> predicate, ...) писать вручную. причем, кажется, сначала считать файл, распознать записанные данные, сохранить в коллекцию, и уже потом через LINQ применять Where(predicate). я правильно понимаю?


Answer:
Да! Именно так. Ты совершенно правильно понял разницу. :D
Причём здесь мы подошли к самой сути Expression<Func<T, bool>> и того, зачем вообще нужны провайдеры LINQ.
Разберём два случая.
1. EfRepository — выполнение через EF Core
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


Здесь происходит следующее:
1. Where(predicate) получает дерево выражения.
2. EF Core анализирует его и строит соответствующий запрос к базе данных.
3. Skip() и Take() также становятся частями SQL-запроса.
4. ToList() инициирует выполнение запроса.
5. EF Core получает результаты и материализует объекты C#.
То есть фильтрацию, пагинацию и выборку выполняет сама база данных, а не наше приложение.
Точнее, это называется LINQ to Entities (через провайдер EF Core).
2. FileRepository — выполнение в памяти
Представим, что книги хранятся в JSON-файле.
Твой сценарий абсолютно правильный:
books.json

Чтение файла + десериализация
List<Book>

LINQ to Objects
Where → Skip → Take → ToList


Реализация могла бы выглядеть примерно так:
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


Обрати внимание на:
predicate.Compile()


Этот метод превращает дерево выражения в обычный исполняемый делегат Func<T, bool>.
После этого Enumerable.Where() просто последовательно проверяет элементы коллекции.
Мы могли бы вместо Compile() написать:
return items
    .AsQueryable()
    .Where(predicate)
    .Skip(skip)
    .Take(take)
    .ToList();


Но принципиально это ничего не меняет: в случае обычной коллекции LINQ работает с объектами в памяти. Никакого SQL не появляется.
3. И вот важный архитектурный вывод
Наш IRepository<T> действительно позволяет заменить EF Core на файловое хранилище без изменения AddBookUseCase.
Однако одинаковый интерфейс не означает одинаковый способ выполнения операций.
	EfRepository	FileRepository
Хранение	База данных	Файл
Интерпретация Expression	EF Core	Компиляция в делегат
Фильтрация	В базе данных	В памяти
Пагинация	В базе данных	В памяти
Загрузка данных	Только необходимые записи	Обычно вся коллекция
Производительность на больших объёмах	Может использовать индексы	Обычно полный перебор
И ещё одна существенная тонкость: универсальный FileRepository<T> потребует не только Find(), но и реализации загрузки, сохранения, идентификации сущностей и поведения Commit(). Причём сериализация сложной доменной модели с навигационными свойствами — отдельная задача.
Но это уже детали Infrastructure.

Самое интересное здесь то, что Application совершенно не знает, какой механизм выполняет выражение.
Он просто передаёт:
b => b.AuthorId == 5


А Infrastructure решает, что с этим делать.
Именно такую гибкость мы и пытались получить в третьем экспериментальном подходе.
Правда, у неё есть цена: не каждое выражение, работающее в памяти, EF Core сможет перевести в SQL.
Думаю, теперь Repository + Expressions можно считать разобранным. Остался CQRS — самый близкий к архитектуре наших последних проектов.


Question:
а почему можно писать как predicate.Compile(), так и просто  predicate? там что, перегрузка метода есть?
===========================================
Answer
Да! Именно перегрузка методов. Причём ты сейчас нащупал очень важное различие между Enumerable и Queryable в LINQ. :D

Вспомним два варианта.

1. predicate.Compile() — LINQ to Objects
List<Book> books = LoadFromFile();

return books
    .Where(predicate.Compile())
    .Skip(skip)
    .Take(take)
    .ToList();

Здесь вызывается метод расширения из System.Linq.Enumerable:

Enumerable.Where<T>(
    IEnumerable<T> source,
    Func<T, bool> predicate)

Он принимает обычный делегат Func<T, bool>.

Но у нас predicate имеет тип:

Expression<Func<Book, bool>>

Поэтому мы вызываем .Compile(), превращая дерево выражения в исполняемый делегат.

2. predicate — LINQ to Queryable
return books
    .AsQueryable()
    .Where(predicate)
    .Skip(skip)
    .Take(take)
    .ToList();

Здесь используется другая перегрузка Where, из System.Linq.Queryable:

Queryable.Where<T>(
    IQueryable<T> source,
    Expression<Func<T, bool>> predicate)

Она принимает непосредственно дерево выражения.

Поэтому .Compile() не нужен!

Сравним
Enumerable.Where

Queryable.Where

Источник

IEnumerable<T>

IQueryable<T>

Предикат

Func<T, bool>

Expression<Func<T, bool>>

Что получает

Исполняемый код

Дерево выражения

Кто обрабатывает

LINQ to Objects

LINQ-провайдер

3. А что происходит внутри AsQueryable()?
Вот здесь особенно интересно.

Для обычного List<Book>:

books.AsQueryable()

создаётся LINQ-провайдер для коллекции в памяти (EnumerableQuery<T>).

Он способен обрабатывать деревья выражений, но никакого SQL не генерирует.

А у EF Core:

_context.Books.Where(predicate)

источником является DbSet<Book>, который реализует IQueryable<Book> и использует провайдер EF Core.

EF Core анализирует дерево выражения и переводит поддерживаемые операции в SQL.

Таким образом, один и тот же вызов:

query.Where(predicate)

может выполняться принципиально по-разному в зависимости от LINQ-провайдера.

4. И ещё одна тонкость
Почему компилятор вообще понимает, какой Where() вызвать?

Потому что он учитывает статический тип источника и тип аргумента.

IEnumerable<Book> a = books;
IQueryable<Book> b = books.AsQueryable();

a.Where(predicate.Compile()); // Enumerable.Where
b.Where(predicate);           // Queryable.Where

А вот так:

a.Where(predicate);

не скомпилируется: IEnumerable<T> не имеет подходящей перегрузки Where, принимающей Expression<Func<T, bool>>.

Итог: Compile() нужен не потому, что мы работаем с файлом или базой данных, а потому, что выбранный метод Where() ожидает Func<T, bool>, а не Expression<Func<T, bool>>.