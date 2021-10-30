using System.IO;
using Eiram;
using UnityEngine.WSA;
using static Eiram.Handles;
using UnityEngine;
using Application = UnityEngine.Application;

namespace IO
{
    public static class Filesystem
    {
        public static Save CreateSave(string saveName)
        {
            var savePath = $"{Application.persistentDataPath}/{saveName}";
            return new Save
            {
                Data = new EiarmDirectory($"{savePath}/Data"),
                Region = new EiarmDirectory($"{savePath}/Region"),
                World = null
            };
        }

        public static EiarmFile SaveTo(object data, string fileName, EiarmDirectory directory)
        {
            var filePath = $"{directory.Path}/{fileName}";
            Serialize.Out(data,  filePath);
            var file = new EiarmFile(filePath);
            directory.SubFiles.Add(file);
            return file;
        }
        
        public static Some<T> LoadFrom<T>(string fileName, EiarmDirectory directory)
        {
            var filePath = $"{directory.Path}/{fileName}";
            return Serialize.In<T>(filePath);
        }

        public static bool SaveExists(string saveName)
        {
            return Directory.Exists($"{Application.persistentDataPath}/{saveName}");
        }
    }
    
    public class Save
    {
        public EiarmDirectory Region;
        public EiarmDirectory Data;
        public EiarmFile World;
    }
}