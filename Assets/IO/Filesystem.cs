using System.Collections.Generic;
using System.IO;
using Eiram;
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
                Data = new EiramDirectory($"{savePath}/Data"),
                Region = new EiramDirectory($"{savePath}/Region"),
                World = new EiramFile($"{savePath}/world.data")
            };
        }

        public static EiramFile SaveTo(object data, string fileName, EiramDirectory directory)
        {
            var filePath = $"{directory.Path}/{fileName}";
            Serialize.Out(data,  filePath);
            var file = new EiramFile(filePath);
            directory.SubFiles.Add(file);
            return file;
        }
        
        public static void SaveTo(object data, EiramFile file)
        {
            Serialize.Out(data,  file.Path);
        }
        
        public static Option<T> LoadFrom<T>(string fileName, EiramDirectory directory)
        {
            var filePath = $"{directory.Path}/{fileName}";
            return Serialize.In<T>(filePath);
        }
        
        public static Option<T> LoadFrom<T>(EiramFile file)
        {
            return Serialize.In<T>(file.Path);
        }

        public static bool SaveExists(string saveName)
        {
            return Directory.Exists($"{Application.persistentDataPath}/{saveName}");
        }

        public static List<EiramDirectory> AllSaves()
        {
            var saveDirectories = new List<EiramDirectory>();
            var savesDirectory = new EiramDirectory(Application.persistentDataPath);
            foreach (var save in savesDirectory.SubDirectories)
            {
                if (save.HasSubDirectory("Region") && save.HasSubDirectory("Data"))
                {
                    saveDirectories.Add(save);
                }
            }

            return saveDirectories;
        }
    }
    
    public class Save
    {
        public EiramDirectory Region;
        public EiramDirectory Data;
        public EiramFile World;
    }
}