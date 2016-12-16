using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSharingApplication.Cache
{
    public class PhotoFile
    {
        public byte[] File { get; set; }
        public string ImageMimeType { get; set; }
    }
}
