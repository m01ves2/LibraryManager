namespace LibraryManager.Application.Repositories
{
    public interface IRepository<T> where T : class //TODO - Это просто generic-класс для EfBookRepository и EfAuthorRepository. Но,
        //как только появляются write-поддерживающие методы типа GetBookByIsbn (сейчас сделано как GetBookByIsbnQuery-класс) - сразу же нужно разделять
        //на IBookRepository и IAuthorRepository, и в каждом реализовывать свои операции чтения-записи.
        //Шаблонный класс это не догма, и не шаблон. Просто тут вначале это было удобнее
    {
        void Add(T entity);
        void Remove(T entity);
        T? GetById(int id);
    }
}
