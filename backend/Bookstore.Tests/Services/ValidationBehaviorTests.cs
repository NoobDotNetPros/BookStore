using Bookstore.Business.Services;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using ValidationException = Bookstore.Business.Models.ValidationException;

namespace Bookstore.Tests.Services;

[TestFixture]
public class ValidationBehaviorTests
{
    [Test]
    public async Task Handle_WithNoValidators_CallsNextDelegate()
    {
        var validators = new List<IValidator<TestRequest>>();
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var request = new TestRequest();
        var expectedResponse = new TestResponse { Success = true };
        var nextCalled = false;

        RequestHandlerDelegate<TestResponse> next = (cancellationToken) =>
        {
            nextCalled = true;
            return Task.FromResult(expectedResponse);
        };

        var result = await behavior.Handle(request, next, CancellationToken.None);

        Assert.That(nextCalled, Is.True);
        Assert.That(result, Is.EqualTo(expectedResponse));
    }

    [Test]
    public async Task Handle_WithValidRequest_CallsNextDelegate()
    {
        var validatorMock = new Mock<IValidator<TestRequest>>();
        validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var validators = new List<IValidator<TestRequest>> { validatorMock.Object };
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var request = new TestRequest();
        var expectedResponse = new TestResponse { Success = true };

        RequestHandlerDelegate<TestResponse> next = (cancellationToken) => Task.FromResult(expectedResponse);

        var result = await behavior.Handle(request, next, CancellationToken.None);

        Assert.That(result, Is.EqualTo(expectedResponse));
    }

    [Test]
    public void Handle_WithInvalidRequest_ThrowsValidationException()
    {
        var validatorMock = new Mock<IValidator<TestRequest>>();
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Property", "Error message")
        };
        validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var validators = new List<IValidator<TestRequest>> { validatorMock.Object };
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var request = new TestRequest();

        RequestHandlerDelegate<TestResponse> next = (cancellationToken) => Task.FromResult(new TestResponse());

        Assert.ThrowsAsync<ValidationException>(async () =>
            await behavior.Handle(request, next, CancellationToken.None));
    }

    public class TestRequest : IRequest<TestResponse> { }
    public class TestResponse { public bool Success { get; set; } }
}

