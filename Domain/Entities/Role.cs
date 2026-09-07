namespace Domain.Entities;

public class Role
{   
    public const string Admin = "Admin";
    public const string User = "User";
    public const int AdminId = 1;
    public const int UserId = 2;
    
    public int Id { get; init; }
    public string Name { get; init; }=string.Empty;
    
    public static string GetNameById(int id) => id switch
    {
        AdminId => Admin,
        UserId => User,
        _ => string.Empty
    };
}
