using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient
{
    public class FileManager
    {
        public static string CalculateFileId(string filePath)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }

    public class FileDetails
    {
        public string FileId { get; set; }
        public string Filename { get; set; }
        public long FileSize { get; set; }
        public string PublishDate { get; set; }

    }

    public class UserDetails
    {
        public string Name { get; set; }
        public string IP { get; set; }
        public int PortIn { get; set; }
        public int PortOut { get; set; }

        public override string ToString()
        {
            return "NAME " + Name + " IP " + IP + " PORTIN " + PortIn + " PORTOUT " + PortOut;
        }
    }
}
