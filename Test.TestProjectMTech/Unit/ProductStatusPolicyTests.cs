using FluentAssertions;
using TestProjectMTech.Api.Domain;
using TestProjectMTech.Api.Exceptions;
using TestProjectMTech.Api.Services.Policies;

namespace Test.TestProjectMTech.Unit;

public class ProductStatusPolicyTests
{
    private readonly ProductStatusPolicy _policy = new();

    [TestCase(Status.Active, Status.Active)]
    [TestCase(Status.Active, Status.Defective)]
    [TestCase(Status.Defective, Status.Defective)]
    [TestCase(Status.Defective, Status.WriteOff)]
    [TestCase(Status.WriteOff, Status.WriteOff)]
    public void CanChangeStatus_Should_Return_True_When_Transition_Is_Allowed(
        Status currentStatus,
        Status newStatus)
    {
        var result = _policy.CanChangeStatus(currentStatus, newStatus);

        result.Should().BeTrue();
    }

    [TestCase(Status.Active, Status.WriteOff)]
    [TestCase(Status.Defective, Status.Active)]
    [TestCase(Status.WriteOff, Status.Active)]
    [TestCase(Status.WriteOff, Status.Defective)]
    public void CanChangeStatus_Should_Return_False_When_Transition_Is_Not_Allowed(
        Status currentStatus,
        Status newStatus)
    {
        var result = _policy.CanChangeStatus(currentStatus, newStatus);

        result.Should().BeFalse();
    }

    [Test]
    public void EnsureCanChangeStatus_Should_Throw_InvalidStatusTransitionException_When_Transition_Is_Not_Allowed()
    {
        var act = () => _policy.EnsureCanChangeStatus(Status.Active, Status.WriteOff);

        act.Should().Throw<InvalidStatusTransitionException>()
            .WithMessage("Cannot change product status from Active to WriteOff");
    }
}
