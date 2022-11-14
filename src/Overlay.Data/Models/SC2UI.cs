using System.Text.Json.Serialization;
using Overlay.Data.Converters;
using Overlay.Data.Enums;

namespace Overlay.Data.Models;

public class SC2UI
{
    [JsonPropertyName("activeScreens")]
    [JsonConverter(typeof(SC2ScenesArrayToEnumConverter))]
    public SC2Scene Scene { get; set; }
}