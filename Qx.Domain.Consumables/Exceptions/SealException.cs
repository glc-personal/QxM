namespace Qx.Domain.Consumables.Exceptions;

public class SealException() 
    : InvalidOperationException($"Cannot change volumes while container is still sealed")
{
    
}