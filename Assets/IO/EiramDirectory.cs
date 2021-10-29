using System.Collections.Generic;
using System.IO;
using System.Linq;
using Eiram;
using static Eiram.Handles;

namespace IO
{
    public class EiarmDirectory
    {
        public readonly string Path;
        public readonly List<EiarmDirectory> SubDirectories;
        public readonly List<EiarmFile> SubFiles;

        public EiarmDirectory(string path)
        {
            this.Path = path;
            this.SubDirectories = new List<EiarmDirectory>();
            this.SubFiles = new List<EiarmFile>();
            
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return;
            }
            
            foreach (var subDirPath in Directory.EnumerateDirectories(path))
            {
                SubDirectories.Add(new EiarmDirectory(subDirPath));
            }
            
            foreach (var subFilePath in Directory.EnumerateFiles(path))
            {
                SubFiles.Add(new EiarmFile(subFilePath));
            }
        }

        public void ReEvaluate(bool recursive = true)
        {
            var subDirsOnDisk = Directory.EnumerateDirectories(Path);

            SubDirectories.RemoveAll((dir) =>
            {
                if (!subDirsOnDisk.Contains(dir.Path))
                {
                    return true;
                }
                else
                {
                    if(recursive) dir.ReEvaluate(recursive: recursive);
                    return false;
                }
            });

            foreach (var pathOnDisk in subDirsOnDisk)
            {
                if (GetSubDirectory(pathOnDisk).IsNone())
                {
                    SubDirectories.Add(new EiarmDirectory(pathOnDisk));
                }
            }
            
            var subFilesOnDisk = Directory.EnumerateFiles(Path);

            SubFiles.RemoveAll((file) =>
            {
                return !subFilesOnDisk.Contains(file.Path);
            });

            foreach (var pathOnDisk in subFilesOnDisk)
            {
                if (GetSubDirectory(pathOnDisk).IsNone())
                {
                    SubFiles.Add(new EiarmFile(pathOnDisk));
                }
            }
        }
        
        public EiarmDirectory CreateSubDirectory(string name)
        {
            return new EiarmDirectory(Directory.CreateDirectory($"{Path}/name").FullName);
        }

        public Some<EiarmDirectory> GetSubDirectory(string name)
        {
            for (int i = 0; i < SubDirectories.Count; i++)
            {
                if (SubDirectories[i].Path.Equals(name))
                    return SubDirectories[i];
            }

            return None;
        }
        
        public Some<EiarmFile> GetFile(string name)
        {
            for (int i = 0; i < SubFiles.Count; i++)
            {
                if (SubFiles[i].Name().Equals(name))
                    return SubFiles[i];
            }

            return None;
        }

        public void Delete(bool recursive = true)
        {
            Directory.Delete(Path, recursive);
        }

        public bool Exists()
        {
            return Directory.Exists(Path);
        }

        public string Name()
        {
            return System.IO.Path.GetFileName(Path);
        }
    }
}