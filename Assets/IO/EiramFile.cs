using System.IO;

namespace IO
{
    public class EiramFile
    {
        public string Path;
        public EiramFile(string path)
        {
            this.Path = path;
        }
        
        public void Delete()
        {
            File.Delete(Path);
        }

        public bool Exists()
        {
            return File.Exists(Path);
        }
        
        public string Name()
        {
            return System.IO.Path.GetFileName(Path);
        }
    }
}