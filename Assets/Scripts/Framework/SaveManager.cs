using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Framework
{
    public class SaveManager
    {
        public static void SaveWithJson(object obj, string path)
        {
            var json = JsonUtility.ToJson(obj);
            using (FileStream file = File.Create(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, json);
            }
        }

        public static bool LoadWithJson<T>(ref T obj, string path)
        {
            if (!File.Exists(path)) return false;

            using (FileStream file = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var json = (string)formatter.Deserialize(file);
                JsonUtility.FromJsonOverwrite(json, obj);
            }

            return true;
        }
    }
}