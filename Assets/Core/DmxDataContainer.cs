using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using SFB;
using UnityEngine;
using File = UnityEngine.Windows.File;

namespace Core
{
    [Serializable]
    public class DmxDataContainer
    {
        [SerializeField]
        public Dictionary<long, DmxUniverseContainer> keyframes = new Dictionary<long, DmxUniverseContainer>();

        public void Save()
        {
            var path = StandaloneFileBrowser.SaveFilePanel("Save recording", Application.dataPath, "recording", "bin");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            
            var stream = new MemoryStream();
            var writer = new BsonWriter(stream);
            var serializer = new JsonSerializer();
            serializer.Serialize(writer, this);
            writer.Close();
            
            Debug.Log($"Saved dmx data with {keyframes.Count} keyframes.");
            
            File.WriteAllBytes(path, stream.ToArray());
        }

        public static DmxDataContainer Load()
        {
            var path = StandaloneFileBrowser.OpenFilePanel("Load recording", Application.dataPath, "bin", false)[0];
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File not found: {path}");
            }
            var bytes = File.ReadAllBytes(path);
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