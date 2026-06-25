using Domain.Abstractions;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User:Entity
{   
    private User(Guid id, FullName fullName, string username,
        Email email, Password password):base(id)
    {
       
        FullName=fullName;
        Username = username;
        Email = email;
        Password = password;
    }

    protected User()
    {
        FullName=null!;
        Username = null!;
        Password = null!;
        Email = null!;
    }
    
    public FullName FullName { get; private set; }
    public string Username { get; private set; }
    public Password Password { get; private set; }
    public Email Email { get; private set; }

    public static User Create( string firstName,
        string lastName, string username, 
        string email, string password,
        IPasswordHasher passwordHasher)
    {
       
        var user = new User(Guid.NewGuid(),
            FullName.Create(firstName, lastName), 
            username,Email.Create(email), Password.Create(password, passwordHasher));
        return user;
    }
    
}
