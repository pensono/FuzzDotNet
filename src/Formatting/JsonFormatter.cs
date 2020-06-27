using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace FuzzDotNet.Formatting
{
    public class JsonFormatter
    {
        private static JsonSerializerOptions DefaultSerializationOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
        };

        private JsonSerializerOptions _serializerOptions;

        public JsonFormatter()
            : this(DefaultSerializationOptions)
        {
        }

        public JsonFormatter(JsonSerializerOptions serializerOptions)
        {
            _serializerOptions = serializerOptions;
        }

        public string Format(Counterexample counterexample)
        {
            var dictionary = counterexample.Arguments.ToDictionary(a => a.Name, a => a.Value);

            return JsonSerializer.Serialize(dictionary, _serializerOptions);
        }
    }
}
