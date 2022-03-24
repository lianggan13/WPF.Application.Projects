using Newtonsoft.Json;

namespace YunDa.ASIS.Server.Utility.JsonTypeConverter
{
    public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset ReadJson(JsonReader reader, Type objectType, DateTimeOffset existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            DateTime.TryParse(reader.Value?.ToString(), out var dateTime);
            return dateTime;
        }


        public override void WriteJson(JsonWriter writer, DateTimeOffset value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFF"));
        }
    }
}
