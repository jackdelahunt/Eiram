using System.IO;

namespace IO
{
    public class EiarmFile
    {
        public string Path;
        public EiarmFile(string path)
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