using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CC98.Software.Data
{
    public class Software
    {
        /// <summary>
        /// 常用软件标记。
        /// </summary>
        public bool IsFrequent
        {
            get; set;
        }
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

        public DateTimeOffset UpdateTime
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

        public string FileLocation
        {
            get; set;
        }

    }
}
