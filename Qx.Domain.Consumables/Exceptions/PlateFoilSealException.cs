using Qx.Domain.Consumables.Interfaces;

namespace Qx.Domain.Consumables.Exceptions;

public class PlateFoilSealException(IPlate plate, string operation, int columnIndex) 
    : InvalidOperationException($"{plate.Name}'s well column ({columnIndex}) is still foil sealed, operation ({operation}) cannot be performed.")
{
    
}