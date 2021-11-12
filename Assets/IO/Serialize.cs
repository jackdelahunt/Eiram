using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Eiram;
using static Eiram.Handles;

namespace IO
{
    public class Serialize  
    {
        public static void Out(object data, string filePath)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePath, FileMode.Create);

            bf.Serialize(fs, data);
            fs.Close();
        }
        
        public static Option<T> In<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return None<T>();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePath, FileMode.Open);

            try
            {
                T data = (T) bf.Deserialize(fs);
                fs.Close();
                return data;
            }
            catch
            {
                fs.Close();
                return None<T>();
            }
        }
    }
}