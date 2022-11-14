using System.Text.Json.Serialization;
using Overlay.Data.Converters;
using Overlay.Data.Enums;

namespace Overlay.Data.Models;

public class SC2Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SC2Type Type { get; set; }
    [JsonConverter(typeof(RaceStringToEnumConverter))]
    public SC2Race Race { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SC2Result Result { get; set; }
    public int MMR { get; set; }
}