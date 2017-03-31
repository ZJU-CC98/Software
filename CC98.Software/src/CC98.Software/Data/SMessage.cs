using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CC98.Software.Data
{
    public class SMessage
    {
        public string Content
        {
            get; set;
        }
        public string Title { get; set; }
        public int Id { get; set; }
        public string Receivername { get; set; }
    }
}
