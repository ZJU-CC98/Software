﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CC98.Software.Data
{
    public class Comment
    {
        public string Contents { get; set; }
        public DateTime Commenttime { get; set; }
        public Software CommentBelongto { get; set; }
        public string Name { get; set; }
}

}
