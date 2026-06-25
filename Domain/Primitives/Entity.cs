namespace Domain.Primitives;

public abstract class Entity:IEquatable<Entity>
{
    protected Entity(Guid id)
    {
        id = Guid.NewGuid();
    }
    protected Entity()
    {}

    public Guid Id { get; private init; }

    public bool Equals(Entity? other)
    {
        if (other is null || other.GetType() != GetType()) return false;
        
        if (ReferenceEquals(this, other)) return true;
        
        return other.Id == Id;
        
    }

    public override bool Equals(object? obj)
    {   
        if (ReferenceEquals(this, obj)) return true;
        
        if (obj is null || obj.GetType() != GetType()) return false;

        return obj is Entity entity && entity.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode()*13;
    }
    
    public static bool operator ==(Entity? left, Entity? right)=>
        left is not null && right is not null && left.Equals(right);

    public static bool operator !=(Entity? left, Entity? right) =>
        !(left == right);
}