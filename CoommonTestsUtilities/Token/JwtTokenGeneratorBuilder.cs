using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Token;
using Moq;

namespace CoommonTestsUtilities.Token;
public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();
        mock.Setup(config => 
        config.Generate(It.IsAny<User>()))
            .Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkdsYXVjbyBTZXJyYW5vIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiNmI0MzJmOTUtZmFiNi00N2EyLWFmOTItOTA2YjZmNWI5YjIxIiwibmJmIjoxNzI1MTEyNzk2LCJleHAiOjE3MjUxNzI3OTUsImlhdCI6MTcyNTExMjc5Nn0.2yGrAfEjedD3cc85oqJ_l9LRpGDRutAB5zmQ6v5rZrE"
            );

        return mock.Object;
    }
}
