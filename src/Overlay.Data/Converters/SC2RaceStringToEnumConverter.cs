using System.Text.Json;
using System.Text.Json.Serialization;
using Overlay.Data.Enums;

namespace Overlay.Data.Converters;

public class RaceStringToEnumConverter : JsonConverter<SC2Race>
{
    public override SC2Race Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            return SC2Race.Unknown;
        }

        switch (reader.GetString())
        {
            case "Prot":
                return SC2Race.Protoss;
            case "Terr":
                return SC2Race.Terran;
            case "Zerg":
                return SC2Race.Zerg;
            case "random":
                return SC2Race.Random;
            default:
                return SC2Race.Unknown;
        }
    }

    public override void Write(Utf8JsonWriter writer, SC2Race race, JsonSerializerOptions options)
    {
        writer.WriteStringValue(race.ToString());
    }
}