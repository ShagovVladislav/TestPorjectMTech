using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.Exceptions;

namespace TestProjectMTech.Api.Services.Policies;

public class ProductStatusPolicy : IProductStatusPolicy
{
    public bool CanChangeStatus(Status currentStatus, Status newStatus)
    {
        if (currentStatus == newStatus)
            return true;

        return (currentStatus, newStatus) switch
        {
            (Status.Active, Status.Defective) => true,
            (Status.Defective, Status.WriteOff) => true,
            _ => false
        };
    }

    public void EnsureCanChangeStatus(Status currentStatus, Status newStatus)
    {
        if (!CanChangeStatus(currentStatus, newStatus))
            throw new InvalidStatusTransitionException(currentStatus, newStatus);
    }
}
