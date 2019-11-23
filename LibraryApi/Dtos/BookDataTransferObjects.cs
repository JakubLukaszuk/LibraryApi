using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Dtos
{
    public class BookDataTransferObjects
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Ibsn { get; set; }
        public DateTime? DatePublished {get; set;} 
    }
}
