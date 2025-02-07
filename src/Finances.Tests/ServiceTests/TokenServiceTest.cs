using Finances.Core.Contexts.SharedContext.Entities;
using Finances.Core.Contexts.UserContext.Entities;
using Finances.Infra.Services;

namespace Finances.Tests.ServiceTests;

public class TokenServiceTest
{
    private readonly Mock<IOptions<Settings>> _settingsMock;
    private readonly TokenService _tokenService;

    public TokenServiceTest()
    {
        this._settingsMock = new Mock<IOptions<Settings>>();

        var settings = new Settings
        {
            Authentication = new AuthenticationSettings
            {
                SecretKey = "14e148ddfa462edd9e9f6084d4f70d12aefc2452abe6c880b6151ee416321ca0",
                Audience = "testAudience",
                Issuer = "testIssuer",
                ExpiresInHours = 1
            }
        };

        this._settingsMock.Setup(i => i.Value).Returns(settings);
        this._tokenService = new TokenService(this._settingsMock.Object);
    }

    [Fact]
    public void Generate_SuccessfulTokenGeneration()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid()
        };

        // Act
        var (token, expires) = this._tokenService.Generate(user);

        // Assert
        Assert.NotNull(token);
        Assert.True(expires > DateTime.UtcNow);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(this._settingsMock.Object.Value.Authentication.SecretKey);

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = this._settingsMock.Object.Value.Authentication.Issuer,
            ValidateAudience = true,
            ValidAudience = this._settingsMock.Object.Value.Authentication.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var userIdClaim = jwtToken.Claims.First(i => i.Type == "user_id").Value;

        Assert.Equal(user.Id.ToString(), userIdClaim);
    }

    [Fact]
    public void Generate_ShouldThrowException_WhenSettingsAreInvalid()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid()
        };

        var settingsErrorMock = new Mock<IOptions<Settings>>();
        var settingsError = new Settings
        {
            Authentication = new AuthenticationSettings
            {
                SecretKey = "testSecretKeyError",
                Audience = "testAudience",
                Issuer = "testIssuer",
                ExpiresInHours = 1
            }
        };

        settingsErrorMock.Setup(i => i.Value).Returns(settingsError);

        var tokenService = new TokenService(settingsErrorMock.Object);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => tokenService.Generate(user));
    }
}