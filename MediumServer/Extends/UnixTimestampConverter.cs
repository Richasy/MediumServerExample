﻿using Newtonsoft.Json;
using System;

namespace MediumServer.Extends
{
    internal class UnixTimestampConverter:JsonConverter
    {
        /// <summary>
        /// Start time
        /// </summary>
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1);
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((DateTime)value - _epoch).TotalMilliseconds.ToString());
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == new DateTime().GetType();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var timestamp = reader.Value as long?;
            return !timestamp.HasValue ? null : Tools.FromUnixTimestampMillSeconds(timestamp);
        }
    }
}
