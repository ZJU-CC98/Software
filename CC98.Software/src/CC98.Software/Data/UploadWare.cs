using Microsoft.AspNetCore.Http;

namespace CC98.Software.Data
{
    public class UploadWare
    {
        public string Name
        {
            get; set;
        }
        public Platform Platform
        {
            get; set;
        }

        public string Introduction
        {
            get; set;
        }
        public IFormFile File
        {
            get; set;
        }
        public IFormFile Photo
        {
            get; set;
        }
       
    }
}