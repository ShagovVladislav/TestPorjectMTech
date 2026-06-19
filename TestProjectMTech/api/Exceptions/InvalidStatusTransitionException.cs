using TestProjectMTech.api.Domain;

namespace TestProjectMTech.api.Exceptions;

public class InvalidStatusTransitionException : ConflictException
{
    public InvalidStatusTransitionException(Status currentStatus, Status newStatus)
        : base($"Cannot change product status from {currentStatus} to {newStatus}")
    {
    }
}
