using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        public IFormFile  File
        {
            get; set;
        }
       public IFormFile Photo
        {
            get; set;

        }
    }
    public class Category
    {
        public int Id
        {
            get; set; 
        }

        public string Name
        {
            get; set;
        }
    }
    public enum Platform
    {                
        Windows,Mac,Android,Ios
    }
    public class Software
    {
        public string Name
        {
            get; set;
        }

        public string PhotoLocation
        {
            get; set;
        }

        public long Size
        {
            get; set;
        }

        public Platform Platform
        {
            get; set;          
        }

        public Category Class
        {
            get; set;         
        }

        public string Introduction
        {
            get; set;         
        }

        public DateTimeOffset   UpdateTime
        {
            get; set;           
        }

        public int DownloadNum
        {
            get; set;
        }

        public int Id
        {
            get; set;
        }

        public IFormFile File
        {
            get; set; 
        }

        public string FileLocation
        {
            get; set;
        }

        public IFormFile Photo
        {
            get; set;
            
        }
    }
}
