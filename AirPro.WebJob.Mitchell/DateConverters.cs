using System;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AirPro.WebJob.Mitchell
{
    class ScanTimeStampConverter : IsoDateTimeConverter
    {
        public ScanTimeStampConverter()
        {
            base.DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
        }
    }
    class UploadTimeStampConverter : IsoDateTimeConverter
    {
        public UploadTimeStampConverter()
        {
            base.DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
        }
    }

    class LocalTimeStampConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(DateTimeOffset);
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Check Value.
            if (!(value is DateTimeOffset input)) return;

            // Lookup Timezone Abbreviation.
            var timeZone = TimeZoneInfo.GetSystemTimeZones()
                .Where(tz => tz.BaseUtcOffset == input.Offset && (tz.DisplayName.Contains("US") || tz.DisplayName.Contains("Canada")))
                .Select(tz => Regex.Replace(tz.IsDaylightSavingTime(input) ? tz.DaylightName : tz.StandardName, @"[a-z\s]", ""))
                .FirstOrDefault();

            // Generate Output.
            var output = input.ToString($"yyyy-MM-ddTHH:mm:ss { timeZone } zz00");

            // Write Value.
            writer.WriteValue(output);
        }

        public override bool CanRead { get; } = false;
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
