using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

public class PrestigesData
{
    [JsonProperty] public PrestigeType type;

    public string Name => Localization.TranslateDefault($"PRESTIGES_NAME_{type.ToString().ToUpper()}");

    public double RewardEffect(PrestigeType argType, int argLevel) // TODO
    {
        // rank.js l.475

        return argType switch
        {
            PrestigeType.Prestige => argLevel switch
            {
                3 => 1,
                5 => 1,
                6 => 1,
                // ...
                _ => 0,
            },
            PrestigeType.Honor => argLevel switch
            {
                4 => 1,
                6 => 1,
                8 => 1,
                // ...
                _ => 0,
            },
            // ...
            _ => 0,
        };
    }
}

[JsonConverter(typeof(StringEnumConverter))]
public enum PrestigeType
{ 
    Prestige = 0,
    Honor,
    Glory,
    Renown,
    Valor
}