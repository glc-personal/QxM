namespace Qx.Domain.Deck.Exceptions;

public class DeckSlotPoseException(DeckSlotPose pose, DeckGeometry geometry) 
    : ArgumentOutOfRangeException($"{nameof(DeckSlotPose)} ({pose.X}, {pose.Y}) is out of the {nameof(DeckGeometry)}'s range ({geometry.DepthX}, {geometry.LengthY})");