using System.Text.Json;
using System.Text.Json.Serialization;
using Overlay.Data.Enums;

namespace Overlay.Data.Converters;

public class SC2ScenesArrayToEnumConverter : JsonConverter<SC2Scene>
{
    public override SC2Scene Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            return SC2Scene.None;
        }

        var SC2Scenes = new List<string>();

        while (reader.Read() && reader.TokenType == JsonTokenType.String)
        {
            SC2Scenes.Add(reader.GetString());
        }

        if (SC2Scenes.Count == 0)
        {
            return SC2Scene.None;
        }

        if (SC2Scenes.Contains("ScreenReplay/ScreenReplay"))
        {
            return SC2Scene.Replay;
        }

        if (SC2Scenes.Contains("ScreenCoopCampaign/ScreenCoopCampaign"))
        {
            return SC2Scene.Coop;
        }

        if (SC2Scenes.Contains("ScreenSingle/ScreenSingle"))
        {
            return SC2Scene.Campaign;
        }

        switch (SC2Scenes?.LastOrDefault())
        {
            case "ScreenLoading/ScreenLoading":
                return SC2Scene.Loading;
            case "ScreenMultiplayer/ScreenMultiplayer":
                return SC2Scene.Multiplayer;
            case "ScreenCustom/ScreenCustom":
                return SC2Scene.Custom;
            case "ScreenScore/ScreenScore":
                return SC2Scene.Score;
            case "ScreenCollection/ScreenCollection":
                return SC2Scene.Collection;
            case "ScreenUserProfile/ScreenUserProfile":
                return SC2Scene.Profile;
            default:
                return SC2Scene.None;
        }
    }

    public override void Write(Utf8JsonWriter writer, SC2Scene SC2Scene, JsonSerializerOptions options)
    {
        writer.WriteStringValue(SC2Scene.ToString());
    }
}