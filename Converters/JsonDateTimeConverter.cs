using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Converters
{
    internal class JsonDateTimeConverter : JsonConverter<DateTime?>
    {
        private const string CustomFormat = "yyyy-MM-dd HH:mm:ss";

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();

            if (str == null)
                return null;

            if (DateTime.TryParseExact(str, CustomFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                return dt;

            return DateTime.Parse(str);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }
            writer.WriteStringValue(value?.ToString(CustomFormat));
        }
    }
}
