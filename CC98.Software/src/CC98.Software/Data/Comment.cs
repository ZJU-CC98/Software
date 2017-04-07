using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace CC98.Software.Data
{
    public class Comment
    {
        public string Contents { get; set; }
        public DateTimeOffset Commenttime { get; set; }
        public Software CommentBelongto { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
}

}
