using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Racer.SaveSystem
{
    /// <summary>
    /// Efficiently converts data types to their target type.
    /// The tokens are types <see cref="SaveSystem"/> can handle.
    /// </summary>
    internal class CustomConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(object) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jToken = JToken.ReadFrom(reader);

            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    return jToken.Value<int>();
                case JsonToken.String:
                    return jToken.Value<string>();
                case JsonToken.Float:
                    return jToken.Value<float>();
                case JsonToken.Boolean:
                    return jToken.Value<bool>();
                default:
                    throw new ArgumentException($"Unknown JsonToken: '{reader.TokenType}'.");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }
    }
}