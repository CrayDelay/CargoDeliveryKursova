namespace DriversManagement.UnitTests.Domain.RolePermissions;

using DriversManagement.Domain;
using DriversManagement.Domain.RolePermissions;
using DriversManagement.Wrappers;
using DriversManagement.Domain.RolePermissions.Models;
using DriversManagement.Domain.Roles;
using Bogus;
using ValidationException = DriversManagement.Exceptions.ValidationException;

public class CreateRolePermissionTests
{
    private readonly Faker _faker;

    public CreateRolePermissionTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void can_create_valid_rolepermission()
    {
        // Arrange
        var permission = _faker.PickRandom(Permissions.List());
        var role = _faker.PickRandom(Role.ListNames());

        // Act
        var newRolePermission = RolePermission.Create(new RolePermissionForCreation()
        {
            Permission = permission,
            Role = role
        });
        
        // Assert
        newRolePermission.Permission.Should().Be(permission);
        newRolePermission.Role.Value.Should().Be(role);
    }
    
    [Fact]
    public void can_NOT_create_rolepermission_with_invalid_role()
    {
        // Arrange
        var rolePermission = () => RolePermission.Create(new RolePermissionForCreation()
        {
            Permission = _faker.PickRandom(Permissions.List()),
            Role = _faker.Lorem.Word()
        });

        // Act + Assert
        rolePermission.Should().Throw<InvalidSmartEnumPropertyName>();
    }
    
    [Fact]
    public void can_NOT_create_rolepermission_with_invalid_permission()
    {
        // Arrange
        var rolePermission = () => RolePermission.Create(new RolePermissionForCreation()
        {
            Role = _faker.PickRandom(Role.ListNames()),
            Permission = _faker.Lorem.Word()
        });

        // Act + Assert
        rolePermission.Should().Throw<ValidationException>();
    }
}