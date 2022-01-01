using System;

namespace StaticFileUploadDownload.Models
{
    public class VMDwonloadFiles
    {
        public int idx { get; set; }
        public string OriginalName { get; set; }
        public string StoredUpName { get; set; }
        public string Type { get; set; }
        public DateTime? UploadDate { get; set; }
    }
}
