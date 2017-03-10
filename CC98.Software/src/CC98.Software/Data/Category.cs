using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CC98.Software.Data
{
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

        public Category Parent
        {
            get; set;
        }

        [InverseProperty("Parent")]
        public virtual ICollection<Category> Children { get; set; } = new Collection<Category>();
   
    }
}