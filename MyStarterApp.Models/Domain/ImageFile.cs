using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStarterApp.Models.Domain
{
    public class ImageFile
    {
        public int Id { get; set; }
        public string ImageFileName { get; set; }
        public string SystemFileName { get; set; }
        public int ImageFileType { get; set; }
        public string Location { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public byte[] ByteArray { get; set; }
        public string Extension { get; set; }

        // For variables set in the web application layer
        public string ImageUrl { get; set; }
        public string EncodedImageFile { get; set; }
        public string FileExtension { get; set; }
    }
}
