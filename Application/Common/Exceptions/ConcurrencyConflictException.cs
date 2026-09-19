namespace Application.Common.Exceptions;

public class ConcurrencyConflictException:Exception
{
  public ConcurrencyConflictException(Exception innerEx)
    :base
     ("The entity you are trying to update  has been modified by" +
      " another user.Please reload the entity and try again.", innerEx)
  { }
}
