
namespace Domain.Entities;

public class UserRole
{
    public Guid UserId { get; set; }
    public int RoleId { get; set; }
    
    private UserRole( Guid userId, int roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
    
    private UserRole()
    { }
    
    internal static UserRole Create(Guid userId, int roleId)
    {
        return new UserRole(userId, roleId);
    }
}