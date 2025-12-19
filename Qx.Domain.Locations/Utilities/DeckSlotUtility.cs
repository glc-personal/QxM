using System.Collections;
using Qx.Core.Configuration.Interfaces;
using Qx.Domain.Locations.Implementations;

namespace Qx.Domain.Locations.Utilities;

public class DeckSlotUtility 
{
    private IConfiguration<DeckSlotConfigModel, Enum> _configuration;

    public DeckSlotUtility(IConfiguration<DeckSlotConfigModel, Enum> configuration)
    {
        _configuration = configuration;
    }
    
    /// <summary>
    /// Get the configured coordinate position of a deck slot position
    /// </summary>
    /// <param name="position">deck slot position of interest</param>
    /// <returns>configured coordinate position for this deck slot position</returns>
    /// <exception cref="NullReferenceException">if the position cannot be found due to an invalid configuration</exception>
    public CoordinatePosition GetCoordinatePosition(DeckSlotPosition position)
    {
        object? posObj = null;
        _configuration.GetValue([position.Name, position.Batch], ref posObj);
        if (posObj == null)
            throw new NullReferenceException($"Invalid configuration for {position.Name}-{position.Batch} position");
        return (CoordinatePosition)posObj;
    }
}