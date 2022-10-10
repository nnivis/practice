using System.Collections.Generic;

namespace MongoApp.Models
{
    public class IndexViewModel
    {
        public FilterViewModel Filter { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}