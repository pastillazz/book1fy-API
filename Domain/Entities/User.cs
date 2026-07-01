using Domain.Abstractions;
using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class User:AggregateRoot
{   
    private User(Guid id, FullName fullName, string username,
        Email email, Password password, string phoneNumber):base(id)
    {
       
        FullName=fullName;
        Username = username;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
    }

    protected User()
    {
        FullName=null!;
        Username = null!;
        Password = null!;
        Email = null!;
        PhoneNumber = null!;
    }
    
    public FullName FullName { get; private set; }
    public string Username { get; private set; }
    public Password Password { get; private set; }
    public string PhoneNumber { get; private set; }
    public Email Email { get; private set; }
    
    public static Result<User> Create( string firstName,
        string lastName, string username, 
        string email, string password, string phoneNumber,
        IPasswordHasher passwordHasher)
    {   
        var fullNameResult = FullName.Create(firstName, lastName);  
        if (fullNameResult.IsFailure) return fullNameResult.Error!;
        
        var emailResult = Email.Create(email);
        if (emailResult.IsFailure) return emailResult.Error!;
        
        var passwordResult = Password.Create(password, passwordHasher);
        if (passwordResult.IsFailure) return passwordResult.Error!;

        var user = new User(Guid.NewGuid(),
            fullNameResult.Value,
            username, emailResult.Value,
            passwordResult.Value, phoneNumber);
        
        var userEvent= new UserCreatedDomainEvent(Guid.NewGuid(),user.Id);
        user.RaiseDomainEvent(userEvent);
        return user;
    }
    
}


