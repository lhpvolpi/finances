namespace Finances.Core.Contexts.SharedContext.Entities;

public class NotFoundException : Exception
{
    private const string DefaultErrorMessage = "Entity not found";

    public NotFoundException(string message = DefaultErrorMessage) : base(message) { }

    public static void ThrowIfNull<T>(T entity, string message = DefaultErrorMessage)
    {
        if (entity is null)
            throw new NotFoundException(message);
    }

    public static void ThrowIfDefault<T>(T entity, T defaultValue, string message = DefaultErrorMessage)
    {
        if (EqualityComparer<T>.Default.Equals(entity, defaultValue))
            throw new NotFoundException(message);
    }
}

public class AlreadyExistsException : Exception
{
    private const string DefaultErrorMessage = "Already exists";

    public AlreadyExistsException(string message = DefaultErrorMessage) : base(message) { }

    public static void ThrowIfNotNull<T>(T entity, string message = DefaultErrorMessage)
    {
        if (entity is not null)
            throw new AlreadyExistsException(message);
    }
}