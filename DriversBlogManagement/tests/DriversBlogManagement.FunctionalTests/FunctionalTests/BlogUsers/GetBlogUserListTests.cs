namespace DriversBlogManagement.FunctionalTests.FunctionalTests.BlogUsers;

using DriversBlogManagement.SharedTestHelpers.Fakes.BlogUser;
using DriversBlogManagement.FunctionalTests.TestUtilities;
using DriversBlogManagement.Domain;
using System.Net;
using System.Threading.Tasks;

public class GetBlogUserListTests : TestBase
{
    [Fact]
    public async Task get_bloguser_list_returns_success_using_valid_auth_credentials()
    {
        // Arrange
        

        var callingUser = await AddNewSuperAdmin();
        FactoryClient.AddAuth(callingUser.Identifier);

        // Act
        var result = await FactoryClient.GetRequestAsync(ApiRoutes.BlogUsers.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Fact]
    public async Task get_bloguser_list_returns_unauthorized_without_valid_token()
    {
        // Arrange
        // N/A

        // Act
        var result = await FactoryClient.GetRequestAsync(ApiRoutes.BlogUsers.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Fact]
    public async Task get_bloguser_list_returns_forbidden_without_proper_scope()
    {
        // Arrange
        FactoryClient.AddAuth();

        // Act
        var result = await FactoryClient.GetRequestAsync(ApiRoutes.BlogUsers.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}