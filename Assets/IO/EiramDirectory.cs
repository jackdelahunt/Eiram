﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Eiram;
using static Eiram.Handles;

namespace IO
{
    public class EiramDirectory
    {
        public readonly string Path;
        public readonly List<EiramDirectory> SubDirectories;
        public readonly List<EiramFile> SubFiles;

        public EiramDirectory(string path)
        {
            this.Path = path;
            this.SubDirectories = new List<EiramDirectory>();
            this.SubFiles = new List<EiramFile>();
            
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return;
            }
            
            foreach (var subDirPath in Directory.EnumerateDirectories(path))
            {
                SubDirectories.Add(new EiramDirectory(subDirPath));
            }
            
            foreach (var subFilePath in Directory.EnumerateFiles(path))
            {
                SubFiles.Add(new EiramFile(subFilePath));
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
                if (GetSubDirectory(pathOnDisk).IsNone)
                {
                    SubDirectories.Add(new EiramDirectory(pathOnDisk));
                }
            }
            
            var subFilesOnDisk = Directory.EnumerateFiles(Path);

            SubFiles.RemoveAll((file) =>
            {
                return !subFilesOnDisk.Contains(file.Path);
            });

            foreach (var pathOnDisk in subFilesOnDisk)
            {
                if (GetSubDirectory(pathOnDisk).IsNone)
                {
                    SubFiles.Add(new EiramFile(pathOnDisk));
                }
            }
        }
        
        public EiramDirectory CreateSubDirectory(string name)
        {
            return new EiramDirectory(Directory.CreateDirectory($"{Path}/name").FullName);
        }

        public Option<EiramDirectory> GetSubDirectory(string name)
        {
            for (int i = 0; i < SubDirectories.Count; i++)
            {
                if (SubDirectories[i].Path.Equals(name))
                    return SubDirectories[i];
            }

            return None<EiramDirectory>();
        }

        public bool HasSubDirectory(string name)
        {
            foreach (var subDirectory in SubDirectories)
            {
                if (subDirectory.Name().Equals(name))
                    return true;
            }

            return false;
        }
        
        public Option<EiramFile> GetFile(string name)
        {
            for (int i = 0; i < SubFiles.Count; i++)
            {
                if (SubFiles[i].Name().Equals(name))
                    return SubFiles[i];
            }

            return None<EiramFile>();
        }

        public DateTime LastWriteTime()
        {
            return Directory.GetLastWriteTime(Path);
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