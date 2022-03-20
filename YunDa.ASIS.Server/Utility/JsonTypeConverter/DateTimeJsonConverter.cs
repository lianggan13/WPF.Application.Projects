

using Newtonsoft.Json;

namespace YunDa.ASIS.Server.Utility.JsonTypeConverter
{
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            DateTime.TryParse(reader.Value?.ToString(), out var dateTime);
            return dateTime;
        }

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFF"));
        }
    }

}
