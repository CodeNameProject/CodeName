namespace BLL.Validation;

public class CustomExeption : Exception
{
    public CustomExeption()
    {
    }

    public CustomExeption(string message)
        : base(message)
    {
    }

    public CustomExeption(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}