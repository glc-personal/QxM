namespace Qx.Domain.Consumables.Exceptions;

public class OutOfUsesException(int uses, int maxUses) 
    : InvalidOperationException($"Out of uses for reuse policy ({uses}/{maxUses})")
{
    
}