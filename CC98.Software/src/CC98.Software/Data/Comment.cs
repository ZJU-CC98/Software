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
        public string Content { get; set; }
        public DateTimeOffset Time { get; set; }
        public Software Software { get; set; }
        public string UserName { get; set; }
        public int Id { get; set; }
}

}
