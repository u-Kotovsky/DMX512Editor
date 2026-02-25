using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using UnityEngine;
using File = UnityEngine.Windows.File;

namespace Core
{
    [Serializable]
    public class DmxDataContainer
    {
        [SerializeField]
        public Dictionary<long, DmxUniverseContainer> keyframes = new Dictionary<long, DmxUniverseContainer>();

        public void Save(string pathToFile)
        {
            if (File.Exists(pathToFile))
            {
                // todo: add a random string to name
                Debug.LogError("File already exists: " + pathToFile);
                return;
            }
            
            var stream = new MemoryStream();
            var writer = new BsonWriter(stream);
            var serializer = new JsonSerializer();
            serializer.Serialize(writer, this);
            writer.Close();
            
            Debug.Log($"Saved dmx data with {keyframes.Count} keyframes.");
            
            File.WriteAllBytes(pathToFile, stream.ToArray());
        }

        public static DmxDataContainer Load(string pathToFile)
        {
            var bytes = File.ReadAllBytes(pathToFile);
            var ms = new MemoryStream(bytes);
            var reader = new BsonReader(ms);
            var serializer = new JsonSerializer();
            var data = serializer.Deserialize<DmxDataContainer>(reader);
            reader.Close();
            
            Debug.Log($"Loaded dmx data with {data.keyframes.Count} keyframes.");

            return data;
        }
    }
}