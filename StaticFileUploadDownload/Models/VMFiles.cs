using System;

namespace StaticFileUploadDownload.Models
{
    public class VMFiles
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime? UploadDate { get; set; }
    }
}
