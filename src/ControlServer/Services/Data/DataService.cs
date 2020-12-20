using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text.Json;

using ControlServer.Data;

namespace ControlServer.Services.Data
{
    public sealed class DataService : IDataService
    {
        private readonly string path;

        private readonly ImmutableArray<Light> lights;

        public DataService(string path)
        {
            this.path = path;

            using (Stream stream = new FileStream(this.path, FileMode.Open, FileAccess.Read))
            using (TextReader reader = new StreamReader(stream))
            {
                string text = reader.ReadToEnd();
                Config config = JsonSerializer.Deserialize<Config>(text, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                });

                this.lights = config.Lights.ToImmutableArray();
            }
        }
        
        public ImmutableArray<Light> Lights => this.lights;
    }
}
