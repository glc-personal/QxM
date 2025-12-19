using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Qx.Core.Configuration.Interfaces;
using Qx.Domain.Locations.Enums;
using Qx.Domain.Locations.Exceptions;
using Qx.Domain.Locations.Implementations;

namespace Qx.Domain.Locations;

public class DeckSlotConfiguration : IConfiguration<DeckSlotConfigModel, Enum>
{
    public DeckSlotConfigModel Model { get; private set; }
    
    public void GetValue(List<Enum> keys, ref object? value)
    {
        if (!Model.ContainsKey((DeckSlotNames)keys[0]))
            throw new ConfigurationKeyNotFoundException(nameof(DeckSlotConfiguration), keys[0].ToString());
        var batch = Model[(DeckSlotNames)keys[0]];
        if (!batch.ContainsKey((BatchNames)keys[1]))
            throw new ConfigurationKeyNotFoundException(nameof(DeckSlotConfiguration), keys[1].ToString());
        var pos = batch[(BatchNames)keys[1]];
        value = pos;
    }

    public void Configure(string filePath)
    {
        throw new NotImplementedException();
    }

    public void Configure(JsonObject jsonObject)
    {
        try
        {
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(namingPolicy: null, allowIntegerValues: true) }
            };
            
            Model = JsonSerializer.Deserialize<DeckSlotConfigModel>(jsonObject, options);
        }
        catch (JsonException ex)
        {
            throw new ApplicationException("Failed to parse deck slot config file", ex);
        }
    }

    public event EventHandler? ConfigurationChanged;
}