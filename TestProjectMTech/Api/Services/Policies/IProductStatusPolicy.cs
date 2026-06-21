using TestProjectMTech.Api.Domain;

namespace TestProjectMTech.Api.Services.Policies;

public interface IProductStatusPolicy
{
    bool CanChangeStatus(Status currentStatus, Status newStatus);
    void EnsureCanChangeStatus(Status currentStatus, Status newStatus);
}
