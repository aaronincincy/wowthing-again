﻿using System.Text.Json;
using Wowthing.Backend.Models.Static;

namespace Wowthing.Backend.Converters.Static;

public class StaticCurrencyConverter : System.Text.Json.Serialization.JsonConverter<StaticCurrency>
{
    public override StaticCurrency Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, StaticCurrency value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.Id);
        writer.WriteNumberValue(value.CategoryId);
        writer.WriteNumberValue(value.MaxPerWeek);
        writer.WriteNumberValue(value.MaxTotal);
        writer.WriteStringValue(value.Name);
        writer.WriteEndArray();
    }
}
