using System.Text.Json.Nodes;
using Qx.Domain.Locations.Enums;
using Qx.Domain.Locations.Implementations;

namespace Qx.Tests.Domain.Utilities;

public static class TestHelperUtility
{
    public static JsonObject CreateDeckSlotConfigObject(SlotName name, BatchNames batch, CoordinatePosition pos)
    {
        JsonObject jsonObject = new JsonObject
        {
            [name.ToString()] = new JsonObject
            {
                [batch.ToString()] = new JsonObject
                {
                    ["X"] = pos.X,
                    ["Y"] = pos.Y,
                    ["Z"] = pos.Z,
                    ["Units"] = pos.Units.ToString(),
                }
            }
        };
        return jsonObject;
    }
}