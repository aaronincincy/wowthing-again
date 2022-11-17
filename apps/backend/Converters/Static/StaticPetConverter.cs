﻿using System.Text.Json;
using Wowthing.Backend.Models.Static;

namespace Wowthing.Backend.Converters.Static;

public class StaticPetConverter : System.Text.Json.Serialization.JsonConverter<StaticPet>
{
    public override StaticPet Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, StaticPet value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.Id);
        writer.WriteNumberValue(value.SourceType);
        writer.WriteNumberValue(value.PetType);
        writer.WriteNumberValue(value.CreatureId);
        writer.WriteNumberValue(value.SpellId);
        writer.WriteStringValue(value.Name);
        writer.WriteEndArray();
    }
}
