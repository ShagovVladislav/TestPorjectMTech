namespace TestProjectMTech.api.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message, StatusCodes.Status404NotFound)
    {
    }
}