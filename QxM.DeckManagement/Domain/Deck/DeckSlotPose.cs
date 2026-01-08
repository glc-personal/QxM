using Qx.Core.Measurement;

namespace QxM.DeckManagement.Domain.Deck;

/// <summary>
/// Deck slot pose to define where the deck slot is in the deck's coordinate frame
/// </summary>
/// <param name="X">Distance in mm along the x-axis to the deck's origin.</param>
/// <param name="Y">Distance in mm along the y-axis to the deck's origin.</param>
/// <param name="Theta">Rotational offset of the deck slot</param>
public readonly record struct DeckSlotPose(Mm X, Mm Y, AngleDegrees Theta);
